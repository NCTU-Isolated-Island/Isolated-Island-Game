using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
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
    [SerializeField]
    private Text unReadMessageAmount;

    private Dictionary<int, Dictionary<int, PlayerConversation>> playerConversationTable = new Dictionary<int, Dictionary<int, PlayerConversation>>();

    public PlayerInformation chattingPlayer;
    [SerializeField]
    private InputField messageInputField;

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
        Dictionary<int, PlayerConversation> conversations;
        int otherPlayerID = GetOtherPlayerID(conversation);
        if (playerConversationTable.TryGetValue(otherPlayerID, out conversations))
        {
            if(conversations.ContainsKey(conversation.message.playerMessageID))
            {
                conversations[conversation.message.playerMessageID] = conversation;
            }
            else
            {
                conversations.Add(conversation.message.playerMessageID, conversation);
            }
        }
        else
        {
            playerConversationTable.Add(otherPlayerID, new Dictionary<int, PlayerConversation> { { conversation.message.playerMessageID, conversation } });
        }
        if(UIManager.Instance.PageStack.Peek() == UIManager.UIPageType.Chat_Record)
        {
            LoadChatRecord();
        }
        if(UIManager.Instance.PageStack.Peek() == UIManager.UIPageType.Chat_Message && GetOtherPlayerID(conversation) == chattingPlayer.playerID)
        {
            LoadMessagePage(chattingPlayer);
        }

        int sum = playerConversationTable.Sum(targetConversations => targetConversations.Value.Count(targetConversation => 
        {
            if(!targetConversation.Value.hasRead && targetConversation.Value.receiverPlayerID == UserManager.Instance.User.Player.PlayerID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }));
        if (sum == 0)
        {
            unReadMessageAmount.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            unReadMessageAmount.transform.parent.gameObject.SetActive(true);
            unReadMessageAmount.text = sum.ToString();
        }
    }

    public void LoadChatRecord()
    {
        foreach (Transform renderedFriend in messageSetContent.transform)
        {
            Destroy(renderedFriend.gameObject);
        }

        foreach (var entry in playerConversationTable.OrderBy(x => x.Value.OrderBy(y => y.Value.message.sendTime).Last().Value.message.sendTime).Reverse())
        {
            GameObject tmp = Instantiate(messageSet);
            tmp.transform.SetParent(messageSetContent.transform);
            tmp.GetComponent<RectTransform>().localScale = Vector2.one;

            Text chatFriendName = tmp.transform.Find("chatFriendName").GetComponent<Text>();
            Text chatFriendGroup = tmp.transform.Find("chatFriendGroup").GetComponent<Text>();
            Image chatFriendImage = tmp.transform.Find("Image").GetComponent<Image>();

            Text lastMessageRecord = tmp.transform.Find("lastMessageRecord").GetComponent<Text>();
            Text unReadAmount = tmp.transform.Find("UnReadImage/UnReadAmount").GetComponent<Text>();

            PlayerInformation opponentPlayer;
            PlayerInformationManager.Instance.FindPlayerInformation(GetOtherPlayerID(entry.Value.Values.First()), out opponentPlayer);

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

            if (playerConversationTable.ContainsKey(opponentPlayer.playerID))
                lastMessageRecord.text = playerConversationTable[opponentPlayer.playerID].OrderBy(x => x.Value.message.sendTime).Last().Value.message.content;

            int unReadCount = playerConversationTable[opponentPlayer.playerID].Count(x =>
            {
                return !x.Value.hasRead && x.Value.receiverPlayerID == UserManager.Instance.User.Player.PlayerID;
            });
            if (unReadCount == 0)
                unReadAmount.transform.parent.gameObject.SetActive(false);
            else
            {
                unReadAmount.transform.parent.gameObject.SetActive(true);
                unReadAmount.text = unReadCount.ToString();
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
        Dictionary<int, PlayerConversation> conversation = new Dictionary<int, PlayerConversation>();
        if (playerConversationTable.ContainsKey(chatPlayer.playerID))
            conversation = playerConversationTable[chatPlayer.playerID];

        foreach (var entry in conversation.OrderBy(x => x.Value.message.sendTime))
        {
            if (renderedMessage.Contains(entry.Value.message.playerMessageID)) return;
            renderedMessage.Add(entry.Value.message.playerMessageID);

            if (entry.Value.receiverPlayerID == UserManager.Instance.User.Player.PlayerID)
            {
                GameObject bubble = Instantiate(receiveMessageBubble);
                Text textObj = bubble.transform.GetComponentInChildren<Text>();
                textObj.text = entry.Value.message.content;

                bubble.transform.SetParent(messageBubbleContent.transform);
                bubble.GetComponent<RectTransform>().localScale = Vector2.one;
                bubble.GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.preferredHeight + 20);
                if (!entry.Value.hasRead)
                {
                    UserManager.Instance.User.Player.OperationManager.ReadPlayerMessage(entry.Value.message.playerMessageID);
                }
            }
            else if (entry.Value.receiverPlayerID == chatPlayer.playerID)
            {
                GameObject bubble = Instantiate(sendMessageBubble);
                Text textObj = bubble.transform.GetComponentInChildren<Text>();
                textObj.text = entry.Value.message.content;

                bubble.transform.SetParent(messageBubbleContent.transform);
                bubble.GetComponent<RectTransform>().localScale = Vector2.one;
                bubble.GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.preferredHeight + 20);
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
