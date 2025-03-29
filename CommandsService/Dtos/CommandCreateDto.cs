using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos
{
    public record CommandCreateDto
    {
        [Required]
        public required string HowTo { get; set; }
        [Required]
        public required string CommandLine { get; set; }
    }
}
