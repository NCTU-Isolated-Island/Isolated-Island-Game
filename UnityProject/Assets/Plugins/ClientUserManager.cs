using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Client
{
    public class ClientUserManager : UserManager
    {
        private User user;

        public ClientUserManager()
        {
            user = new User(new ClientUserCommunicationInterface());
        }

        public override User User
        {
            get
            {
                return user;
            }
        }
    }
}
