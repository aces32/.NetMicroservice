using CommandsService.Models;

namespace CommandsService.Repository
{
    public interface IPlatformRepository
    {

        // Platforms
        IEnumerable<Platform> GetAllPlatforms();
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);
        void CreatePlatform(Platform platform);
        bool SaveChanges();
    }
}
