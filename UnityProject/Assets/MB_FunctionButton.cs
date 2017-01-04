using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MB_FunctionButton : MonoBehaviour
{

    public Button yourButton;
    public GameObject MainBoat;
    public Button FunctionButton0;
    public GameObject Bag;
    public GameObject ShowBag;
    public Button FunctionButton1;
    public Button FunctionButton2;
    public GameObject FunctionList;
    float A,B;
    // Use this for initialization
    void Start()
    {
        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        // this.GetComponent<RectTransform>().localPosition = new Vector3(0, 90, 0);
        yourButton = this.gameObject.GetComponent<Button>();
        yourButton.onClick.AddListener(UpAndDown);

        FunctionButton0 = FunctionList.transform.GetChild(2).GetComponent<Button>();
        FunctionButton1 = FunctionList.transform.GetChild(3).GetComponent<Button>();
        FunctionButton2 = FunctionList.transform.GetChild(4).GetComponent<Button>();

        FunctionButton0.onClick.AddListener(Button0);
        FunctionButton1.onClick.AddListener(Button1);
        FunctionButton2.onClick.AddListener(Button2);
    }

    // Update is called once per frame

    void UpAndDown()
    {
       

        if (FunctionList.transform.GetComponent<RectTransform>().localPosition.x == B)
        {

            FunctionList.transform.GetComponent<RectTransform>().localPosition = new Vector3(A, FunctionList.transform.GetComponent<RectTransform>().localPosition.y, 0);
            Debug.Log(A);
        }
        else
        {
            FunctionList.transform.GetComponent<RectTransform>().localPosition = new Vector3(B, FunctionList.transform.GetComponent<RectTransform>().localPosition.y, 0);
            Debug.Log(B);
        }
    }

    void Button0()
    {
        Bag.GetComponent<ShowBag_pos>().enabled = true;
        ShowBag.active = true;
        MainBoat.active = false;
        FunctionList.transform.GetComponent<RectTransform>().localPosition = new Vector3(B, FunctionList.transform.GetComponent<RectTransform>().localPosition.y, 0);
    }
    void Button1()
    { }
    void Button2()
    { }
}
