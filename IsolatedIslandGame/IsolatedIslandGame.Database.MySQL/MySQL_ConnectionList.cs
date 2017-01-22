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

        private MySQL_TextDataConnection textDataConnection = new MySQL_TextDataConnection();
        public override TextDataConnection TextDataConnection { get { return textDataConnection; } }

        private MySQL_ArchiveDataConnection archiveDataConnection = new MySQL_ArchiveDataConnection();
        public override ArchiveDataConnection ArchiveDataConnection { get { return archiveDataConnection; } }

        public MySQL_ConnectionList()
        {
            childConnections.Add(playerDataConnection);
            childConnections.Add(settingDataConnection);
            childConnections.Add(textDataConnection);
            childConnections.Add(archiveDataConnection);
        }
    }
}
