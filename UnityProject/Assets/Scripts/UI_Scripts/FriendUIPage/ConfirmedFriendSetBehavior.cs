using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class ConfirmedFriendSetBehavior : MonoBehaviour {

    public PlayerInformation information;
    private Button sendMessageButton;
    private Button friendSetButton;

    void Awake()
    {

    }

    void OnEnable()
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
                OtherBoatUIManager.Instance.SetOtherPlayerInfo(information.playerID);
                UIManager.Instance.SwapPage(UIManager.UIPageType.OtherBoat);
                CameraManager.Instance.ToNearAnchor(GameManager.Instance.UserGameObject[information.playerID]);
                FriendUIManager.Instance.gameObject.SetActive(false);
            });
        sendMessageButton = gameObject.transform.Find("SendMessageButton").GetComponent<Button>();
        sendMessageButton.onClick.AddListener(delegate
        {
            UIManager.Instance.SwapPage(UIManager.UIPageType.Chat_Message);
            ChatUIManager.Instance.LoadMessagePage(information);
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

    public void CheckFriendVessel()
    {
        OtherBoatUIManager.Instance.SetOtherPlayerInfo(information.playerID);
        UIManager.Instance.SwapPage(UIManager.UIPageType.OtherBoat);
        // TODO
    }

}
