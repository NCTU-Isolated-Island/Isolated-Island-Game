﻿namespace IsolatedIslandGame.Protocol.Communication.OperationCodes
{
    public enum PlayerOperationCode : byte
    {
        FetchData,
        CreateCharacter,
        DrawMaterial,
        UpdateVesselTransform,
        AddDecorationToVessel,
        RemoveDecorationFromVessel,
        UpdateDecorationOnVessel,
        SynthesizeMaterial,
        UseBlueprint,
        InviteFriend,
        AcceptFriend,
        DeleteFriend,
        SendMessage,
        TransactionRequest,
        AcceptTransaction,
        ChangeTransactionItem,
        ChangeTransactionConfirmStatus,
        CancelTransaction,
        SetFavoriteItem,
        ReadPlayerMessage,
        SendMaterialToIsland,
        ScanQR_Code
    }
}
