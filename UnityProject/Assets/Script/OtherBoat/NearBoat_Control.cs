using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using System.Linq;
using UnityEngine.UI;
public class NearBoat_Control : MonoBehaviour {

    float time = 0;
    public bool StopForTest;
    public bool ShowList;

    public GameObject UIControl;
    public GameObject NearBoatPanel;
    public GameObject NearBoatSet;
    public GameObject NearBoatContent;

    public Button View_Button;
    public Button Back_Button;

    float CanvasWidth;
    public bool HideNearBool = false;
    float passtime = 0;
    int times = 0;

    // Use this for initialization
    void Start () {
        time = 0;
        SetGameObject();

    }
   void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!NearBoatPanel)
            NearBoatPanel = this.gameObject.transform.GetChild(4).gameObject;
        if (!NearBoatContent)
            NearBoatContent = NearBoatPanel.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;

        if (!Back_Button)
            Back_Button = NearBoatPanel.transform.GetChild(3).GetComponent<Button>();
        Back_Button.onClick.AddListener(HideList);
        if (!View_Button)
            View_Button = NearBoatPanel.transform.GetChild(4).GetComponent<Button>();
        View_Button.onClick.AddListener(View);
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        CanvasWidth = UIControl.GetComponent<UImanager>().Canvas.GetComponent<RectTransform>().rect.width;

        StopForTest= UIControl.GetComponent<UImanager>().StopForTest;
    }
    void Update()
    {
       
        if (!StopForTest)
            if(ShowList)
            {
                time += Time.deltaTime;
                if(time>10f)
                {
                    Reset();
                }
            }

        if (HideNearBool)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(CanvasWidth / 10, 0, 0);
                if (times < 9)
                { times++; }
                else { times = 0; HideNearBool = false; }
            }
        }
    }
  public  void Reset()
    {if (!StopForTest)
        {

            for (int i = 0, t = NearBoatContent.transform.childCount - 1; t >= 0 && i < 30; i++, t--)//清空
            {
                Destroy(NearBoatContent.transform.GetChild(t).gameObject);
            }
            foreach (var Boat in VesselManager.Instance.Vessels)//周圍的船
            { if(Boat.OwnerPlayerID != UserManager.Instance.User.Player.PlayerID)
                {
                    int OwnerID = Boat.OwnerPlayerID;
                    PlayerInformation Owner;
                    if (PlayerInformationManager.Instance.FindPlayerInformation(OwnerID, out Owner))
                    {
                        GameObject NewBoat = Instantiate(NearBoatSet, NearBoatContent.transform);
                        Debug.Log(Owner.nickname);
                        NewBoat.GetComponent<NearBoatSetScript>().SetInfo(Owner.playerID, Owner.nickname, Owner.groupType, Owner.signature);
                    }
                }
               
            }


        }
        //藍圖
       /* foreach(Blueprint b in BlueprintManager.Instance.Blueprints)
        {
            foreach(var requirement in b.Requirements)
            {
                int itemID = requirement.itemID;//....
            }
            foreach (var product in b.Products)
            {
                int itemID = product.itemID;//....
            }
            bool isSufficient = b.IsSufficientRequirements(new List<Blueprint.ElementInfo> { new Blueprint.ElementInfo { itemID = thinginA, positionIndex = placeA, itemCount = 1 } });
        }*/
       
      
    }
    void View() { }
  public  void HideList()
    {
        HideNearBool = true;
        /*if(!ShowList)
        {
            Reset();
            NearBoatPanel.transform.localPosition = new Vector3(0,45,0);
            ShowList = true;
        }
        else if(ShowList)
        {
            NearBoatPanel.transform.localPosition = new Vector3(391, 45, 0);
            ShowList = false;
        }*/
    }
}
