using IsolatedIslandGame.Library;
using System;
using System.Net;

namespace IsolatedIslandGame.Server
{
    public class ServerUser : User
    {
        public Guid Guid { get; protected set; }

        public ServerUser(IPAddress lastConnectedIPAddress)
        {
            LastConnectedIPAddress = lastConnectedIPAddress;
            Guid = Guid.NewGuid();
            while(UserFactory.Instance.ContainsUserGuid(Guid))
            {
                Guid = Guid.NewGuid();
            }
        }
    }
}
