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

    public void AddGame(GameDetails game)
    {
        var genres = new GenresClient().GetGenres();

        _games.Add(new GameSummary
        {
            Id = _games.Max(g => g.Id) + 1,
            Name = game.Name,
            Genre = genres.FirstOrDefault(g => g.Id.ToString() == game.GenreId)?.Name ?? "Unknown",
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        });
    }

    public GameDetails? GetGameDetails(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null) return null;

        var genres = new GenresClient().GetGenres();
        return new GameDetails
        {
            Id = game.Id,
            Name = game.Name,
            GenreId = genres.SingleOrDefault(g => string.Equals(g.Name, game.Genre, StringComparison.OrdinalIgnoreCase))?.Id.ToString() ?? string.Empty,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public void UpdateGame(GameDetails game)
    {
        var existingGame = _games.FirstOrDefault(g => g.Id == game.Id);
        if (existingGame != null)
        {
            existingGame.Name = game.Name;
            existingGame.Genre = new GenresClient().GetGenres().FirstOrDefault(g => g.Id.ToString() == game.GenreId)?.Name ?? "Unknown";
            existingGame.Price = game.Price;
            existingGame.ReleaseDate = game.ReleaseDate;
        }
    }

    public void DeleteGame(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game != null)
        {
            _games.Remove(game);
        }
    }
}
