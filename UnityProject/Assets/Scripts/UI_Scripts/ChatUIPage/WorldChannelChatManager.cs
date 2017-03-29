using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorldChannelChatManager : MonoBehaviour
{
    public static WorldChannelChatManager Instance { get; private set; }

    [SerializeField]
    private GameObject worldChannelMessagePage;

    [SerializeField]
    private GameObject receiveMessageBubble;
    [SerializeField]
    private GameObject sendMessageBubble;

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
        print("UpdateWorldChannelMessageBubble");
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
        textObj.text = worldChannelMessage.Message.content;

        bubble.transform.parent.SetParent(messageBubbleContent.transform);
        bubble.GetComponent<RectTransform>().localScale = Vector2.one;
        bubble.GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.preferredHeight + 20);

        bubble.transform.parent.GetComponent<RectTransform>().localScale = Vector2.one;
        bubble.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(bubble.GetComponent<RectTransform>().sizeDelta.x, textObj.preferredHeight + 20);
    }

    public void ToWorldChannelChatPage()
    {
        GameObject page = Instantiate(worldChannelMessagePage);
        page.transform.SetParent(transform);
        page.GetComponent<RectTransform>().localScale = Vector3.one;
        page.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        page.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        messageInputField = page.GetComponentInChildren<InputField>();
        messageBubbleContent = page.transform.Find("List/Viewport/Content").gameObject;
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