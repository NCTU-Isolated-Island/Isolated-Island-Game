using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WorldChannelChatManager : MonoBehaviour
{
    public static WorldChannelChatManager Instance { get; private set; }

    [SerializeField]
    private GameObject worldChannelMessagePagePrefab;

    private GameObject worldChannelMessagePage;

    [SerializeField]
    private GameObject receiveMessageBubble;
    [SerializeField]
    private GameObject sendMessageBubble;

    [SerializeField]
    private GameObject accessOtherPlayerPage;
    [SerializeField]
    private GameObject messageBubbleContent;
    [SerializeField]
    private InputField messageInputField;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SystemManager.Instance.OnLoadWorldChannelMessage += UpdateWorldChannelMessageBubble;
    }

    private void UpdateWorldChannelMessageBubble(WorldChannelMessage newMessage)
    {
        //print("UpdateWorldChannelMessageBubble");
        if (messageBubbleContent != null)
            RenderWorldChannelBubble();
    }

    private void RenderWorldChannelBubble()
    {
        print("Rendering");
        foreach (Transform oldMessage in messageBubbleContent.transform)
        {
            Destroy(oldMessage.gameObject);
        }

        foreach (WorldChannelMessage newMessage in
            SystemManager.Instance.GetWorldChannelMessages().OrderBy(x => x.Message.sendTime))
        {
            // i am sender
            if (newMessage.Message.senderPlayerID == GameManager.Instance.PlayerID)
            {
                GameObject outframe = Instantiate(sendMessageBubble);
                GameObject bubble = outframe.transform.Find("SendBubble").gameObject;
                FillInBubbleData(newMessage, bubble);
            }
            else
            {
                GameObject outframe = Instantiate(receiveMessageBubble);
                GameObject bubble = outframe.transform.Find("ReceiveBubble").gameObject;
                FillInBubbleData(newMessage, bubble);
            }
        }
    }

    private void FillInBubbleData(WorldChannelMessage worldChannelMessage, GameObject bubble)
    {
        Text textObj = bubble.transform.GetComponentInChildren<Text>();

        if (worldChannelMessage.Message.senderPlayerID == GameManager.Instance.PlayerID)
        {
            textObj.text = worldChannelMessage.Message.content;
        }
        else
        {
            PlayerInformation info;
            if (PlayerInformationManager.Instance.FindPlayerInformation(worldChannelMessage.Message.senderPlayerID, out info))
            {
                textObj.text = string.Format("{0} 說 : {1}", info.nickname, worldChannelMessage.Message.content);
            }
            else
            {
                textObj.text = string.Format("{0} 說 : {1}", "...", worldChannelMessage.Message.content);
            }

            bubble.AddComponent<Button>();
            Button bubbleButton = bubble.GetComponent<Button>();
            bubbleButton.onClick.AddListener(delegate
            {
                accessOtherPlayerPage.SetActive(true);
                SetAccessOtherPlayerPage(info);
            });
        }

        bubble.transform.parent.SetParent(messageBubbleContent.transform);
        bubble.GetComponent<RectTransform>().localScale = Vector2.one;
        bubble.GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.preferredHeight + 20);

        bubble.transform.parent.GetComponent<RectTransform>().localScale = Vector2.one;
        bubble.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.preferredHeight + 20);
    }

    private void SetAccessOtherPlayerPage(PlayerInformation info)
    {
        Button sendMessageButton = accessOtherPlayerPage.transform.Find("SendMessage").GetComponent<Button>();
        Button sendFriendRequestButton = accessOtherPlayerPage.transform.Find("SendFriendRequest").GetComponent<Button>();
        Button sendTransactionRequestButton = accessOtherPlayerPage.transform.Find("SendTransactionRequest").GetComponent<Button>();
        Button checkVesselButton = accessOtherPlayerPage.transform.Find("CheckVessel").GetComponent<Button>();

        sendMessageButton.onClick.RemoveAllListeners();
        sendFriendRequestButton.onClick.RemoveAllListeners();
        sendTransactionRequestButton.onClick.RemoveAllListeners();
        checkVesselButton.onClick.RemoveAllListeners();

        sendMessageButton.onClick.AddListener(delegate
        {
            ChatUIManager.Instance.ToMessagePageByPlayerInformation(info);
            Destroy(worldChannelMessagePage);
        });
        sendFriendRequestButton.onClick.AddListener(delegate
        {
            UserManager.Instance.User.Player.OperationManager.InviteFriend(info.playerID);
            accessOtherPlayerPage.SetActive(false);
        });
        sendTransactionRequestButton.onClick.AddListener(delegate
        {
            TransactionManager.Instance.SendTransactionRequest(info.playerID);
            accessOtherPlayerPage.SetActive(false);
        });
        checkVesselButton.onClick.AddListener(delegate
        {
            if (!GameManager.Instance.UserGameObject.ContainsKey(info.playerID))
            {
                UserManager.Instance.User.UserInform("通知" , "此玩家目前不在線上");
                return;
            }

            CameraManager.Instance.ToNearAnchor(GameManager.Instance.UserGameObject[info.playerID]);
            UIManager.Instance.SwapPage(UIManager.UIPageType.OtherBoat);
            OtherBoatUIManager.Instance.SetOtherPlayerInfo(info.playerID);
            Destroy(worldChannelMessagePage);
        });

        if (UserManager.Instance.User.Player.ContainsFriend(info.playerID))
        {
            sendFriendRequestButton.interactable = false;
        }
    }

    public void ToWorldChannelChatPage()
    {
        GameObject page = Instantiate(worldChannelMessagePagePrefab);
        page.transform.SetParent(transform);
        page.GetComponent<RectTransform>().localScale = Vector3.one;
        page.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        page.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        worldChannelMessagePage = page;

        messageInputField = page.GetComponentInChildren<InputField>();
        messageBubbleContent = page.transform.Find("List/Viewport/Content").gameObject;
        accessOtherPlayerPage = page.transform.Find("AccessOtherPlayerPanel").gameObject;
        Button sendMessageButton = page.transform.Find("OperationField/EnterButton").gameObject.GetComponent<Button>();
        Button backButton = page.transform.Find("OperationField/BackButton").gameObject.GetComponent<Button>();

        sendMessageButton.onClick.AddListener(delegate 
        {
            SendMessageToWorldChannel();
        });

        backButton.onClick.AddListener(delegate
        {
            Destroy(page);
        });
        accessOtherPlayerPage.SetActive(false);

        RenderWorldChannelBubble();
    }

    public void SendMessageToWorldChannel()
    {
        if (IsOnlyWhiteSpaceOrEmpty(messageInputField.text))
            return;
       UserManager.Instance.User.Player.OperationManager.SendWorldChannelMessage(messageInputField.text);
        messageInputField.text = null;
    }

    private bool IsOnlyWhiteSpaceOrEmpty(string s)
    {
        foreach (char c in s)
        {
            if (c != ' ') return false;
        }
        return true;
    }

}