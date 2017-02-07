using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Protocol;
using IsolatedIslandGame.Library;
using UnityEngine.EventSystems;
public class ChatASetScript : MonoBehaviour, IPointerClickHandler
{
    public bool StopForTest;
    public GameObject UIControl;
    public GameObject Chat;
    public GameObject ChatBPanel;
    public int PlayerID;
    public string PlayerName;
    public string Camp;
    public string LastSpeak;

    float CanvasWidth;
    public bool ShowChatB = false;
    float passtime = 0;
    int times = 0;

    // Use this for initialization
    void Awake()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!Chat)
            Chat = UIControl.GetComponent<UImanager>().UIObject[8];
        if (!ChatBPanel)
            ChatBPanel = Chat.transform.GetChild(1).gameObject;
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        if (ShowChatB)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                ChatBPanel.transform.localPosition = ChatBPanel.transform.localPosition - new Vector3(CanvasWidth / 10, 0, 0);
                if (times < 9)
                { times++; }
                else { times = 0; ShowChatB = false; }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!StopForTest)
        {
            //寫讀取人物
        }
        ShowChatB = true;

    }

    public void SetInfo(int InPlayerID)
    {
        PlayerID = InPlayerID;
        PlayerInformation Owner;
        PlayerInformationManager.Instance.FindPlayerInformation(InPlayerID, out Owner);
        PlayerName = Owner.nickname;
        switch (Owner.groupType)
        {
            case GroupType.Animal: Camp = (">動物-等級10"); break;
            case GroupType.Businessman: Camp = (">商業-等級8"); break;
            case GroupType.Farmer: Camp = (">農林-等級15"); break;
        }
        this.transform.GetChild(1).GetComponent<Text>().text = PlayerName;
        this.transform.GetChild(2).GetComponent<Text>().text = Camp;
    }
}
