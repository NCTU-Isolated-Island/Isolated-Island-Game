using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class L_CampSelect : MonoBehaviour
{

    public GameObject UIControl;

    public GameObject[] Camp;
    public int CampNum; //0=農,1=商,2=動物
    public GameObject CampTitle;

    public Button RightButton, LeftButton, CampSelect;
    public GameObject Confirm;
    // Use this for initialization
    void Start()
    {

        UIControl = GameObject.FindWithTag("UImanager");

        int t = this.gameObject.transform.childCount;
        RightButton = this.gameObject.transform.GetChild(t - 3).GetComponent<Button>();
        RightButton.onClick.AddListener(Right);
        LeftButton = this.gameObject.transform.GetChild(t - 2).GetComponent<Button>();
        LeftButton.onClick.AddListener(Left);
        CampSelect = this.gameObject.transform.GetChild(t - 4).GetComponent<Button>();
        CampSelect.onClick.AddListener(Select);

        if (!CampTitle)
        { CampTitle = this.gameObject.transform.GetChild(t - 5).gameObject; }

        CampNum = 0;

        if (!Confirm)
            Confirm = this.gameObject.transform.GetChild(8).gameObject;
        Confirm.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Yes);
        Confirm.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(No);
    }
    void Update()
    {
        switch (CampNum)
        {
            case 0: CampTitle.GetComponent<Text>().text = ("Farmer");
                //CampTitle.transform.GetChild(0).GetComponent<Text>().text =
                break;
            case 1: CampTitle.GetComponent<Text>().text = ("Businessman");

                break;
            case 2: CampTitle.GetComponent<Text>().text = ("Animal");

                break;
        }
    }
    void Right()
    {
        if (!Confirm.activeInHierarchy)
        {
            Camp[CampNum].SetActive(false);
            if (CampNum == 2) { CampNum = 0; } else { CampNum++; }
            Camp[CampNum].SetActive(true);
        }
      
    }
    void Left()
    {
        if (!Confirm.activeInHierarchy)
        {
            Camp[CampNum].SetActive(false);
        if (CampNum == 0) { CampNum = 2; } else { CampNum--; }
        Camp[CampNum].SetActive(true);
        }
    }
    void Select()
    {
        if (!Confirm.activeInHierarchy)
        {
            Confirm.SetActive(true);
        }
    }
    void Yes()
    {
        UIControl.GetComponent<UImanager>().CampNum = CampNum;
        UIControl.GetComponent<UImanager>().Create_Charter();
        this.gameObject.transform.parent.GetComponent<L_Loading>().LoadingCase = 0;
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
        this.gameObject.SetActive(false);
    }
    void No()
    {
        Confirm.SetActive(false);
    }

}
