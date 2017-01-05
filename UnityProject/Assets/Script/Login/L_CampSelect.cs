using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L_CampSelect : MonoBehaviour {

    public GameObject UIControl;

    public GameObject[] Camp;
    public int CampNum;
    public GameObject CampTitle;

    public Button RightButton,LeftButton;

    // Use this for initialization
    void Start () {

        UIControl = GameObject.FindWithTag("UImanager");

        int t = this.gameObject.transform.GetChildCount();
        RightButton = this.gameObject.transform.GetChild(t-2).GetComponent<Button>();
        RightButton.onClick.AddListener(Right);
        LeftButton = this.gameObject.transform.GetChild(t - 1).GetComponent<Button>();
        LeftButton.onClick.AddListener(Left);

        if(!CampTitle)
        { CampTitle = this.gameObject.transform.GetChild(t - 4).gameObject; }
      
        CampNum = 0;

    }
    void Update()
    {switch(CampNum)
        {
            case 0: CampTitle.GetComponent<Text>().text = ("陣營A"); break;
            case 1: CampTitle.GetComponent<Text>().text = ("陣營B"); break;
            case 2: CampTitle.GetComponent<Text>().text = ("陣營C"); break;
        }
    }
	void Right()
    {
        Camp[CampNum].active = false;
        if(CampNum == 2) { CampNum = 0; } else { CampNum++; }
        Camp[CampNum].active = true;
    }
    void Left()
    {
        Camp[CampNum].active = false;
        if (CampNum == 0) { CampNum = 2; } else { CampNum--; }
        Camp[CampNum].active = true;
    }
	// Update is called once per frame

}
