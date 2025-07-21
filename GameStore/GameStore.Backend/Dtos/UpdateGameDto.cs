using System.ComponentModel.DataAnnotations;

namespace GameStore.Backend.Dtos;

public record class UpdateGameDto(
    [Required]
    [StringLength(50, MinimumLength = 2)]
    string Name,
    [Range(0.01, 100)]
    decimal Price,
    [Required]
    int GenreId,
    DateOnly ReleaseDate
);
