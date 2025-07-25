using System;
using GameStore.Backend.Dtos;
using GameStore.Backend.Entities;

namespace GameStore.Backend.Mapping;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.Id, genre.Name);
    }
}
