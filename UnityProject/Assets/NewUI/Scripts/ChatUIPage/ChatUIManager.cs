﻿using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIManager : MonoBehaviour {

    public static ChatUIManager Instance { get; private set; }

    // UI Variable for Record page
    [SerializeField]
    private GameObject messageSet;
    [SerializeField]
    private GameObject messageSetContent;

    [SerializeField]
    private GameObject receiveMessageBubble;
    [SerializeField]
    private GameObject sendMessageBubble;
    [SerializeField]
    private GameObject messageBubbleContent;

    [SerializeField]
    private Text titleText;

    private Dictionary<int, List<PlayerConversation>> playerConversationTable = new Dictionary<int, List<PlayerConversation>>();

    // Variable for Message page

    private PlayerInformation chattingPlayer;
    [SerializeField]
    private InputField messageInputField;
    //

    void InitSetting()
    {

    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;
    }

    void OnDestroy()
    {
        UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
        UserManager.Instance.User.Player.OnGetPlayerConversation -= OnGetPlayerConversation;
    }

    void OnPlayerOnline(Player player)
    {
        UserManager.Instance.User.Player.OnGetPlayerConversation += OnGetPlayerConversation;
    }

    private int GetOtherPlayerID(PlayerConversation conversation)
    {
        if (conversation.receiverPlayerID == UserManager.Instance.User.Player.PlayerID)
            return conversation.message.senderPlayerID;
        else
            return conversation.receiverPlayerID;
    }

    void OnGetPlayerConversation(PlayerConversation conversation)
    {
        List<PlayerConversation> conversations;
        int otherPlayerID = GetOtherPlayerID(conversation);
        if (playerConversationTable.TryGetValue(otherPlayerID, out conversations))
        {
            conversations.Add(conversation);
        }
        else
        {
            playerConversationTable.Add(otherPlayerID, new List<PlayerConversation> { conversation });
        }
        LoadChatRecord();
    }

    public void LoadChatRecord()
    {
        foreach (Transform renderedFriend in messageSetContent.transform)
        {
            Destroy(renderedFriend.gameObject);
        }

        foreach (var entry in playerConversationTable)
        {
            GameObject tmp = Instantiate(messageSet);
            tmp.transform.SetParent(messageSetContent.transform);

            Text chatFriendName = tmp.transform.FindChild("chatFriendName").GetComponent<Text>();
            Text chatFriendGroup = tmp.transform.FindChild("chatFriendGroup").GetComponent<Text>();

            PlayerInformation opponentPlayer;
            PlayerInformationManager.Instance.FindPlayerInformation(GetOtherPlayerID(entry.Value[0]), out opponentPlayer);

            chatFriendName.text = opponentPlayer.nickname;

            switch (opponentPlayer.groupType)
            {
                case GroupType.Animal:
                    chatFriendGroup.text = "信仰";
                    break;
                case GroupType.Businessman:
                    chatFriendGroup.text = "動物";
                    break;
                case GroupType.Farmer:
                    chatFriendGroup.text = "農夫";
                    break;
            }

            tmp.GetComponent<Button>().onClick.AddListener(delegate{
                ToMessagePageByChatRecord(opponentPlayer);
            });
        }
    }

    public void ToMessagePageByChatRecord(PlayerInformation chatPlayer)
    {
        // Swap to Message Page
        UIManager.Instance.SwapPage(UIManager.UIPageType.Chat_Message);
        // Load Message Page by chatPlayer
        LoadMessagePage(chatPlayer);
    }

    public void LoadMessagePage(PlayerInformation chatPlayer)
    {
        foreach(Transform bubble in messageBubbleContent.transform)
        {
            Destroy(bubble.gameObject);
        }

        // Set chattingPlayer to the chatPlayer
        chattingPlayer = chatPlayer;
        // Set title to the chatting player's name
        titleText.text = chatPlayer.nickname;
        // Instantiate Message Bubble and put under content
        List<int> renderedMessage = new List<int>();
        List<PlayerConversation> conversation = playerConversationTable[chatPlayer.playerID];

        // TODO : Sort conversation by date

        foreach (var entry in conversation)
        {
            // Make sure not rendered same message
            if (renderedMessage.Contains(entry.message.playerMessageID)) return;
            renderedMessage.Add(entry.message.playerMessageID);

            if(entry.receiverPlayerID == UserManager.Instance.User.Player.PlayerID)
            {
                GameObject bubble = Instantiate(receiveMessageBubble);
                Text textObj = bubble.transform.FindChild("Bubble").FindChild("Text").GetComponent<Text>();
                textObj.text = entry.message.content;

                bubble.transform.SetParent(messageBubbleContent.transform);
            }
            else if(entry.receiverPlayerID == chatPlayer.playerID)
            {
                GameObject bubble = Instantiate(sendMessageBubble);
                Text textObj = bubble.transform.FindChild("Bubble").FindChild("Text").GetComponent<Text>();
                textObj.text = entry.message.content;

                bubble.transform.SetParent(messageBubbleContent.transform);
            }
        }
    }

    public void SendMessageToChattingPlayer()
    {
        UserManager.Instance.User.Player.OperationManager.SendMessage(chattingPlayer.playerID , messageInputField.text);

        messageInputField.text = null;
        LoadMessagePage(chattingPlayer);
    }
}