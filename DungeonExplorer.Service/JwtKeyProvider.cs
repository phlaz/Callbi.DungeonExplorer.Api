namespace DungeonExplorer.Api.Service;

public class DevJwtKeyProvider(IConfiguration configuration) : IJwtKeyProvider
{
    private readonly string configKeyPath = "Jwt:Key";

    public string GetOrCreateKey()
    {
        var existingKey = configuration[configKeyPath];
        if(!string.IsNullOrWhiteSpace(existingKey))
        {
            return existingKey;
        }

        // Generate a random 256-bit key
        var keyBytes = RandomNumberGenerator.GetBytes(32);
        var newKey = Convert.ToBase64String(keyBytes);

        // Persist securely depending on environment
        // For dev, we’ll just inject into config and keeping in memory, this will be regened next time.
        // Should persist to user secrets
        configuration[configKeyPath] = newKey;

        return newKey;
    }
}

public class ProductionJwtKeyProvider(IConfiguration configuration) : IJwtKeyProvider
{
    public string GetOrCreateKey()
    {
        //Get from a keystore or secure file with restricted permissions
        return string.Empty;
    }
}