using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
public class OtherBoat_Control : MonoBehaviour {

    public GameObject UIControl;
    public Button Back_Button;
    public int WhereIFrom;
    public GameObject Title;
    public GameObject Name, Camp, Speach;

    public Button ListButton;
    public GameObject FunctionList;

    void Awake () {
        SetGameObject();
    }
    void SetGameObject()
    {

        if (WhereIFrom == 0)
            WhereIFrom = 1;
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(2).GetChild(8).GetComponent<Button>();
        Back_Button.onClick.AddListener(Back);
        if (!Name)
            Name = this.gameObject.transform.GetChild(2).GetChild(0).gameObject;
        if (!Camp)
            Camp = this.gameObject.transform.GetChild(2).GetChild(1).gameObject;
        if (!Speach)
            Speach = this.gameObject.transform.GetChild(2).GetChild(3).gameObject;
        if (!Title)
            Title = this.gameObject.transform.GetChild(0).GetChild(1).gameObject;

        if (!ListButton)
            ListButton = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);
        if (!FunctionList)
            FunctionList = UIControl.GetComponent<UImanager>().FunctionList;
    }
    void ShowList()
    {
        FunctionList.GetComponent<FunctionList_Control>().ShowList();
    }
    public void Reset(int InOwnerID)
    {
        SetGameObject();
        PlayerInformation Owner;
        PlayerInformationManager.Instance.FindPlayerInformation(InOwnerID, out Owner);
        Title.GetComponent<Text>().text = (Owner.nickname );
        Name.GetComponent<Text>().text = Owner.nickname;
        switch (Owner.groupType)
        {
            case GroupType.Animal: Camp.GetComponent<Text>().text = (">動物-等級10"); break;
            case GroupType.Businessman: Camp.GetComponent<Text>().text = (">商業-等級8"); break;
            case GroupType.Farmer: Camp.GetComponent<Text>().text = (">農林-等級15"); break;
        }
        Speach.GetComponent<Text>().text = Owner.signature;
    }
	void Back()
    {
          UIControl.GetComponent<UImanager>().BlackFade = true;
          UIControl.GetComponent<UImanager>().ChangeUI(WhereIFrom);
    }
  
}
