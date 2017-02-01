using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class Mission_Control : MonoBehaviour {

    float time = 0;
    public bool StopForTest;
    public bool ShowDetailBool;

    public GameObject UIControl;
    public GameObject MissionDetailPanel;
    int Page = 1;
    public GameObject Page1, Page2;
    public Button Right, Left;
    public float CanvasWidth;
    public GameObject MissionSet;
    public GameObject MissionContent;

    public Button View_Button;
    public Button Back_Button;

    public Sprite[] Pic_ICON;

    public bool MisOut, MoveMis;
    bool ResetOnce = true;
    float passtime = 0;
    int times = 0;
    float CanvasHeight;
    // Use this for initialization
    void Start () {
        SetGameObject();
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        ShowDetailBool = false;
        if (!StopForTest)
        { Reset(); }
    }
    void Update()
    { 
        if (MoveMis)
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
                if (MisOut)
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(0, CanvasHeight / 10, 0);
                else
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(0, CanvasHeight / 10, 0);
                if (times < 9)
                { times++; }
                else
                {
                    times = 0; MoveMis = false; MisOut = !MisOut; ResetOnce = true;
                    if(MisOut == false)
                    this.gameObject.SetActive(false);
                }
            }

        }

    }
    void Back()
    {
        if(ShowDetailBool)
        { ShowDetail(); }
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
        MoveMis = true;     
    }
    void View()
    {

    }

   public void ShowDetail()
    {
        if(ShowDetailBool)
        {
            MissionDetailPanel.SetActive(false);
            ShowDetailBool = false;
            //重置位置
            Page1.transform.localPosition = new Vector3(0, 0, 0);
            Page2.transform.localPosition = new Vector3(CanvasWidth, 0, 0);
            Page = 1;
            MissionDetailPanel.transform.GetChild(3).GetComponent<Text>().text = ("Page 1/2");
            Left.targetGraphic.color = new Color(255f, 255f, 255f, 130f);
            Left.interactable = false;
            Right.targetGraphic.color = new Color(255f, 255f, 255f, 255);
            Right.interactable = true;
        }
        else if(!ShowDetailBool)
        {
            MissionDetailPanel.SetActive(true);
            ShowDetailBool = true;
            //更新細節
        }
    }
    void ChangePage()
    {
        if(Page == 1)
        {
            Page1.transform.localPosition = new Vector3(-CanvasWidth, 0, 0);
            Page2.transform.localPosition = new Vector3(0, 0, 0);

            Page = 2;
            MissionDetailPanel.transform.GetChild(3).GetComponent<Text>().text = ("Page 2/2");
            Right.targetGraphic.color = new Color(255f, 255f, 255f, 130f);
            Right.interactable = false;
            Left.targetGraphic.color = new Color(255f, 255f, 255f, 255);
            Left.interactable = true;
        }
        else if(Page == 2)
        {  
            Page1.transform.localPosition = new Vector3(0, 0, 0);
            Page2.transform.localPosition = new Vector3(CanvasWidth, 0, 0);

            Page = 1;
            MissionDetailPanel.transform.GetChild(3).GetComponent<Text>().text = ("Page 1/2");
            Left.targetGraphic.color = new Color(255f, 255f, 255f, 130f);
            Left.interactable = false;
            Right.targetGraphic.color = new Color(255f, 255f, 255f, 255);
            Right.interactable = true;
        }
    }
    void Reset()
    {

    }
    void SetGameObject()
    {
        
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!MissionDetailPanel)
            MissionDetailPanel = this.gameObject.transform.GetChild(5).gameObject;
        if (!MissionContent)
            MissionContent = this.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;

        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(3).GetComponent<Button>();
        Back_Button.onClick.AddListener(Back);
        if (!View_Button)
            View_Button = this.gameObject.transform.GetChild(4).GetComponent<Button>();
        View_Button.onClick.AddListener(View);

        Right =  MissionDetailPanel.transform.GetChild(2).GetComponent<Button>();
        Right.onClick.AddListener(ChangePage);
        Left = MissionDetailPanel.transform.GetChild(4).GetComponent<Button>();
        Left.onClick.AddListener(ChangePage);

        MissionDetailPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ShowDetail);

        if (!Page1)
            Page1 = MissionDetailPanel.transform.GetChild(5).gameObject;
        if (!Page2)
            Page2 = MissionDetailPanel.transform.GetChild(6).gameObject;

        Page = 1;
        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;
        CanvasHeight = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.height;     

        this.gameObject.transform.localPosition = new Vector3(0, -CanvasHeight , 0);
    }
}
