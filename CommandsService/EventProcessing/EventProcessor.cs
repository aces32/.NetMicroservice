﻿using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.Repository;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private void AddPlatform(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPlatformRepository>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(platform.ExternalID))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added to database");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists in database");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to Database: {ex.Message}");
                }

            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine($"--> Determining Event Type");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            if (eventType == null || eventType.Event == null)
            {
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
            }
            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        enum EventType
        {
            PlatformPublished,
            Undetermined
        }
    }
}
