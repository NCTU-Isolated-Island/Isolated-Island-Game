using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using IsolatedIslandGame.Library;

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
            WebClient webClient = new WebClient();
            Stream data = null;
            StreamReader reader = null;
            string responseText;

            try
            {
                data = webClient.OpenRead(string.Format("https://graph.facebook.com/{0}/permissions?access_token={1}", facebookID, accessToken));
                reader = new StreamReader(data);
                responseText = reader.ReadToEnd();
                LogService.Info(responseText);
                data?.Close();
                reader?.Close();
                return true;
            }
            catch (WebException exception)
            {
                using (var readerEX = new StreamReader(exception.Response.GetResponseStream()))
                {
                    responseText = readerEX.ReadToEnd();
                    LogService.Info(responseText);
                }
                data?.Close();
                reader?.Close();
                return false;
            }
        }
    }
}
