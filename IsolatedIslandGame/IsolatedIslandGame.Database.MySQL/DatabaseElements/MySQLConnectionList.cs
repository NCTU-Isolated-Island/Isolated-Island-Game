using IsolatedIslandGame.Database.DatabaseElements;
using IsolatedIslandGame.Database.DatabaseElements.Connections;
using IsolatedIslandGame.Database.MySQL.DatabaseElements.Connections;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements
{
    class MySQLConnectionList : ConnectionList
    {
        private MySQLPlayerDataConnection playerDataConnection = new MySQLPlayerDataConnection();
        public override PlayerDataConnection PlayerDataConnection { get { return playerDataConnection; } }
    }
}
