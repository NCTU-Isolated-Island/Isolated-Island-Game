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

    public bool ChaOut, MoveCha;
    bool ResetOnce = true;
    float passtime = 0;
    int times = 0;
    float CanvasWidth;

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
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(CanvasWidth / 10, 0, 0);
                else
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(CanvasWidth / 10, 0, 0);
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
    }
    void Reset()
    {

    }

    void BACK()
    {
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
        MoveCha = true;
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

        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;
        this.gameObject.transform.localPosition = new Vector3(CanvasWidth, 0, 0);
    }

}
