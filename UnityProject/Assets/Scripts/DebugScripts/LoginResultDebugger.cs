using UnityEngine;

namespace IsolatedIslandGame.Client.Scripts.DebugScripts
{
    public class LoginResultDebugger : MonoBehaviour
    {
        private void OnGUI()
        {
            if(UserManager.Instance.User.IsOnline)
            {
                var player = UserManager.Instance.User.Player;
                GUI.Label(new Rect(50, 50, 400, 40), string.Format("PlayerID: {0}", player.PlayerID));
                GUI.Label(new Rect(50, 150, 400, 40), string.Format("FacebookID: {0}", player.FacebookID));
                GUI.Label(new Rect(50, 250, 400, 40), string.Format("Nickname: {0}", player.Nickname));
                GUI.Label(new Rect(50, 350, 400, 40), string.Format("LastConnectedIPAddress: {0}", player.LastConnectedIPAddress));
            }
            else
            {
                GUI.Label(new Rect(50, 200, 200, 40), "Not Login");
            }
        }
    }
}
