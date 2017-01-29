using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;
public class FunctionList_Control : MonoBehaviour
{
    public GameObject UIControl;
    public bool StopForTest;
    // public GameObject MainBoat;
    public GameObject FunctionList;
    private float y_value;

    public Button ListButton;

    public Button FunctionButton0;
    public GameObject Bag;
    public GameObject ShowBag;

    public Button FunctionButton1;
    public Button FunctionButton2;
    public Button FunctionButton3;
    public Button FunctionButton4;
    public Button FunctionButton5;
    float A, B;
   public  bool ShowListBool = false;
   public bool GoRightOrLeft = false;
    float passtime = 0;
    int times = 0;


    void Awake()
    {
        SetGameObject();

    }

    public void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;

        if (!FunctionList)
            FunctionList = this.gameObject;

        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        y_value = FunctionList.GetComponent<RectTransform>().localPosition.y;
  
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
        if (!FunctionButton5)
            FunctionButton5 = FunctionList.transform.GetChild(7).GetComponent<Button>();
        FunctionButton0.onClick.AddListener(Button0);
        FunctionButton1.onClick.AddListener(Button1);
        FunctionButton2.onClick.AddListener(Button2);
        FunctionButton3.onClick.AddListener(Button3);
        FunctionButton4.onClick.AddListener(Button4);
        FunctionButton5.onClick.AddListener(Button5);
       
    }



    void Update()
    {      
        if (ShowListBool)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                if (GoRightOrLeft)
                { FunctionList.transform.localPosition = FunctionList.transform.localPosition + new Vector3((A - B) / 4, 0, 0); }
                else
                { FunctionList.transform.localPosition = FunctionList.transform.localPosition - new Vector3((A - B) / 4, 0, 0); }
                if (times < 3)
                { times++; }
                else { times = 0; ShowListBool = false; }
            }
        }
    }


   public void ShowList()
    {
        ShowListBool = true;
        GoRightOrLeft = !GoRightOrLeft;
    }


    void Button0()
    {
        ShowList();
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Show_Bag);
    }
    void Button1()
    { }
    void Button2()
    {
        ShowList();
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Mission);
    }
    void Button3()
    {
        ShowList();
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Friend);
    }
    void Button4()
    {
        ShowList();
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Chat);
    }
    void Button5()
    {
        //開啟抽素材動畫
        //抽素材
        UserManager.Instance.User.Player.OperationManager.DrawMaterial();
        UserManager.Instance.User.Player.ResponseManager.OnDrawMaterialResponse += (returnCode, item, count) =>
        {
            switch (returnCode)
            {
                case IsolatedIslandGame.Protocol.ErrorCode.NoError:
                    break;
            }
        };
    }
}
