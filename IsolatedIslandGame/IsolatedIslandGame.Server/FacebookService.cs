namespace IsolatedIslandGame.Server
{
    public class FacebookService
    {
        private static FacebookService instance;

        public static void InitialService()
        {
            instance = new FacebookService();
        }

        public static bool LoginCheck(ulong facebookID, string accessToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
