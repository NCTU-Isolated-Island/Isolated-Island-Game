using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MessageSetScript : MonoBehaviour, IPointerClickHandler
{

    public bool StopForTest;
    public GameObject UIControl;

    public GameObject ChatPanel;

    void Start()
    {
        SetGameObject();
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!StopForTest) //資料傳入細節版
        {

        }
        ChatPanel.GetComponent<Chat_Control>().SetMessage();
        ChatPanel.GetComponent<Chat_Control>().MoveChaDetail = true;
    }
    void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");

        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        if (!ChatPanel)
            ChatPanel = UIControl.GetComponent<UImanager>().UIObject[8];
       

    }
}
