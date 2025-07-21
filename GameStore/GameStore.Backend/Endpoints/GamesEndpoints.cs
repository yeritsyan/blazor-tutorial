using System;
using GameStore.Backend.Data;
using GameStore.Backend.Dtos;
using GameStore.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Backend.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";

    private static readonly List<GameDto> games =
    [
        new (1, "Game 1", "Genre 1", 19.99m, new DateOnly(2023, 1, 1)),
        new (2, "Game 2", "Genre 2", 29.99m, new DateOnly(2023, 2, 1)),
        new (3, "Game 3", "Genre 3", 39.99m, new DateOnly(2023, 3, 1))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", () => Results.Ok(games));

        group.MapGet("/{id:int}", (int id) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            return game is not null ? Results.Ok(game) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = createGameDto.Name,
                GenreId = createGameDto.GenreId,
                Price = createGameDto.Price,
                ReleaseDate = createGameDto.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            var savedGame = dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefault(g => g.Id == game.Id);

            if (savedGame is null)
            {
                return Results.BadRequest("Saved game could not be found.");
            }

            GameDto gameDto = new(
                savedGame.Id,
                savedGame.Name,
                savedGame.Genre?.Name ?? "Unknown",
                savedGame.Price,
                savedGame.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        group.MapPut("/{id:int}", (int id, UpdateGameDto updateGameDto) =>
        {
            var game = games.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            var updatedGame = new GameDto(
                game.Id,
                updateGameDto.Name,
                updateGameDto.Genre,
                updateGameDto.Price,
                DateOnly.FromDateTime(updateGameDto.ReleaseDate)
            );
            var index = games.FindIndex(g => g.Id == id);
            games[index] = updatedGame;

            return Results.Ok(updatedGame);
        });

        group.MapDelete("/{id:int}", (int id) =>
        {
            var removedCount = games.RemoveAll(g => g.Id == id);
            return removedCount > 0 ? Results.NoContent() : Results.NotFound();
        });
        
        return group;
    }
}
