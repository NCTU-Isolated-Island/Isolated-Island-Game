using IsolatedIslandGame.Database.DatabaseElements.Connections;

namespace IsolatedIslandGame.Database.DatabaseElements
{
    public abstract class ConnectionList
    {
        public abstract PlayerDataConnection PlayerDataConnection { get; }
    }
}
