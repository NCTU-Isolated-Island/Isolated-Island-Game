using System;

namespace IsolatedIslandGame.Library.TextData
{
    public struct PlayerMessage
    {
        public int playerMessageID;
        public int senderPlayerID;
        public DateTime sendTime;
        public string content;
    }
}
