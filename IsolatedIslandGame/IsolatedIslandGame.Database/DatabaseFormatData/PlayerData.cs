using IsolatedIslandGame.Protocol;
using System.Net;

namespace IsolatedIslandGame.Database.DatabaseFormatData
{
    public class PlayerData
    {
        public int playerID;
        public ulong facebookID;
        public string nickname;
        public string signature;
        public GroupType groupType;
        public IPAddress lastConnectedIPAddress;
    }
}
