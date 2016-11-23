using System.Net;

namespace IsolatedIslandGame.Library
{
    public class Player
    {
        #region properties
        public User User { get; protected set; }
        public int PlayerID { get; protected set; }
        public ulong FacebookID { get; protected set; }
        public string Nickname { get; protected set; }
        public IPAddress LastConnectedIPAddress { get; set; }
        #endregion

        public Player(User user, int playerID, ulong facebookID, string nickname, IPAddress lastConnectedIPAddress)
        {
            User = user;
            PlayerID = playerID;
            FacebookID = facebookID;
            Nickname = nickname;
            LastConnectedIPAddress = lastConnectedIPAddress;
        }
    }
}
