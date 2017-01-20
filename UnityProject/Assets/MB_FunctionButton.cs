using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MB_FunctionButton : MonoBehaviour
{
    public GameObject UIControl;

   // public GameObject MainBoat;
    public GameObject FunctionList;
    private float y_value;

    public Button ListButton;

    public Button MapButton;

    public Button FunctionButton0;
    public GameObject Bag;
    public GameObject ShowBag;

    public GameObject my_boat_text1,my_boat_text2;

    public Button FunctionButton1;
    public Button FunctionButton2;
    public Button FunctionButton3;
    public Button FunctionButton4;

    float A, B;
    // Use this for initialization
    void Start()
    {
        SetGameObject();
       
    }

    public void SetGameObject()
    {
        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        y_value = FunctionList.GetComponent<RectTransform>().localPosition.y;

        if(!ListButton)
        ListButton = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);

        if (!MapButton)
            MapButton = this.gameObject.transform.GetChild(1).GetComponent<Button>();
        MapButton.onClick.AddListener(ShowMap);

        if (!FunctionButton0)
            FunctionButton0 = FunctionList.transform.GetChild(2).GetComponent<Button>();
        if (!FunctionButton1)
            FunctionButton1 = FunctionList.transform.GetChild(3).GetComponent<Button>();
        if (!FunctionButton2)
            FunctionButton2 = FunctionList.transform.GetChild(4).GetComponent<Button>();
        if (!FunctionButton3)
            FunctionButton3 = FunctionList.transform.GetChild(5).GetComponent<Button>();
        if (!FunctionButton4)
            FunctionButton4 = FunctionList.transform.GetChild(6).GetComponent<Button>();

        FunctionButton0.onClick.AddListener(Button0);
        FunctionButton1.onClick.AddListener(Button1);
        FunctionButton2.onClick.AddListener(Button2);
        FunctionButton3.onClick.AddListener(Button3);
        FunctionButton4.onClick.AddListener(Button4);

        my_boat_text1.GetComponent<Text>().text = (UIControl.GetComponent<UImanager>().player_name + "的船");
        my_boat_text2.GetComponent<Text>().text = my_boat_text1.GetComponent<Text>().text;
    }



   /* void Update()
    {
        if(y_value != FunctionList.transform.GetComponent<RectTransform>().localPosition.y)
        { y_value = FunctionList.transform.GetComponent<RectTransform>().localPosition.y; }
    }*/


    void ShowList()
    {
        if (FunctionList.transform.GetComponent<RectTransform>().localPosition.x == B)
        {
            FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(A, y_value, 0);      
        }
        else
        {
            FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        }
    }
    void ShowMap()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Map;
    }
    void Button0()
    {
       // Bag.GetComponent<ShowBag_pos>().enabled = true;
       // ShowBag.active = true;
       // MainBoat.active = false;
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Show_Bag);
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Show_Bag;
    }
    void Button1()
    { }
    void Button2()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Show_Bag;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Mission);
    }
    void Button3()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Friend);
    }
    void Button4()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Chat);
    }
}
