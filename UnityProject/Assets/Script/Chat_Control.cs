using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
public class Chat_Control : MonoBehaviour
{

    float time = 0;
    public bool StopForTest;

    public GameObject UIControl;
    public GameObject ChatSet;
    public GameObject ChatContent;
   
    public Button Back_Button;
    public Button View_Button;

    // Use this for initialization
    void Start()
    {
        SetGameObject();
        if (!StopForTest)
            Reset();
    }
    void Reset()
    {

    }
    void BACK()
    {
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
    }
    void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!ChatSet) ;
        if (!ChatContent)
            ChatContent = this.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).gameObject;
       
        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(0).GetChild(3).GetComponent<Button>();
        Back_Button.onClick.AddListener(BACK);
        if (!View_Button)
            View_Button = this.gameObject.transform.GetChild(0).GetChild(4).GetComponent<Button>();
    }

}
