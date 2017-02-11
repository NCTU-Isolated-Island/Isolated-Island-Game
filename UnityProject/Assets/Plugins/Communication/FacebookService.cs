using Facebook.Unity;
using IsolatedIslandGame.Library;
using System.Collections.Generic;
using Facebook.MiniJSON;

namespace IsolatedIslandGame.Client.Communication
{
    public static class FacebookService
    {
        public static void LoginWithFacbook()
        {
            InitDelegate onInitialComplete = () =>
            {
                if (FB.IsInitialized)
                {
                    FacebookDelegate<ILoginResult> loginCallBack = (result) =>
                    {
                        if (FB.IsLoggedIn)
                        {
                            UserManager.Instance.User.OperationManager.Login(ulong.Parse(result.AccessToken.UserId), result.AccessToken.TokenString);
                        }
                    };
                    if (!FB.IsLoggedIn)
                    {
                        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "user_friends" }, loginCallBack);
                    }
                    else
                    {
                        FB.Mobile.RefreshCurrentAccessToken((result) =>
                        {
                            UserManager.Instance.User.OperationManager.Login(ulong.Parse(result.AccessToken.UserId), result.AccessToken.TokenString);
                        });
                    }
                }
                else
                {
                    LogService.Fatal("facebook initial failed");
                }
            };
            if (FB.IsInitialized)
            {
                onInitialComplete();
            }
            else
            {
                FB.Init(onInitialComplete);
            }
        }
    }
}
