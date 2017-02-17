using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.LandmarkRoom;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.LandmarkRoomOperationHandlers
{
    class ChangeMultiplayerSynthesizeItemHandler : LandmarkRoomOperationHandler
    {
        public ChangeMultiplayerSynthesizeItemHandler(LandmarkRoom landmarkRoom) : base(landmarkRoom, 2)
        {
        }
        internal override bool Handle(CommunicationInterface communicationInterface, LandmarkRoomOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    int itemID = (int)parameters[(byte)ChangeMultiplayerSynthesizeItemParameterCode.ItemID];
                    int itemCount = (int)parameters[(byte)ChangeMultiplayerSynthesizeItemParameterCode.ItemCount];

                    MultiplayerSynthesizeParticipantInfo info;
                    Player player = communicationInterface.User.Player;
                    if (subject.FindMutiplayerSynthesizeParticipantInfo(player.PlayerID, out info))
                    {
                        if(player.Inventory.ItemCount(itemID) >= itemCount)
                        {
                            info.providedItemID = itemID;
                            info.providedItemCount = itemCount;
                            subject.LoadMutiplayerSynthesizeParticipantInfo(info);
                            return true;
                        }
                        else
                        {
                            LogService.Error($"ChangeMultiplayerSynthesizeItem Error Player ID: {player.PlayerID} Item Not Enough ItemID: {itemID}, Count: {itemCount}");
                            return false;
                        }
                    }
                    else
                    {
                        LogService.ErrorFormat("ChangeMultiplayerSynthesizeItem Error Player ID: {0} Not in LandmarkRoom Identity: {1}", communicationInterface.User.Player.PlayerID, subject.IdentityInformation);
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("ChangeMultiplayerSynthesizeItem Parameter Cast Error");
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.ErrorFormat(ex.Message);
                    LogService.ErrorFormat(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
