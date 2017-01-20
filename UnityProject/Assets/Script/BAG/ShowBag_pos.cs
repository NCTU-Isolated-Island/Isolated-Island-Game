using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
public class ShowBag_pos : MonoBehaviour {

    public bool StopForTest;

    public GameObject UIControl;
    public GameObject canvas;

    public GameObject ShowBagPanel;
   // public GameObject BagContent;

    public GameObject ShowWay_Button;
    public Button BackButton;
    public GameObject MainBoat;
    public GameObject CombineArea;

    public GameObject Content;
    public GameObject ItemSet;
    public float A_pos;
    float B_pos;

    public bool BackOnce = false;

    // Use this for initialization
    void Start ()
    {
        SetGameObject();  
        A_pos = -canvas.GetComponent<RectTransform>().rect.height;
        B_pos = 0;
        this.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0);
    }

    void Update () {
        if (ShowBagPanel.activeInHierarchy == false)//showbag關閉
        {
            if (!BackOnce)
            {
                this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0); 

                BackOnce = true;
            }
        }
        else if (ShowBagPanel.activeInHierarchy == true  && this.gameObject.transform.localPosition.y == A_pos)//showbag開啟
        {
            UpAndDown();
            BackOnce = false;
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
                  Create.GetComponent<ShowBag_ItemSelect>().PositionIndex = item.PositionIndex;
                  Create.transform.GetChild(0).GetComponent<Text>().text = item.Item.ItemName;
             }

    }
    public void UpAndDown()
    {
        if (Mathf.Abs(this.GetComponent<RectTransform>().localPosition.y - A_pos) < 1)
        {
            this.GetComponent<RectTransform>().localPosition = new Vector3(0, B_pos, 0);
            if (!StopForTest)
            { Reset_Bag(); }
        }
        else if (Mathf.Abs(this.GetComponent<RectTransform>().localPosition.y - B_pos) < 1)
        {this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0);}
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
        //  BagContent = this.gameObject.transform.GetChild(2).gameObject;
        if (!BackButton)
            BackButton = this.gameObject.transform.GetChild(4).GetComponent<Button>();
        BackButton.onClick.AddListener(BACK);
        if (!MainBoat)
            MainBoat = UI.UIObject[1];
        if (!CombineArea)
            CombineArea = UI.UIObject[5];
        if (!Content)
            Content = this.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
    }
}
