using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;
public class MB_FunctionButton : MonoBehaviour
{
    public GameObject UIControl;
    public bool StopForTest;
    private float y_value;
    
    public Button ListButton;
    public GameObject FunctionList;

    public Button MapButton;

    public GameObject my_boat_text1, my_boat_text2;

    float A, B;




    void Start()
    {
        SetGameObject();

    }

    public void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        y_value = FunctionList.GetComponent<RectTransform>().localPosition.y;

        if (!ListButton)
            ListButton = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);

        if (!MapButton)
            MapButton = this.gameObject.transform.GetChild(1).GetComponent<Button>();
        MapButton.onClick.AddListener(ShowMap);

        if(!StopForTest)
        my_boat_text1.GetComponent<Text>().text = (UserManager.Instance.User.Player.Nickname);
        my_boat_text2.GetComponent<Text>().text = my_boat_text1.GetComponent<Text>().text;

        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
    }

    void ShowList()
    {
        FunctionList.GetComponent<FunctionList_Control>().ShowList();
    }


    void ShowMap()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        FunctionList.GetComponent<FunctionList_Control>().GoRightOrLeft = false;
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Map;
    }

}
