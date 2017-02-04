using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library.TextData;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
public class Chat_Control : MonoBehaviour
{

    float time = 0;
    public bool StopForTest;

    public GameObject UIControl;
    public GameObject ChatSet;
    public GameObject ChatContent;
    public GameObject ChatBPanel;

    public Button Back_Button;
    public Button Back2_Button;
    public Button View_Button;

    public bool ChaOut, MoveCha;
    bool ResetOnce = true;
    public bool MoveChaDetail,ChaDetailOut;

    float passtime = 0;
    int times = 0;
    float CanvasWidth;

    public Button ListButton;
    public GameObject FunctionList;
    public GameObject MessageManager;

    List<PlayerConversation> Conversations;
    List<int> People;

    public  void SetMessage()
    {
       
    }

    // Use this for initialization
    void Start()
    {
        SetGameObject();
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
        Conversations.Clear();
        Conversations = MessageManager.GetComponent<MessageManager>().Conversations;
        int MessageAmount = Conversations.Count;

        People.Clear();
        People.Add(UserManager.Instance.User.Player.PlayerID);
        for (int i = MessageAmount-1;i>=0;i--)
        {
           if(Conversations[i].message.senderPlayerID)
            //從最後的訊息開始記錄所有對話對象與最後的句子 People.add();
        }

        //點選對話對象後，執行以下式子
            int Person = 0;
            for (int l = 0; l < MessageAmount; l++)
            {
               if(Conversations[l].message.senderPlayerID == Person)
                 {
                //在該人物的對話紀錄下面生成文字氣泡
                int TextLong = Conversations[l].message.content.Length;
                  //根據對話長度調整對話框大小 
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
       if(!ChatBPanel)
            ChatBPanel = this.transform.GetChild(1).gameObject;
        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Button>();
        Back_Button.onClick.AddListener(BACK);
        if(!Back2_Button)
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
