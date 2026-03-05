namespace DungeonExplorer.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IDungeonAuthService authService, UserManager<IdentityUser> userManager, IConfiguration config, ILoggerFactory loggerFactory) : ControllerBase
{
    private readonly ILogger logger = loggerFactory.CreateLogger<AuthController>();

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]UserCredentials credentials)
    {
        //add logging
        ArgumentNullException.ThrowIfNull(credentials, nameof(credentials));
        var email = credentials.Email;
        var password = credentials.Password;

        //if no email or password, don't bother looking for the user
        if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            //to get rid of the unused logger warning
            logger.LogWarning("Email or password is empty");
            return Unauthorized();
        }

        var user = await userManager.FindByEmailAsync(email);
        if(user is null || !await userManager.CheckPasswordAsync(user, password))
        {
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwtKey = config["Jwt:Key"];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(jwtKey, "Jwt:Key");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserCredentials credentials)
    {
        //add logging
        ArgumentNullException.ThrowIfNull(credentials, nameof(credentials));
        var email = credentials.Email;
        ArgumentNullException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        var password = credentials.Password;
        ArgumentNullException.ThrowIfNullOrWhiteSpace(password, nameof(password));

        if(!authService.IsValidEmail(email))
        {
            logger.LogWarning("Email is not valid");
            return BadRequest();
        }

        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        var result = await userManager.CreateAsync(user, password);

        if(result.Succeeded)
        {
            return Ok("User created successfully");
        }

        return BadRequest(result.Errors);
    }

}