using System;
using System.Threading.Tasks;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient(HttpClient client)
{
    public async Task<List<GameSummary>> GetGamesAsync()
        => await client.GetFromJsonAsync<List<GameSummary>>("games") ?? [];

    public async Task AddGameAsync(GameDetails game)
    {
        await client.PostAsJsonAsync("games", game);
    }

    public async Task<GameDetails?> GetGameDetailsAsync(int id)
    {
        var game = await client.GetFromJsonAsync<GameDetails>($"games/{id}");
        return game;
    }

    public async Task UpdateGameAsync(GameDetails game)
    {
        await client.PutAsJsonAsync($"games/{game.Id}", game);
    }
    public async Task DeleteGameAsync(int id)
    {
        await client.DeleteAsync($"games/{id}");
    }
}
