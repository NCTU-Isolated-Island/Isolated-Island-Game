using IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.UserResponseHandlers.FetchDataResponseHandlers;
using IsolatedIslandGame.Protocol.Communication.FetchDataCodes;
using IsolatedIslandGame.Protocol.Communication.OperationCodes;

namespace IsolatedIslandGame.Library.CommunicationInfrastructure.Responses.Handlers.UserResponseHandlers
{
    class UserFetchDataResponseResolver : FetchDataResponseResolver<User, UserOperationCode, UserFetchDataCode>
    {
        public UserFetchDataResponseResolver(User subject) : base(subject)
        {
            fetchResponseTable.Add(UserFetchDataCode.SystemVersion, new FetchSystemVersionResponseHandler(subject));
        }
    }
}
