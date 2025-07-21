using System;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GenresClient(HttpClient httpClient)
{
    public async Task<List<Genre>> GetGenresAsync()
    {
        return await httpClient.GetFromJsonAsync<List<Genre>>("genres") ?? new List<Genre>();
    }
}
