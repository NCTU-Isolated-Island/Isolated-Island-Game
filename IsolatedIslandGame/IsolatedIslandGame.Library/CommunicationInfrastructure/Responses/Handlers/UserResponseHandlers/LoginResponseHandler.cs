using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.ResponseParameters.User;
using System;
using System.Collections.Generic;
using System.Net;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.UserResponseHandlers
{
    class LoginResponseHandler : ResponseHandler<User, UserOperationCode>
    {
        public LoginResponseHandler(User subject) : base(subject)
        {
        }

        internal override bool CheckError(Dictionary<byte, object> parameters, ErrorCode returnCode, string debugMessage)
        {
            switch (returnCode)
            {
                case ErrorCode.NoError:
                    {
                        if (parameters.Count != 6)
                        {
                            LogService.ErrorFormat(string.Format("LoginResponse Parameter Error, Parameter Count: {0}", parameters.Count));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                case ErrorCode.Fail:
                    {
                        LogService.ErrorFormat("Login Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "登入失敗");
                        return false;
                    }
                case ErrorCode.AlreadyExisted:
                    {
                        LogService.ErrorFormat("Login Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "此帳號已經登入");
                        return false;
                    }
                default:
                    {
                        LogService.ErrorFormat("Login Error DebugMessage: {0}", debugMessage);
                        subject.EventManager.ErrorInform("錯誤", "未知的錯誤種類");
                        return false;
                    }
            }
        }

        internal override bool Handle(UserOperationCode operationCode, ErrorCode returnCode, string debugMessage, Dictionary<byte, object> parameters)
        {
            if (base.Handle(operationCode, returnCode, debugMessage, parameters))
            {
                try
                {
                    int playerID = (int)parameters[(byte)LoginResponseParameterCode.PlayerID];
                    ulong facebookID = ulong.Parse((string)parameters[(byte)LoginResponseParameterCode.FacebookID]);
                    string nickname = (string)parameters[(byte)LoginResponseParameterCode.Nickname];
                    string signature = (string)parameters[(byte)LoginResponseParameterCode.Signature];
                    GroupType groupType = (GroupType)parameters[(byte)LoginResponseParameterCode.GroupType];
                    string lastConnectedIPAddress = (string)parameters[(byte)LoginResponseParameterCode.LastConnectedIPAddress];
                    Player player = new Player(playerID, facebookID, nickname, signature, groupType, IPAddress.Parse(lastConnectedIPAddress));
                    player.BindUser(subject);
                    subject.PlayerOnline(player);
                    subject.Player.OperationManager.FetchDataResolver.FetchInventory();
                    subject.Player.OperationManager.FetchDataResolver.FetchAllKnownBlueprints();
                    subject.Player.OperationManager.FetchDataResolver.FetchFriendInformations();
                    subject.Player.OperationManager.FetchDataResolver.FetchAllPlayerConversations();
                    subject.Player.OperationManager.FetchDataResolver.FetchAllQuestRecords();

                    SystemManager.Instance.OperationManager.FetchDataResolver.FetchIslandTotalScore();
                    SystemManager.Instance.OperationManager.FetchDataResolver.FetchIslandTodayMaterialRanking();
                    SystemManager.Instance.OperationManager.FetchDataResolver.FetchIslandPlayerScoreRanking();

                    Vessel vessel;
                    if(VesselManager.Instance.FindVesselByOwnerPlayerID(playerID, out vessel))
                    {
                        subject.Player.BindVessel(vessel);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (InvalidCastException ex)
                {
                    subject.PlayerOffline();
                    LogService.Error("PlayerLoginResponse Parameter Cast Error");
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
                catch (Exception ex)
                {
                    subject.PlayerOffline();
                    LogService.Error(ex.Message);
                    LogService.Error(ex.StackTrace);
                    return false;
                }
            }
            else
            {
                subject.PlayerOffline();
                return false;
            }
        }
    }
}
