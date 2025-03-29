namespace PlatformService.Dtos
{
    public record PlatformPublishedDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Event { get; set; }
    }
}
