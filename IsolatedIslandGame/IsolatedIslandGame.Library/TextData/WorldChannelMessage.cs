namespace IsolatedIslandGame.Library.TextData
{
    public class WorldChannelMessage
    {
        public int WorldChannelMessageID { get; private set; }
        public PlayerMessage Message { get; private set; }

        public WorldChannelMessage(int worldChannelMessageID, PlayerMessage message)
        {
            WorldChannelMessageID = worldChannelMessageID;
            Message = message;
        }
    }
}
