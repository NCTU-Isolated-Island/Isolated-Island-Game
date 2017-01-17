using IsolatedIslandGame.Library.TextData;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class PlayerMessageRepository
    {
        public abstract bool Create(int senderPlayerID, string content, out PlayerMessage message);
        public abstract bool Read(int playerMessageID, out PlayerMessage message);
        public abstract List<PlayerMessage> ListOfSender(int senderPlayerID);
    }
}
