using IsolatedIslandGame.Library.TextData;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class PlayerConversationRepository
    {
        public abstract bool Create(int receiverPlayerID, int playerMessageID, bool hasRead, out PlayerConversation conversation);
        public abstract bool Read(int receiverPlayerID, int playerMessageID, out PlayerConversation conversation);
        public abstract List<PlayerConversation> ListOfPlayer(int playerID);
        public abstract bool SetPlayerMessageRead(int playerID, int playerMessageID);
    }
}
