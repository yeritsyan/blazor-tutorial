using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GameStore.Frontend.Converters;

namespace GameStore.Frontend.Models;

public class GameDetails
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "GenreId is required")]
    [JsonConverter(typeof(StringConverter))]
    public string? GenreId { get; set; }

    [Range(1, 100, ErrorMessage = "Rating must be between 0 and 100")]
    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }
}
