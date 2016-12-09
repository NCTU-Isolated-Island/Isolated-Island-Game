namespace IsolatedIslandGame.Library
{
    public abstract class UserManager
    {
        public static UserManager Instance { get; private set; }

        public static void Initial(UserManager userManager)
        {
            Instance = userManager;
        }

        public abstract User User { get; }
    }
}
