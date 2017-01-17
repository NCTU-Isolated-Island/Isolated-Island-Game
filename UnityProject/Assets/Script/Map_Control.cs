using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Control : MonoBehaviour {

    public GameObject UIControl;

    public Button Map_BackButton;
    public Button ShowList_Button;

    public GameObject FunctionList;

    float A, B, y_value;

    // Use this for initialization
    void Start () {
        if(FunctionList == null)
        FunctionList = this.gameObject.transform.GetChild(1).gameObject;

        A = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 + FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        B = -FunctionList.transform.parent.parent.GetComponent<RectTransform>().rect.width / 2 - FunctionList.transform.GetComponent<RectTransform>().rect.width / 2;
        y_value = FunctionList.GetComponent<RectTransform>().localPosition.y;

        if (UIControl == null)
            UIControl = GameObject.FindWithTag("UImanager");

        if (ShowList_Button == null)
            ShowList_Button = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        ShowList_Button.onClick.AddListener(ShowList);

        if (Map_BackButton == null)
            Map_BackButton = this.gameObject.transform.GetChild(2).GetComponent<Button>();
        Map_BackButton.onClick.AddListener(Back);    
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Back()
    {
        FunctionList.GetComponent<RectTransform>().localPosition = new Vector3(B, y_value, 0);
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
    }
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
}
