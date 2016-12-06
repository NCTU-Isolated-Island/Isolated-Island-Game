using IsolatedIslandGame.Database.DatabaseElements;
using IsolatedIslandGame.Database.DatabaseElements.Connections;
using IsolatedIslandGame.Database.MySQL.DatabaseElements.Connections;

namespace IsolatedIslandGame.Database.MySQL.DatabaseElements
{
    class MySQL_ConnectionList : ConnectionList
    {
        private MySQL_PlayerDataConnection playerDataConnection = new MySQL_PlayerDataConnection();
        public override PlayerDataConnection PlayerDataConnection { get { return playerDataConnection; } }

        private MySQL_ItemDataConnection itemDataConnection = new MySQL_ItemDataConnection();
        public override ItemDataConnection ItemDataConnection { get { return itemDataConnection; } }

        public MySQL_ConnectionList()
        {
            childConnections.Add(playerDataConnection);
            childConnections.Add(itemDataConnection);
        }
    }
}
