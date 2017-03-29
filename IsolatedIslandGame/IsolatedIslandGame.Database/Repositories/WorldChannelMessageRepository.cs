using IsolatedIslandGame.Library.TextData;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class WorldChannelMessageRepository
    {
        public abstract bool Create(int playerMessageID, out WorldChannelMessage worldMessage);
        public abstract List<WorldChannelMessage> ListLatestN_Message(int n);
    }
}
