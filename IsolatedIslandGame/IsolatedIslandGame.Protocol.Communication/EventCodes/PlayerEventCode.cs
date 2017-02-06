﻿namespace IsolatedIslandGame.Protocol.Communication.EventCodes
{
    public enum PlayerEventCode : byte
    {
        SyncData,
        GetBlueprint,
        FriendInvited,
        GetPlayerConversation,
        TransactionRequest,
        StartTransaction,
        EndTransaction,
    }
}
