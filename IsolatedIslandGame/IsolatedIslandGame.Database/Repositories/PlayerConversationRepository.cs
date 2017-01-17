using IsolatedIslandGame.Library.TextData;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class PlayerConversationRepository
    {
        public abstract bool Create(int receiverPlayerID, int playerMessageID, bool hasRead, out PlayerConversation conversation);
        public abstract List<PlayerConversation> ListOfReceiver(int receiverPlayerID);
    }
}
