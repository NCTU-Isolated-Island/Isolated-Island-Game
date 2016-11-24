using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Client
{
    public class UserManager
    {
        private static UserManager instance;
        public static UserManager Instance { get { return instance; } }

        static UserManager()
        {
            instance = new UserManager();
        }

        public User User { get; private set; }

        public UserManager()
        {
            User = new User(new ClientUserCommunicationInterface());
        }
    }
}
