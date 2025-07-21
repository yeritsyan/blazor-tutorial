using System;
using GameStore.Backend.Data;
using GameStore.Backend.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Backend.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var genres = await dbContext.Genres.Select(g => g.ToDto()).AsNoTracking().ToListAsync();
            return Results.Ok(genres);
        });

        return group;
    }
}
