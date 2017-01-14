using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.FetchDataResponseParameters.Player;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.PlayerOperationHandlers.FetchDataHandlers
{
    class FetchFriendInformationsHandler : PlayerFetchDataHandler
    {
        public FetchFriendInformationsHandler(Player subject) : base(subject, 0)
        {
        }

        public override bool Handle(PlayerFetchDataCode fetchCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(fetchCode, parameters))
            {
                try
                {
                    foreach (var information in subject.FriendInformations)
                    {
                        var result = new Dictionary<byte, object>
                        {
                            { (byte)FetchFriendInformationsResponseParameterCode.PlayerID, information.playerInformation.playerID },
                            { (byte)FetchFriendInformationsResponseParameterCode.Nickname, information.playerInformation.nickname },
                            { (byte)FetchFriendInformationsResponseParameterCode.Signature, information.playerInformation.signature },
                            { (byte)FetchFriendInformationsResponseParameterCode.GroupType, (byte)information.playerInformation.groupType },
                            { (byte)FetchFriendInformationsResponseParameterCode.VesselID, information.playerInformation.vesselID },
                            { (byte)FetchFriendInformationsResponseParameterCode.IsInviter, information.isInviter },
                            { (byte)FetchFriendInformationsResponseParameterCode.IsConfirmed, information.isConfirmed }
                        };
                        SendResponse(fetchCode, result);
                    }
                    return true;
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("FetchFriendInformations Invalid Cast!");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
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
