using IsolatedIslandGame.Database;
using IsolatedIslandGame.Database.Connections;
using IsolatedIslandGame.Database.MySQL.Connections;

namespace IsolatedIslandGame.Database.MySQL
{
    class MySQL_ConnectionList : ConnectionList
    {
        private MySQL_PlayerDataConnection playerDataConnection = new MySQL_PlayerDataConnection();
        public override PlayerDataConnection PlayerDataConnection { get { return playerDataConnection; } }

        private MySQL_SettingDataConnection settingDataConnection = new MySQL_SettingDataConnection();
        public override SettingDataConnection SettingDataConnection { get { return settingDataConnection; } }

        public MySQL_ConnectionList()
        {
            childConnections.Add(playerDataConnection);
            childConnections.Add(settingDataConnection);
        }
    }
}
