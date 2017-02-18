namespace IsolatedIslandGame.Protocol.Communication.OperationCodes
{
    public enum LandmarkRoomOperationCode : byte
    {
        FetchData,
        ExitRoom,
        KickParticipant,
        ChangeMultiplayerSynthesizeItem,
        ChangeMultiplayerSynthesizeCheckStatus,
        StartMultiplayerSynthesize
    }
}
