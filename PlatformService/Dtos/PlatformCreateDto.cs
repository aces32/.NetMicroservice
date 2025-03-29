namespace PlatformService.Dtos
{
    public record PlatformCreateDto
    {
        public required string Name { get; set; }
        public required string Publisher { get; set; }
        public required string Cost { get; set; }
    }
}
