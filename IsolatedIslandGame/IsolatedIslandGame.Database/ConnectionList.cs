using IsolatedIslandGame.Database.Connections;

namespace IsolatedIslandGame.Database
{
    public abstract class ConnectionList : DatabaseConnection
    {
        public abstract PlayerDataConnection PlayerDataConnection { get; }
        public abstract SettingDataConnection SettingDataConnection { get; }
        public abstract TextDataConnection TextDataConnection { get; }

        protected override string DatabaseName { get { return ""; } }
        public override bool Connect(string hostName, string userName, string password, string database)
        {
            childConnections.ForEach(x => x.Connect(hostName, userName, password, database));

            return Connected;
        }
    }
}
