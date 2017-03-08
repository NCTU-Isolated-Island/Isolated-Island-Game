using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitingFriendSetBehavior : MonoBehaviour {

    public PlayerInformation information;

    private Button friendSetButton;

    void Awake()
    {

    }

    void Start()
    {
        
    }

    public void InitialButtonStatus()
    {
        friendSetButton = gameObject.GetComponent<Button>();
        friendSetButton.onClick.AddListener(
            delegate
            {
                UIManager.Instance.SwapPage(UIManager.UIPageType.OtherBoat);
                CameraManager.Instance.ToNearAnchor(GameManager.Instance.UserGameObject[information.playerID]);
                FriendUIManager.Instance.gameObject.SetActive(false);
            });
        //print("UpdateButtonStatus");
        if (PlayerInformationManager.Instance.FindPlayerInformation(information.playerID, out information))
        {
            if (GameManager.Instance.UserGameObject.ContainsKey(information.playerID))
                friendSetButton.interactable = true;
            else
                friendSetButton.interactable = false;
        }
    }

    public void AcceptFriendInvite()
    {
        UserManager.Instance.User.Player.OperationManager.AcceptFriend(information.playerID);
    }

    public void RefuseFriendInvite()
    {
        UserManager.Instance.User.Player.OperationManager.DeleteFriend(information.playerID);
        Destroy(gameObject);
    }

}
