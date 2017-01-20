using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class L_CampSelect : MonoBehaviour
{

    public GameObject UIControl;

    public GameObject[] Camp;
    public int CampNum; //0=農,1=商,2=動物
    public GameObject CampTitle;

    public Button RightButton, LeftButton, CampSelect;

    // Use this for initialization
    void Start()
    {

        UIControl = GameObject.FindWithTag("UImanager");

        int t = this.gameObject.transform.childCount;
        RightButton = this.gameObject.transform.GetChild(t - 2).GetComponent<Button>();
        RightButton.onClick.AddListener(Right);
        LeftButton = this.gameObject.transform.GetChild(t - 1).GetComponent<Button>();
        LeftButton.onClick.AddListener(Left);
        CampSelect = this.gameObject.transform.GetChild(t - 3).GetComponent<Button>();
        CampSelect.onClick.AddListener(Select);

        if (!CampTitle)
        { CampTitle = this.gameObject.transform.GetChild(t - 4).gameObject; }

        CampNum = 0;

    }
    void Update()
    {
        switch (CampNum)
        {
            case 0: CampTitle.GetComponent<Text>().text = ("陣營A"); break;
            case 1: CampTitle.GetComponent<Text>().text = ("陣營B"); break;
            case 2: CampTitle.GetComponent<Text>().text = ("陣營C"); break;
        }
    }
    void Right()
    {
        Camp[CampNum].SetActive(false);
        if (CampNum == 2) { CampNum = 0; } else { CampNum++; }
        Camp[CampNum].SetActive(true); 
    }
    void Left()
    {
        Camp[CampNum].SetActive(false);
        if (CampNum == 0) { CampNum = 2; } else { CampNum--; }
        Camp[CampNum].SetActive(true);
    }
    void Select()
    {
        UIControl.GetComponent<UImanager>().CampNum = CampNum;
        UIControl.GetComponent<UImanager>().Create_Charter();
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
    }

}
