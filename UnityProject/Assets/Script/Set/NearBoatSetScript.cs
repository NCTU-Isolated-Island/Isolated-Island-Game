using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Protocol;
using UnityEngine.EventSystems;
public class NearBoatSetScript : MonoBehaviour, IPointerClickHandler
{
    public bool StopForTest;
    public GameObject UIControl;
    public GameObject OtherBoat;
    public int PlayerID;
    public GameObject PlayerName;
    public GameObject Camp;
    public GameObject Speach;

    // Use this for initialization
    void Awake()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!OtherBoat)
            OtherBoat = UIControl.GetComponent<UImanager>().UIObject[2];
        if (!PlayerName)
            PlayerName = this.gameObject.transform.GetChild(1).gameObject;
        if (!Camp)
            Camp = this.gameObject.transform.GetChild(2).gameObject;
        if (!Camp)
            Speach = this.gameObject.transform.GetChild(4).gameObject;
    }
    void Update()
    {
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Map)
            OtherBoat.GetComponent<OtherBoat_Control>().WhereIFrom = (int)UImanager.UI.Map;
        else if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Friend)
            OtherBoat.GetComponent<OtherBoat_Control>().WhereIFrom = (int)UImanager.UI.Friend;

        if(!StopForTest)
        {
            GameObject target;
            GameManager.Instance.UserGameObject.TryGetValue(PlayerID, out target);
            CameraManager.Instance.ToNearAnchor(target);
        }    
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Other_Boat);
        OtherBoat.GetComponent<OtherBoat_Control>().Reset(PlayerID);
    }

    public void SetInfo(int InPlayerID,string InPlayerName, GroupType InCamp, string InSpeach)
    {
        PlayerID = InPlayerID;
        Debug.Log("InPlayerName");
        PlayerName.GetComponent<Text>().text = InPlayerName;
        switch (InCamp)
        {
            case GroupType.Animal: Camp.GetComponent<Text>().text = (">動物-等級10"); break;
            case GroupType.Businessman: Camp.GetComponent<Text>().text = (">商業-等級8"); break;
            case GroupType.Farmer: Camp.GetComponent<Text>().text = (">農林-等級15"); break;
        }
        Speach.GetComponent<Text>().text = InSpeach;
    }
}
