var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services
    .AddControllers(options =>
    {
        //Add filters
        options.Filters.Add<ValidationFilter>();
        options.Filters.Add<ExceptionFilter>();
    })
     //Prevents automatic model state leaks
    .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

builder.Services.AddDbContext<DungeonContext>(opt => opt.UseSqlite(Strings.DataSource));
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

    // Example JWT auth scheme if you add authentication later
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
    var db = scope.ServiceProvider.GetRequiredService<DungeonContext>();
    //db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    //db.Database.Migrate();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
