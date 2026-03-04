namespace DungeonExplorer.Api.Domain;

public interface IJwtKeyProvider
{
    string GetOrCreateKey();
}
