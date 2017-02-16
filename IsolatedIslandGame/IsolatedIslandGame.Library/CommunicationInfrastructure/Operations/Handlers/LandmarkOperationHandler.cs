using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Operations.Handlers
{
    public abstract class LandmarkOperationHandler
    {
        protected Landmark subject;
        protected int correctParameterCount;

        internal LandmarkOperationHandler(Landmark landmark, int correctParameterCount)
        {
            this.subject = landmark;
            this.correctParameterCount = correctParameterCount;
        }

        internal virtual bool Handle(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameters)
        {
            string debugMessage;
            if (CheckParameter(parameters, out debugMessage))
            {
                return true;
            }
            else
            {
                SendError(communicationInterface, operationCode, ErrorCode.ParameterError, debugMessage);
                return false;
            }
        }
        internal virtual bool CheckParameter(Dictionary<byte, object> parameters, out string debugMessage)
        {
            if (parameters.Count != correctParameterCount)
            {
                debugMessage = string.Format("Parameter Count: {0} Should be {1}", parameters.Count, correctParameterCount);
                return false;
            }
            else
            {
                debugMessage = "";
                return true;
            }
        }
        internal void SendError(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, ErrorCode errorCode, string debugMessage)
        {
            LogService.ErrorFormat("Error On Landmark Operation: {1}, ErrorCode: {2}, Debug Message: {3}", operationCode, errorCode, debugMessage);
            Dictionary<byte, object> parameters = new Dictionary<byte, object>();
            subject.ResponseManager.SendResponse(communicationInterface, operationCode, errorCode, debugMessage, parameters);
        }
        internal void SendResponse(CommunicationInterface communicationInterface, LandmarkOperationCode operationCode, Dictionary<byte, object> parameter)
        {
            subject.ResponseManager.SendResponse(communicationInterface, operationCode, ErrorCode.NoError, null, parameter);
        }
    }
}
