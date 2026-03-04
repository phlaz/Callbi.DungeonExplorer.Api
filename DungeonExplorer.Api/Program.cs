var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
var loggingBuilder = builder.Logging.SetMinimumLevel(LogLevel.Trace);

var connectionString = builder.Configuration.GetConnectionString(Strings.DefaultConnection);
builder.Services.AddDbContext<DungeonDBContext>(options =>
    options.UseSqlite(connectionString));

//swap out the implementation of IJwtKeyProvider for production
var keyProvider = new DevJwtKeyProvider(builder.Configuration);

// Ensure we have a valid JWT key
var jwtKey = keyProvider.GetOrCreateKey();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DungeonDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});


builder.Services.AddAuthorization();

builder.Services
    .AddControllers(options =>
    {
        //Add filters
        options.Filters.Add<ValidationFilter>();
        options.Filters.Add<ExceptionFilter>();
    })
     //Prevents automatic model state leaks
    .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

builder.Services.AddDbContext<DungeonDBContext>(opt => opt.UseSqlite(Strings.DataSource));
builder.Services.AddScoped<IDungeonRepository, DungeonRepository>();
builder.Services.AddScoped<IPathfindingService, PathfindingService>();
builder.Services.AddScoped<IDungeonService, DungeonService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.EnableAnnotations();
    config.SwaggerDoc(Strings.Version, new OpenApiInfo
    {
        Title = Strings.DungeonExplorerAPI,
        Version = Strings.Version,
        Description = "Secure API following OWASP Top 10 best practices"
    });

    config.AddSecurityDefinition(Strings.Bearer, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme.",
        Name = Strings.Authorization,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = Strings.Bearer
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = Strings.Bearer }
            },
            Array.Empty<string>()
        }
    });
});

// --- CORS (restrict origins in production) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(Strings.DefaultPolicy, policy =>
    {
        policy.WithOrigins(Strings.FrontendOrigin) // frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Security Middleware ---
if(!app.Environment.IsDevelopment())
{
    app.UseHsts(); // Enforce HTTPS Strict Transport Security
}

app.UseHttpsRedirection();
app.UseCors(Strings.DefaultPolicy);

app.UseAuthentication();   // must come before UseAuthorization
app.UseAuthorization();

// --- Global Error Handling ---
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = Strings.ApplicationJson;

        var error = new { error = "An unexpected error occurred. Please try again later." };
        await context.Response.WriteAsJsonAsync(error);
    });
});

// --- Swagger ---
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint(Strings.SwaggerUrl, $"{Strings.DungeonExplorerAPI} {Strings.Version}");
    });
}

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DungeonDBContext>();
    
    //for dev - comment out for prod
    //db.Database.EnsureDeleted();
    //db.Database.EnsureCreated();

    db.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
