using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class ChatUIManager : MonoBehaviour {

    public static ChatUIManager Instance { get; private set; }

    // UI Variable
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

    }

    public void LoadChatRecord()
    {
        List<int> talkedFriends = new List<int>();
        foreach (var entry in MessageManager.Instance.Conversations)
        {
            if (!talkedFriends.Contains(entry.receiverPlayerID))
                talkedFriends.Add(entry.receiverPlayerID);
            if (!talkedFriends.Contains(entry.message.senderPlayerID))
                talkedFriends.Add(entry.message.senderPlayerID);
        }
    }

    public void ToMessagePageByChatRecord(Player chatPlayer)
    {
        // Swap to Message Page
        UIManager.Instance.SwapPage(UIManager.UIPageType.Chat_Message);
        // Load Message Page by chatPlayer
        LoadMessagePage(chatPlayer);
    }

    public void LoadMessagePage(Player chatPlayer)
    {
        // Set title to the chatting player's name
        titleText.text = chatPlayer.Nickname;
        // Instantiate Message Bubble and put under content
        List<int> renderedMessage = new List<int>();
        foreach (var entry in MessageManager.Instance.Conversations)
        {
            // Make sure not rendered same message
            if (renderedMessage.Contains(entry.message.playerMessageID)) return;
            renderedMessage.Add(entry.message.playerMessageID);

            if(entry.receiverPlayerID == UserManager.Instance.User.Player.PlayerID)
            {
                GameObject bubble = Instantiate(receiveMessageBubble);
                Text textObj = bubble.transform.FindChild("Bubble").FindChild("Text").GetComponent<Text>();
                textObj.text = entry.message.content;
            }
            else if(entry.message.senderPlayerID == chatPlayer.PlayerID)
            {
                GameObject bubble = Instantiate(sendMessageBubble);
                Text textObj = bubble.transform.FindChild("Bubble").FindChild("Text").GetComponent<Text>();
                textObj.text = entry.message.content;
            }
        }
    }
}
