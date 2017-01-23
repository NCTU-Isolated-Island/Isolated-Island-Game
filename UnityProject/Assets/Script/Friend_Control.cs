using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;
public class Friend_Control : MonoBehaviour {

    float time = 0;
    public bool StopForTest;

    public GameObject UIControl;
    public GameObject FriendSet;
    public GameObject FriendContent;
    public GameObject OtherBoat;
    public Button Back_Button;
    public Button View_Button;

    public bool FriOut, MoveFri;
    bool ResetOnce = true;
    float passtime = 0;
    int times = 0;
    float CanvasWidth;

    // Use this for initialization
    void Start () {
       
        SetGameObject();
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        if(!StopForTest)
        Reset();

    }
    void Update()
    {

        if (MoveFri)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                if (ResetOnce)
                {
                    if (!StopForTest)
                    { Reset(); }
                    ResetOnce = false;
                }
                if (FriOut)
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(CanvasWidth / 10, 0, 0);
                else
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(CanvasWidth / 10, 0, 0);
                if (times < 9)
                { times++; }
                else
                {
                    times = 0; MoveFri = false; FriOut = !FriOut; ResetOnce = true;
                    if (FriOut == false)
                        this.gameObject.SetActive(false);
                }
            }

        }
    }

    void Reset()
    {
        for (int i = 0, t = FriendContent.transform.childCount - 1; t >= 0 && i < 30; i++, t--)
        {
            Destroy(FriendContent.transform.GetChild(t).gameObject);
        }
        foreach (FriendInformation f in UserManager.Instance.User.Player.FriendInformations)//抓朋友
        {
            PlayerInformation Owner;
            if (PlayerInformationManager.Instance.FindPlayerInformation(f.friendPlayerID, out Owner))
            {
                GameObject newFriend = Instantiate(FriendSet, FriendContent.transform);
                newFriend.GetComponent<NearBoatSetScript>().SetInfo(Owner.playerID, Owner.nickname,Owner.groupType,Owner.signature);
            }
        }
    }
	void BACK()
    {
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
        MoveFri = true;
    }
    void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!FriendSet) ;
        if (!FriendContent)
            FriendContent = this.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        if (!OtherBoat)
            OtherBoat = UIControl.GetComponent<UImanager>().UIObject[2];
        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(3).GetComponent<Button>();
        Back_Button.onClick.AddListener(BACK);
        if (!View_Button)
            View_Button = this.gameObject.transform.GetChild(4).GetComponent<Button>();

        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;
        this.gameObject.transform.localPosition = new Vector3(CanvasWidth,0 , 0);
    }
           
}
