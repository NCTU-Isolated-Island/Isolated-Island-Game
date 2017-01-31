using UnityEngine;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Client.Scripts.SystemScripts
{
    public class SystemController : MonoBehaviour
    {
        private void Awake()
        {
            UserManager.Initial(new ClientUserManager());
            ClientSystemManager.Initial(UserManager.Instance.User);
            ItemManager.Initial(new ClientItemManager());
            VesselManager.Initial(new ClientVesselManager());
            BlueprintManager.Initial(new ClientBlueprintManager());
            PlayerInformationManager.Initial(new PlayerInformationManager());
        }
    }
}
