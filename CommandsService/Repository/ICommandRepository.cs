using CommandsService.Models;

namespace CommandsService.Repository
{
    public interface ICommandRepository
    {
        // Commands
        bool SaveChanges();
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);

    }
}
