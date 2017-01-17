namespace IsolatedIslandGame.Protocol.Communication.EventParameters.Player
{
    public enum GetPlayerConversationParameterCode : byte
    {
        PlayerMessageID,
        SenderPlayerID,
        SendTime,
        Content,
        HasRead
    }
}
