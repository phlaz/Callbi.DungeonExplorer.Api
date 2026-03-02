using System.Net.Http.Json;

namespace DungeonExplorer.Client.Maui.Dungeon;

public class DungeonService
{
    private readonly HttpClient httpClient;

    public DungeonService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dungeon?> CreateDungeon(Dungeon dungeon)
    {
        var response = await httpClient.PostAsJsonAsync("api/dungeons", dungeon);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Dungeon>();
    }
}