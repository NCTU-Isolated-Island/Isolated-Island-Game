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

    public GameObject NearBoatPanel;
    public GameObject NearBoatSet;
    public GameObject NearBoatContent;
    public Button Show_Button;
    public Button Back_Button_A;
    public Button Back_Button_B;
    public Button View_Button;

    // Use this for initialization
    void Start () {
        time = 0;
        SetGameObject();

    }
   void SetGameObject()
    {
        if (!NearBoatPanel)
            NearBoatPanel = this.gameObject.transform.GetChild(4).gameObject;
        if (!NearBoatContent)
            NearBoatContent = NearBoatPanel.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;

        if (!Back_Button_A)
            Back_Button_A = NearBoatPanel.transform.GetChild(3).GetComponent<Button>();
        Back_Button_A.onClick.AddListener(Back);
        if (!Show_Button)
            Show_Button = this.gameObject.transform.GetChild(3).GetComponent<Button>();
        Show_Button.onClick.AddListener(MoveList);

        if (!Back_Button_B)
            Back_Button_B = NearBoatPanel.transform.GetChild(3).GetComponent<Button>();
        Back_Button_B.onClick.AddListener(MoveList);
        if (!View_Button)
            View_Button = NearBoatPanel.transform.GetChild(4).GetComponent<Button>();
    }
    void Back()
    {

    }
    void Update()
    {
        if(!StopForTest)
            if(ShowList)
            {
                time += Time.deltaTime;
                if(time>10f)
                {
                    Reset();
                }
            }
    }
    void Reset()
    {
        for (int i = 0, t = NearBoatContent.transform.childCount - 1; t >= 0 && i < 30; i++, t--)
        {
            Destroy(NearBoatContent.transform.GetChild(t).gameObject);
        }
        foreach (var Boat in VesselManager.Instance.Vessels)//周圍的船
        {
            int OwnerID = Boat.OwnerPlayerID;
            PlayerInformation Owner;
            if (PlayerInformationManager.Instance.FindPlayerInformation(OwnerID, out Owner))
            {
                GameObject NewBoat = Instantiate(NearBoatSet, NearBoatContent.transform);
                NewBoat.GetComponent<NearBoatSetScript>().SetInfo(Owner.nickname, Owner.groupType, Owner.signature);              
            }
        }

        foreach(FriendInformation f in UserManager.Instance.User.Player.FriendInformations)
        {//抓朋友
            PlayerInformation Owner;
            if (PlayerInformationManager.Instance.FindPlayerInformation(f.friendPlayerID, out Owner))
            {
                //Owner.~~;
            }
        }
        //合成
       /* int thinginA = 0; //0沒東西 1,2,3物品
        int placeA = 0;
        UserManager.Instance.User.Player.OperationManager.SynthesizeMaterial(new Blueprint.ElementInfo[]
        {
            
            new Blueprint.ElementInfo { itemID = thinginA , positionIndex = placeA, itemCount= 1 },
            new Blueprint.ElementInfo { itemID = thinginA , positionIndex = placeA, itemCount= 1 },
            new Blueprint.ElementInfo { itemID = thinginA , positionIndex = placeA, itemCount= 1 }
        });*/
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
        //抽素材
        /*UserManager.Instance.User.Player.OperationManager.DrawMaterial();
        UserManager.Instance.User.Player.ResponseManager.OnDrawMaterialResponse += (returnCode, item, count) => 
        {
            switch(returnCode)
            {
                case IsolatedIslandGame.Protocol.ErrorCode.NoError:
                    break;
            }
        };*/
      
    }
    void MoveList()
    {
        if(!ShowList)
        {
            NearBoatPanel.transform.localPosition = new Vector3(0,45,0);
            ShowList = true;
        }
        else if(ShowList)
        {
            NearBoatPanel.transform.localPosition = new Vector3(391, 45, 0);
            ShowList = false;
        }
    }
}
