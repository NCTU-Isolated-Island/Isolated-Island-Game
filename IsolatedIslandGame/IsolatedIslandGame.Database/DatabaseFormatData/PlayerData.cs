using System.Net;

namespace IsolatedIslandGame.Database.DatabaseFormatData
{
    public class PlayerData
    {
        public int playerID;
        public ulong facebookID;
        public string nickname;
        public IPAddress lastConnectedIPAddress;
    }
}
