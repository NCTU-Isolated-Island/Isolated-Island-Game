using IsolatedIslandGame.Database.DatabaseElements.Connections;

namespace IsolatedIslandGame.Database.DatabaseElements
{
    public abstract class ConnectionList : DatabaseConnection
    {
        public abstract PlayerDataConnection PlayerDataConnection { get; }
        protected override string DatabaseName { get { return ""; } }
        public override bool Connect(string hostName, string userName, string password, string database)
        {
            childConnections.ForEach(x => x.Connect(hostName, userName, password, database));

            return Connected;
        }
    }
}
