using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library.TextData;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;

public class Chat_Control : MonoBehaviour
{
    public static Chat_Control Instance;

    float time = 0;
    public bool StopForTest;

    public GameObject UIControl;
    public GameObject ChatSet;
    public GameObject ChatContent;
    public GameObject ChatBPanel;
    public GameObject ChatBContent;

    public Button Back_Button;
    public Button Back2_Button;
    public Button View_Button;

    public bool ChaOut, MoveCha;
    bool ResetOnce = true;
    public bool MoveChaDetail, ChaDetailOut;

    float passtime = 0;
    int times = 0;
    float CanvasWidth;

    public Button ListButton;
    public GameObject FunctionList;
    public GameObject MessageManager;

    List<PlayerConversation> Conversations = new List<PlayerConversation>();
    public List<int> People = new List<int>();
  public  GameObject ChatBubbleA, ChatBubbleB;


    // Use this for initialization
    void Start()
    {
        SetGameObject();
        if (Instance == null)
            Instance = this;
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        if (!StopForTest)
            Reset();
    }
    void Update()
    {
        if (MoveCha)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                if (ResetOnce)
                {
                    if (!StopForTest)
                    { Reset(); }
                    ResetOnce = false;
                }
                if (ChaOut)
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(CanvasWidth / 10, 0, 0);
                else
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(CanvasWidth / 10, 0, 0);
                if (times < 9)
                { times++; }
                else
                {
                    times = 0; MoveCha = false; ChaOut = !ChaOut; ResetOnce = true;
                    if (ChaOut == false)
                        this.gameObject.SetActive(false);
                }
            }

        }
        if (MoveChaDetail)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                if (ChaDetailOut)
                    ChatBPanel.transform.localPosition = ChatBPanel.transform.localPosition - new Vector3(CanvasWidth / 10, 0, 0);
                else
                    ChatBPanel.transform.localPosition = ChatBPanel.transform.localPosition + new Vector3(CanvasWidth / 10, 0, 0);
                if (times < 9)
                { times++; }
                else
                {
                    times = 0; MoveChaDetail = false; ChaDetailOut = !ChaDetailOut;
                }
            }
        }
    }
    void Reset()
    {
        if(Conversations != null)
        Conversations.Clear();
        Conversations = MessageManager.GetComponent<MessageManager>().Conversations;
        int MessageAmount = Conversations.Count;
        if (People != null)
            People.Clear();
        Debug.Log(UserManager.Instance.User.Player.PlayerID);
        if(UserManager.Instance.User.Player.PlayerID == 28)
        People.Add(UserManager.Instance.User.Player.PlayerID);

        for (int i = 0, t = ChatContent.transform.childCount - 1; t >= 0 && i < 30; i++, t--)
        {
            Destroy(ChatContent.transform.GetChild(t).gameObject);
        }
        for (int i = MessageAmount - 1; i >= 0; i--)
        {
            if (!People.Contains(Conversations[i].receiverPlayerID))
            {
                PlayerInformation Owner;
                PlayerInformationManager.Instance.FindPlayerInformation(Conversations[i].receiverPlayerID, out Owner);

                People.Add(Conversations[i].receiverPlayerID);
                GameObject Instance = Instantiate(ChatSet, ChatContent.transform);
                Instance.GetComponent<MessageSetScript>().PlayerID = Conversations[i].receiverPlayerID;
                Instance.transform.GetChild(1).GetComponent<Text>().text = Owner.nickname;
                switch (Owner.groupType)
                {
                    case GroupType.Animal: Instance.transform.GetChild(2).GetComponent<Text>().text = (">動物-等級10"); break;
                    case GroupType.Businessman: Instance.transform.GetChild(2).GetComponent<Text>().text = (">商業-等級8"); break;
                    case GroupType.Farmer: Instance.transform.GetChild(2).GetComponent<Text>().text = (">農林-等級15"); break;
                }
                Instance.transform.GetChild(4).GetComponent<Text>().text = Conversations[i].message.content;
            }
            if (!People.Contains(Conversations[i].message.senderPlayerID))
            {
                PlayerInformation Owner;
                PlayerInformationManager.Instance.FindPlayerInformation(Conversations[i].message.senderPlayerID, out Owner);

                People.Add(Conversations[i].message.senderPlayerID);
                GameObject Instance = Instantiate(ChatSet, ChatContent.transform);
                Instance.GetComponent<MessageSetScript>().PlayerID = Conversations[i].message.senderPlayerID;
                Instance.transform.GetChild(1).GetComponent<Text>().text = Owner.nickname;
                switch (Owner.groupType)
                {
                    case GroupType.Animal: Instance.transform.GetChild(2).GetComponent<Text>().text = (">動物-等級10"); break;
                    case GroupType.Businessman: Instance.transform.GetChild(2).GetComponent<Text>().text = (">商業-等級8"); break;
                    case GroupType.Farmer: Instance.transform.GetChild(2).GetComponent<Text>().text = (">農林-等級15"); break;
                }
                Instance.transform.GetChild(4).GetComponent<Text>().text = Conversations[i].message.content;
            }
        }

        

       

    }
    //點選對話對象後，執行以下式子
    public void ResetDetail(int target = 22)
    {
        if (Conversations != null)
            Conversations.Clear();
        Conversations = MessageManager.GetComponent<MessageManager>().Conversations;
        int MessageAmount = Conversations.Count;

        float Y = -50;
        int Person = target;

        for (int i = 0, t = ChatBContent.transform.childCount - 1; t >= 0 && i < 30; i++, t--)
        {
            Destroy(ChatBContent.transform.GetChild(t).gameObject);
        }
        for (int l = 0; l < MessageAmount; l++)
        {
            if (Conversations[l].receiverPlayerID == Person)
            {  //在該人物的對話紀錄下面生成文字氣泡
                GameObject Instance = Instantiate(ChatBubbleA, ChatBContent.transform);
                Instance.GetComponent<BubbleSet>().AorB = 0;
                Instance.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = (Conversations[l].message.content);
                //根據對話長度調整對話框大小  
                int TextLong = Conversations[l].message.content.Length;
                Instance.GetComponent<BubbleSet>().SetSize(TextLong);
                Instance.transform.position = new Vector3(0, Y, 0);
                Y = Instance.GetComponent<RectTransform>().rect.height / 2 + 27;
            }
            else if (Conversations[l].message.senderPlayerID == Person)
            {
                GameObject Instance = Instantiate(ChatBubbleB, ChatBContent.transform);
                Instance.GetComponent<BubbleSet>().AorB = 1;
                Instance.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = (Conversations[l].message.content);
                //根據對話長度調整對話框大小  
                int TextLong = Conversations[l].message.content.Length;
                Instance.GetComponent<BubbleSet>().SetSize(TextLong);
                Instance.transform.position = new Vector3(0, Y, 0);
                Y = Instance.GetComponent<RectTransform>().rect.height / 2 + 27;

            }

        }
    }
    void BACK()
    {
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
        MoveCha = true;
    }
    void BACK2()
    {

        MoveChaDetail = true;
    }
    void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!ChatSet) ;
        if (!ChatContent)
            ChatContent = this.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).gameObject;
        if (!ChatBPanel)
            ChatBPanel = this.transform.GetChild(1).gameObject;
        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Button>();
        Back_Button.onClick.AddListener(BACK);
        if (!Back2_Button)
            Back2_Button = this.gameObject.transform.GetChild(1).GetChild(3).GetComponent<Button>();
        Back2_Button.onClick.AddListener(BACK2);
        if (!View_Button)
            View_Button = this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<Button>();

        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;
        

        this.gameObject.transform.localPosition = new Vector3(-CanvasWidth, 0, 0);
        if (!ListButton)
            ListButton = this.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);
        if (!FunctionList)
            FunctionList = UIControl.GetComponent<UImanager>().FunctionList;
    }
    void ShowList()
    {
        FunctionList.GetComponent<FunctionList_Control>().ShowList();
    }
}
