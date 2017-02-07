using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MissionSetScript : MonoBehaviour, IPointerClickHandler
{

    public bool StopForTest;
    public GameObject UIControl;
    public GameObject Mission;
    public GameObject DetailPanel;

    public Sprite[] Pic_ICON;

    public int MissionID;
    public string MissionName;
    public string MissionTime;
    public string MissionType;
    public string MissionInfo;
    public string MissionNeed;
    public string MissionPrize;
    public string MissionPlace;

    void Start () {
        GameObjectSet();
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
    }
	

	void GameObjectSet() {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!Mission)
            Mission = UIControl.GetComponent<UImanager>().UIObject[6];
        if (!DetailPanel)
            DetailPanel = Mission.transform.GetChild(5).gameObject;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!StopForTest) //資料傳入細節版
        {
           
        }
        Mission.GetComponent<Mission_Control>().ShowDetail();
    }
}
