using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public PlayerInformation chattingPlayer;
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
        LoadMessagePage(chattingPlayer);
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
            tmp.GetComponent<RectTransform>().localScale = Vector2.one;

            Text chatFriendName = tmp.transform.FindChild("chatFriendName").GetComponent<Text>();
            Text chatFriendGroup = tmp.transform.FindChild("chatFriendGroup").GetComponent<Text>();
            Image chatFriendImage = tmp.transform.FindChild("Image").GetComponent<Image>();

            PlayerInformation opponentPlayer;
            PlayerInformationManager.Instance.FindPlayerInformation(GetOtherPlayerID(entry.Value[0]), out opponentPlayer);

            chatFriendName.text = opponentPlayer.nickname;

            switch (opponentPlayer.groupType)
            {
                case GroupType.Animal:
                    chatFriendGroup.text = "信仰";
                    chatFriendImage.sprite = Resources.Load<Sprite>("GroupIcon/animal");
                    break;
                case GroupType.Businessman:
                    chatFriendGroup.text = "科技";
                    chatFriendImage.sprite = Resources.Load<Sprite>("GroupIcon/businessman");
                    break;
                case GroupType.Farmer:
                    chatFriendGroup.text = "自然";
                    chatFriendImage.sprite = Resources.Load<Sprite>("GroupIcon/farmer");
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
        foreach (Transform bubble in messageBubbleContent.transform)
        {
            Destroy(bubble.gameObject);
        }

        // Set chattingPlayer to the chatPlayer
        chattingPlayer = chatPlayer;
        // Set title to the chatting player's name
        titleText.text = chatPlayer.nickname;
        // Instantiate Message Bubble and put under content
        List<int> renderedMessage = new List<int>();
        List<PlayerConversation> conversation = new List<PlayerConversation>();
        if (playerConversationTable.ContainsKey(chatPlayer.playerID))
            conversation = playerConversationTable[chatPlayer.playerID];

        foreach (var entry in conversation.OrderBy(x => x.message.sendTime))
        {
            // Make sure not rendered same message
            if (renderedMessage.Contains(entry.message.playerMessageID)) return;
            renderedMessage.Add(entry.message.playerMessageID);

            if (entry.receiverPlayerID == UserManager.Instance.User.Player.PlayerID)
            {
                GameObject bubble = Instantiate(receiveMessageBubble);
                Text textObj = bubble.transform.GetComponentInChildren<Text>();
                textObj.text = entry.message.content;

                if (entry.hasRead == false)
                    UserManager.Instance.User.Player.OperationManager.ReadPlayerMessage(entry.message.playerMessageID);

                bubble.transform.SetParent(messageBubbleContent.transform);
                bubble.GetComponent<RectTransform>().localScale = Vector2.one;
                // Adjust bubble height
                //bubble.transform.Find("Bubble").GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.gameObject.GetComponent<RectTransform>().rect.height + 10);
            }
            else if (entry.receiverPlayerID == chatPlayer.playerID)
            {
                GameObject bubble = Instantiate(sendMessageBubble);
                Text textObj = bubble.transform.GetComponentInChildren<Text>();
                textObj.text = entry.message.content;

                bubble.transform.SetParent(messageBubbleContent.transform);
                bubble.GetComponent<RectTransform>().localScale = Vector2.one;
                // Adjust bubble height
                //bubble.transform.Find("Bubble").GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.gameObject.GetComponent<RectTransform>().rect.height + 10);
            }
        }
    }

    public void SendMessageToChattingPlayer()
    {
        if (IsOnlyWhiteSpaceOrEmpty(messageInputField.text))
            return;

        UserManager.Instance.User.Player.OperationManager.SendMessage(chattingPlayer.playerID, messageInputField.text);

        messageInputField.text = null;
        LoadMessagePage(chattingPlayer);
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
