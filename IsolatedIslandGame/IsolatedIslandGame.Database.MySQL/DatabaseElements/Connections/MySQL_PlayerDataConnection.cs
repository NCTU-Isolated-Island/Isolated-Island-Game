using IsolatedIslandGame.Database.DatabaseElements.Connections;
using MySql.Data.MySqlClient;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements.Connections
{
    class MySQL_PlayerDataConnection : PlayerDataConnection
    {
        protected override string DatabaseName { get { return "PlayerData"; } }

        public override bool Connect(string hostName, string userName, string password, string database)
        {
            string connectString = string.Format("server={0};uid={1};pwd={2};database={3}_{4}", hostName, userName, password, database, DatabaseName);
            connection = new MySqlConnection(connectString);

            childConnections.ForEach(x => x.Connect(hostName, userName, password, string.Format("{0}_{1}", database, DatabaseName)));

            return Connected;
        }
    }
}
