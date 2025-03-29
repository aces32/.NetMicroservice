namespace CommandsService.Dtos
{
    public record PlatformReadDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }  
    }
}
