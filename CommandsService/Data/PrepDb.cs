
using CommandsService.Models;
using CommandsService.Repository;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient?.ReturnAllPlatforms();
                SeedData(serviceScope.ServiceProvider.GetService<IPlatformRepository>(), platforms);
            }
        }

        private static void SeedData(IPlatformRepository? commandRepository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding Platforms...");
            foreach (var plat in platforms)
            {
                if (!commandRepository.ExternalPlatformExists(plat.ExternalID))
                {
                    commandRepository.CreatePlatform(plat);
                }
                commandRepository.SaveChanges();
            }
        }
    }
}
