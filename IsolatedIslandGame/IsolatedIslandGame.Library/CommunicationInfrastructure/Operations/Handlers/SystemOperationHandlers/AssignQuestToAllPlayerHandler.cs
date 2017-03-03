using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using IsolatedIslandGame.Protocol.Communication.OperationParameters.System;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers.SystemOperationHandlers
{
    class AssignQuestToAllPlayerHandler : SystemOperationHandler
    {
        internal AssignQuestToAllPlayerHandler(SystemManager subject) : base(subject, 2)
        {
        }

        internal override bool Handle(CommunicationInterface communicationInterface, SystemOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            if (base.Handle(communicationInterface, operationCode, parameters))
            {
                try
                {
                    int questID = (int)parameters[(byte)AssignQuestToAllPlayerParameterCode.QuestID];
                    string administratorPassword = (string)parameters[(byte)AssignQuestToAllPlayerParameterCode.AdministratorPassword];

                    return communicationInterface.AssignQuestToAllPlayer(questID, administratorPassword);
                }
                catch (InvalidCastException ex)
                {
                    LogService.ErrorFormat("AssignQuestToAllPlayer Parameter Cast Error");
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
