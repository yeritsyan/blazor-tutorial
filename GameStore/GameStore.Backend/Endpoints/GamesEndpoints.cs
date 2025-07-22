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

        group.MapGet("/", async (GameStoreContext dbContext) =>
        Results.Ok(await dbContext.Games
            .Include(g => g.Genre)
            .Select(g => g.ToSummaryDto())
            .AsNoTracking()
            .ToListAsync()));

        group.MapGet("/{id:int}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games
                .FirstOrDefaultAsync(g => g.Id == id);

            return game is not null ? Results.Ok(game.ToDetailsDto()) : Results.NotFound();
        })
        .WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = createGameDto.ToEntity();

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            var savedGame = await dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(g => g.Id == game.Id);

            if (savedGame is null)
            {
                return Results.BadRequest("Saved game could not be found.");
            }

            GameSummaryDto gameDto = savedGame.ToSummaryDto();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        group.MapPut("/{id:int}", async (int id, GameStoreContext dbContext, UpdateGameDto updateGameDto) =>
        {
            var game = await dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            game.UpdateFromDto(updateGameDto);

            await dbContext.SaveChangesAsync();

            // Load with Genre for response if needed
            var updatedGame = await dbContext.Games
                .Include(g => g.Genre)
                .FirstOrDefaultAsync(g => g.Id == id);

            return Results.Ok(updatedGame?.ToSummaryDto());
        });

        group.MapDelete("/{id:int}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });
        
        return group;
    }
}
