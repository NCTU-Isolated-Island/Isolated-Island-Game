namespace IsolatedIslandGame.Protocol
{
    public enum ErrorCode : short
    {
        NoError,
        ParameterError,
        InvalidOperation,
        PermissionDeny,
        Fail,
        NotExist,
        AlreadyExisted,
        NotEnough
    }
}
