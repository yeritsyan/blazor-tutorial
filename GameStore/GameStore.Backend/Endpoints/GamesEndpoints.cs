using System;
using GameStore.Backend.Dtos;

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

        group.MapPost("/", (CreateGameDto createGameDto) =>
        {
            var newId = games.Max(g => g.Id) + 1;
            var newGame = new GameDto(newId, createGameDto.Name, createGameDto.Genre, createGameDto.Price, createGameDto.ReleaseDate);
            games.Add(newGame);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = newId }, newGame);
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
