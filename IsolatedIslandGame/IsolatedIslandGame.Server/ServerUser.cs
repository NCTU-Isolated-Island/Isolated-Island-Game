using IsolatedIslandGame.Library;
using System;

namespace IsolatedIslandGame.Server
{
    public class ServerUser : User
    {
        public Guid Guid { get; protected set; }

        public ServerUser()
        {
            Guid = Guid.NewGuid();
            while(UserFactory.Instance.ContainsUserGuid(Guid))
            {
                Guid = Guid.NewGuid();
            }
        }
    }
}
