using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.EventCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure
{
    public abstract class CommunicationInterface
    {
        public User User { get; protected set; }
        public virtual void BindUser(User user)
        {
            User = user;
        }
        public abstract void SendEvent(UserEventCode eventCode, Dictionary<byte, object> parameters);
        public abstract void SendOperation(UserOperationCode operationCode, Dictionary<byte, object> parameters);
        public abstract void SendResponse(UserOperationCode operationCode, ErrorCode errorCode, string debugMessage, Dictionary<byte, object> parameters);
        public abstract void ErrorInform(string title, string message);

        public abstract void GetSystemVersion(out string serverVersion, out string clientVersion);
        public abstract void CheckSystemVersion(string serverVersion, string clientVersion);
        public abstract bool Login(ulong facebookID, string accessToken, out string debugMessage, out ErrorCode errorCode);
        public abstract bool PlayerIDLogin(int playerID, string password, out string debugMessage, out ErrorCode errorCode);
        public abstract bool InviteFriend(int inviterPlayerID, int accepterPlayerID);
        public abstract bool AcceptFriend(int inviterPlayerID, int accepterPlayerID);
        public abstract bool DeleteFriend(int selfPlayerID, int targetPlayerID);
    }
}
