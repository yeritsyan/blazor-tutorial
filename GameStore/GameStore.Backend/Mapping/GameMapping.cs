using System;
using GameStore.Backend.Dtos;
using GameStore.Backend.Entities;

namespace GameStore.Backend.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto gameDto)
    {
        return new Game
        {
            Name = gameDto.Name,
            GenreId = gameDto.GenreId,
            Price = gameDto.Price,
            ReleaseDate = gameDto.ReleaseDate
        };
    }

    public static void UpdateFromDto(this Game game, UpdateGameDto updateDto)
    {
        game.Name = updateDto.Name;
        game.GenreId = updateDto.GenreId;
        game.Price = updateDto.Price;
        game.ReleaseDate = updateDto.ReleaseDate;
    }

    public static GameSummaryDto ToSummaryDto(this Game game)
    {
        return new GameSummaryDto
        (
            game.Id,
            game.Name,
            game.Genre?.Name ?? "Unknown",
            game.Price,
            game.ReleaseDate
        );
    }

    public static GameDetailsDto ToDetailsDto(this Game game)
    {
        return new GameDetailsDto
        (
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
}
