using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.SystemResponseHandlers
{
    class SystemFetchDataResponseResolver : FetchDataResponseResolver<SystemManager, SystemOperationCode, SystemFetchDataCode>
    {
        public SystemFetchDataResponseResolver(SystemManager subject) : base(subject)
        {

        }
    }
}
