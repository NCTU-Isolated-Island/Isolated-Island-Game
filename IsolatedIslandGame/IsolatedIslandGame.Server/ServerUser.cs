using IsolatedIslandGame.Library;
using System;

namespace IsolatedIslandGame.Server
{
    public class ServerUser : User
    {
        public Guid Guid { get; protected set; }
    }
}
