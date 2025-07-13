using System;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GenresClient
{
    private List<Genre> _genres = new()    {
        new Genre { Id = 1, Name = "Action" },
        new Genre { Id = 2, Name = "Adventure" },
        new Genre { Id = 3, Name = "Role-Playing" },
        new Genre { Id = 4, Name = "Simulation" },
        new Genre { Id = 5, Name = "Strategy" }
    };

    public List<Genre> GetGenres()
    {
        // Simulate an API call
        return _genres;
    }
}
