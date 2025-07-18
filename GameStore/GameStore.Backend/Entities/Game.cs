using System;

namespace GameStore.Backend.Entities;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int GenreId { get; set; }
    public Genre? Genre { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
}
