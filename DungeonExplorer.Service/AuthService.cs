
namespace DungeonExplorer.Api.Service;

public interface IDungeonAuthService
{
    bool IsValidEmail(string email);
}

public class DungeonAuthService(ILoggerFactory loggerFactory) : IDungeonAuthService
{
    private readonly ILogger logger = loggerFactory.CreateLogger<DungeonAuthService>();
    
    private readonly Regex regex = new Regex("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$");

    public bool IsValidEmail(string email)
    {
        //check for valid email. This is a ver basic email format regex. Could be improved to RFC 5322 compliance.
        var result = regex.IsMatch(email);
        if(!result)
        {
            logger.LogWarning("Email is not a valid format");
        }
        return result;
    }
}
