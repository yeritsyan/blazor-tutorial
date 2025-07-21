using System;
using GameStore.Backend.Data;
using GameStore.Backend.Dtos;
using GameStore.Backend.Entities;
using GameStore.Backend.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Backend.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", (GameStoreContext dbContext) => Results.Ok(dbContext.Games.Include(g => g.Genre).Select(g => g.ToDto()).ToList()));

        group.MapGet("/{id:int}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefault(g => g.Id == id);

            return game is not null ? Results.Ok(game.ToDto()) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = createGameDto.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            var savedGame = dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefault(g => g.Id == game.Id);

            if (savedGame is null)
            {
                return Results.BadRequest("Saved game could not be found.");
            }

            GameDto gameDto = savedGame.ToDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        group.MapPut("/{id:int}", (int id, GameStoreContext dbContext, UpdateGameDto updateGameDto) =>
        {
            var game = dbContext.Games.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            game.UpdateFromDto(updateGameDto);

            dbContext.Games.Update(game);
            dbContext.SaveChanges();

            return Results.Ok(game.ToDto());
        });

        group.MapDelete("/{id:int}", (int id, GameStoreContext dbContext) =>
        {
            var game = dbContext.Games.FirstOrDefault(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            dbContext.SaveChanges();

            return Results.NoContent();
        });
        
        return group;
    }
}
