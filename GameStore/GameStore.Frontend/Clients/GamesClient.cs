using System;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient
{
    private List<GameSummary> _games = new List<GameSummary>
        {
            new GameSummary
            {
                Id = 1,
                Name = "Game One",
                Genre = "Action",
                Price = 29.99m,
                ReleaseDate = new DateOnly(2023, 10, 1)
            },
            new GameSummary
            {
                Id = 2,
                Name = "Game Two",
                Genre = "Adventure",
                Price = 49.99m,
                ReleaseDate = new DateOnly(2023, 11, 15)
            },
            new GameSummary
            {
                Id = 3,
                Name = "Game Three",
                Genre = "RPG",
                Price = 59.99m,
                ReleaseDate = new DateOnly(2024, 2, 20)
            }
        };

    public List<GameSummary> GetGames() => _games;
}
