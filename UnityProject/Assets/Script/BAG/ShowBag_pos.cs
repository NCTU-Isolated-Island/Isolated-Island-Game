using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
public class ShowBag_pos : MonoBehaviour
{

    public bool StopForTest;

    public GameObject UIControl;
    public GameObject canvas;

    public GameObject ShowBagPanel;

    public GameObject ShowWay_Button;
    public Button BackButton;
    public GameObject MainBoat;
    public GameObject CombineArea;

    //public GameObject ItemPic;

    public GameObject Content;
    public GameObject ItemSet;
    public float A_pos;
    float B_pos;
    int ChangeViewControl;

    public bool BagOut, MoveBag;
    bool ResetOnce = true;
    float passtime = 0;
    int times = 0;

    public Button ListButton;
    public GameObject FunctionList;

    // Use this for initialization
    void Start()
    {
        SetGameObject();
        StopForTest = UIControl.GetComponent<UImanager>().StopForTest;
        A_pos = -canvas.GetComponent<RectTransform>().rect.height;
        B_pos = 0;
        ChangeViewControl = 0;
        this.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0);
    }
   public void ChangeView()//改變排列順序
    {
        int Total = Content.transform.childCount;
        if (ChangeViewControl == 0)//照位置
        {
            Debug.Log("照位置排列");
            for(int i = Total-1;i>0;i-- )
            {
                for(int t = Total-1;t>0; t--)
                {
                    GameObject A = Content.transform.GetChild(t).gameObject;
                    GameObject B = Content.transform.GetChild(t-1).gameObject;
                    if(A.GetComponent<ShowBag_ItemSelect>().PositionIndex < B.GetComponent<ShowBag_ItemSelect>().PositionIndex)
                    {
                        A.transform.SetSiblingIndex(t-1);
                    }
                }
            }
            ChangeViewControl = 1;
        }
       else if(ChangeViewControl == 1)//照ID
        {
            Debug.Log("照ID排列");
            for (int i = Total - 1; i > 0; i--)
            {
                for (int t = Total - 1; t > 0; t--)
                {
                    GameObject A = Content.transform.GetChild(t).gameObject;
                    GameObject B = Content.transform.GetChild(t - 1).gameObject;
                    if (A.GetComponent<ShowBag_ItemSelect>().ItemNo < B.GetComponent<ShowBag_ItemSelect>().ItemNo)
                    {
                        A.transform.SetSiblingIndex(t - 1);
                    }
                }
            }
            ChangeViewControl = 2;
        }
       else if(ChangeViewControl == 2)//照最愛
        {
            Debug.Log("照ID+最愛排列");
            for (int i = Total; i > 0; i--)
            {
                for (int t = Total -1 ; t > 0; t--)
                {
                    GameObject A = Content.transform.GetChild(t).gameObject;
                    GameObject B = Content.transform.GetChild(t - 1).gameObject;
                    if(A.GetComponent<ShowBag_ItemSelect>().FavoriteBool == true && B.GetComponent<ShowBag_ItemSelect>().FavoriteBool == false)
                    {
                     
                        A.transform.SetSiblingIndex(t-1);
                    }
                    if (B.GetComponent<ShowBag_ItemSelect>().FavoriteBool == true && A.GetComponent<ShowBag_ItemSelect>().FavoriteBool == false)
                    {

                    }
                    else
                    {
                        if (A.GetComponent<ShowBag_ItemSelect>().ItemNo < B.GetComponent<ShowBag_ItemSelect>().ItemNo)
                        {
                           
                            A.transform.SetSiblingIndex(t - 1);
                        }
                    }                  
                }
            }
            ChangeViewControl = 0;
        }
            


    }
    void Update()
    {


        if (MoveBag)
        {
            passtime += Time.deltaTime;
            if (passtime > 0.05)
            {
                if (ResetOnce)
                {
                    if (!StopForTest)
                    { ChangeViewControl = 0; Reset_Bag(); }
                    ResetOnce = false;
                }
                if (BagOut)
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition + new Vector3(0, A_pos / 10, 0);
                else
                    this.gameObject.transform.localPosition = this.gameObject.transform.localPosition - new Vector3(0, A_pos / 10, 0);
                if (times < 9)
                { times++; }
                else
                { times = 0; MoveBag = false; BagOut = !BagOut; ResetOnce = true; }
            }

        }

    }
    public void Reset_Bag()
    {
        for (int i = 0, t = Content.transform.childCount - 1; t >= 0 && i < 30; i++, t--)
        {
            Destroy(Content.transform.GetChild(t).gameObject);
        }
        foreach (InventoryItemInfo item in UserManager.Instance.User.Player.Inventory.ItemInfos)
        {
            GameObject Create = Instantiate(ItemSet, Content.transform);
            Create.GetComponent<ShowBag_ItemSelect>().ItemNo = item.Item.ItemID;
           
               if(Resources.Load("2D/" + item.Item.ItemID) != null)
                {
                    Debug.Log(1230);               
                    Create.GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/" + item.Item.ItemID);
            }
            
            Create.GetComponent<ShowBag_ItemSelect>().inventoryItemInfoID = item.InventoryItemInfoID;
            Create.GetComponent<ShowBag_ItemSelect>().PositionIndex = item.PositionIndex;
            Create.GetComponent<ShowBag_ItemSelect>().FavoriteBool = item.IsFavorite;
            Create.GetComponent<ShowBag_ItemSelect>().ItemName = item.Item.ItemName;
            Create.GetComponent<ShowBag_ItemSelect>().ItemDescribe = item.Item.Description;
            Create.transform.GetChild(0).GetComponent<Text>().text = item.Item.ItemName;
        }
        ChangeView();
    }
    public void UpAndDownDirect()
    {
        if (BagOut)
            this.gameObject.transform.localPosition = new Vector3(0, A_pos, 0);
        else
            this.gameObject.transform.localPosition = new Vector3(0, B_pos, 0);
        BagOut = !BagOut;
    }
    public void UpAndDown()
    {
        MoveBag = true;
        /* if (Mathf.Abs(this.GetComponent<RectTransform>().localPosition.y - A_pos) < 1)
         {
             this.GetComponent<RectTransform>().localPosition = new Vector3(0, B_pos, 0);
             { Reset_Bag(); }
             ResetOnce = false;
         }
         else if (Mathf.Abs(this.GetComponent<RectTransform>().localPosition.y - B_pos) < 1)
         {this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0);}*/
    }

    public void SetPicture(GameObject ItemSelect)
    {
        if (CombineArea.GetComponent<Combine_block>().ItemInHere != null)
        { CombineArea.GetComponent<Combine_block>().ItemInHere.GetComponent<ShowBag_ItemSelect>().CombineAreaILocate = null; }

        CombineArea.GetComponent<Combine_block>().ItemInHere = ItemSelect;
        CombineArea.transform.GetChild(0).GetComponent<Image>().sprite = ItemSelect.GetComponent<Image>().sprite as Sprite;
    }

    void BACK()
    {
        if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Show_Bag)
        {
            // UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
            UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
            UpAndDown();
        }
        else if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Combine)
            UpAndDown();
    }
    void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        UImanager UI = UIControl.GetComponent<UImanager>();

        if (!canvas)
            canvas = UI.Canvas;
        if (!ShowBagPanel)
            ShowBagPanel = UI.UIObject[4];
        // if (!BagContent)
        //  BagContent = this.gameObject.transform.GetChild(3).gameObject;
        if (!BackButton)
            BackButton = this.gameObject.transform.GetChild(4).GetComponent<Button>();
        BackButton.onClick.AddListener(BACK);
        if (!ShowWay_Button)
            ShowWay_Button = this.gameObject.transform.GetChild(3).gameObject;
        if (!MainBoat)
            MainBoat = UI.UIObject[1];
        if (!CombineArea)
            CombineArea = UI.UIObject[5];
        if (!Content)
            Content = this.gameObject.transform.GetChild(3).GetChild(0).GetChild(0).gameObject;
        ResetOnce = true;
        /*if (!ItemPic)
            ItemPic = UI.ItemPic;*/

        /*UserManager.Instance.User.Player.Inventory.OnItemInfoChange += (changeType, info) =>
        {
            switch (changeType)
            {
                case DataChangeType.Add://有一格空的被填入東西了
                    Reset_Bag();               //使用info來調整UI
                    break;
                case DataChangeType.Remove://有一格東西變空了
                    Reset_Bag();                     //使用info來調整UI
                    break;
                case DataChangeType.Update://更新一格內的資訊
                    Reset_Bag();                      //使用info來調整UI
                    break;
            }
        };*/

        if (!ListButton)
            ListButton = this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        ListButton.onClick.AddListener(ShowList);
        if (!FunctionList)
            FunctionList = UIControl.GetComponent<UImanager>().FunctionList;
    }
    void ShowList()
    {
        FunctionList.GetComponent<FunctionList_Control>().ShowList();
    }
}
