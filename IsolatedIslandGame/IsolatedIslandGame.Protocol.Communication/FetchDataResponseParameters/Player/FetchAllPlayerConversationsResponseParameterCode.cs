namespace IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player
{
    public enum FetchAllPlayerConversationsResponseParameterCode : byte
    {
        PlayerMessageID,
        SenderPlayerID,
        SendTime,
        Content,
        ReceiverPlayerID,
        HasRead
    }
}
