using System.ComponentModel.DataAnnotations;

namespace GameStore.Backend.Dtos;

public record class CreateGameDto(
    [Required]
    [StringLength(50, MinimumLength = 2)]
    string Name,
    [Required]
    int GenreId,
    [Range(0.01, 100)]
    decimal Price,
    DateOnly ReleaseDate
);
