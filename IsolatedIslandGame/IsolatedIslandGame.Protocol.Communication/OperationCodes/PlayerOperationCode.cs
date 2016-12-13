﻿namespace IsolatedIslandGame.Protocol.Communication.OperationCodes
{
    public enum PlayerOperationCode : byte
    {
        FetchData,
        CreateCharacter,
        DrawMaterial,
        UpdateVesselTransform,
        AddDecorationToVessel,
        RemoveDecorationFromVessel
    }
}
