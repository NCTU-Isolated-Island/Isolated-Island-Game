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
                LogService.Info("facebook initial complete");
                if (FB.IsInitialized)
                {
                    LogService.Info("initial successiful waiting for login");
                    FacebookDelegate<ILoginResult> loginCallBack = (result) =>
                    {
                        LogService.Info("facebook login complete");
                        if (FB.IsLoggedIn)
                        {
                            LogService.Info("facebook login successiful");
                            LogService.InfoFormat("FacebookID: {0}, FacebookAccessToken: {1}", result.AccessToken.UserId, result.AccessToken.TokenString);
                            UserManager.Instance.User.OperationManager.Login(ulong.Parse(result.AccessToken.UserId), result.AccessToken.TokenString);
                        }
                    };
                    LogService.Info("waiting for facebook login response");
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
                    LogService.Info("facebook initial failed");
                }
            };
            LogService.Info("waiting for facebook initial response");
            FB.Init(onInitialComplete);
        }
    }
}
