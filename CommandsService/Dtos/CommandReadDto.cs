﻿namespace CommandsService.Dtos
{
    public record CommandReadDto
    {
        public int Id { get; set; }
        public required string HowTo { get; set; }
        public required string CommandLine { get; set; }
        public required int PlatformId { get; set; }

    }
}
