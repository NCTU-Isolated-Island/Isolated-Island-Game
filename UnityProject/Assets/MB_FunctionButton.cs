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

    public Button FunctionButton1;

    public Button FunctionButton2;


    float A, B;
    // Use this for initialization
    void Start()
    {
        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        y_value = FunctionList.GetComponent<RectTransform>().localPosition.y;

        ListButton = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);

        MapButton = this.gameObject.transform.GetChild(1).GetComponent<Button>();
        MapButton.onClick.AddListener(ShowMap);

        FunctionButton0 = FunctionList.transform.GetChild(2).GetComponent<Button>();
        FunctionButton1 = FunctionList.transform.GetChild(3).GetComponent<Button>();
        FunctionButton2 = FunctionList.transform.GetChild(4).GetComponent<Button>();

        FunctionButton0.onClick.AddListener(Button0);
        FunctionButton1.onClick.AddListener(Button1);
        FunctionButton2.onClick.AddListener(Button2);
    }

    void Update()
    {
        if(y_value != FunctionList.transform.GetComponent<RectTransform>().localPosition.y)
        { y_value = FunctionList.transform.GetComponent<RectTransform>().localPosition.y; }
    }

    // Update is called once per frame

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
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Show_Bag;
    }
    void Button1()
    { }
    void Button2()
    { }
}
