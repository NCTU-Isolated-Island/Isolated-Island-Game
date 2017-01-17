using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
public class ShowBag_pos : MonoBehaviour {
    public GameObject UIControl;
    public GameObject canvas;

    public GameObject ShowBagPanel;
    public GameObject BAG;

    public GameObject ShowWay_Button;
    public Button BackButton;
    public GameObject MainBoat;
    public GameObject CombineArea;

    public GameObject Content;
    public GameObject ItemSet;
    float A_pos;
    float B_pos;
    // Use this for initialization
    void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        A_pos = -canvas.GetComponent<RectTransform>().rect.height;
        B_pos = 0;
        BackButton = this.gameObject.transform.GetChild(4).GetComponent<Button>();
        BackButton.onClick.AddListener(BACK);
    }
   public bool BackOnce = false;
	// Update is called once per frame
	void Update () {
        if (ShowBagPanel.activeInHierarchy == false)
        {
            if (!BackOnce)
            {
                this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0); 

                BackOnce = true;
            }
        }
        else if (ShowBagPanel.activeInHierarchy == true)
        {
            this.GetComponent<RectTransform>().localPosition = new Vector3(0, B_pos, 0);
            if(BackOnce)
            {
                Reset_Bag();
            }
            BackOnce = false;
        }
      
    }
    public void Reset_Bag()
    {
        /* foreach (InventoryItemInfo item in UserManager.Instance.User.Player.Inventory.ItemInfos)
            {
                GameObject Create = Instantiate(ItemSet, Content.transform);
            if (item.Item.ItemID != 0)
                Create.GetComponent<ShowBag_ItemSelect>().ItemNo = item.Item.ItemID;
            if (item.Item.ItemName != null)
                Create.transform.GetChild(0).GetComponent<Text>().text = item.Item.ItemName;
            }*/
    }
    public void UpAndDown()
    {              
        if (Mathf.Abs(this.GetComponent<RectTransform>().localPosition.y - A_pos) < 1)
        {this.GetComponent<RectTransform>().localPosition = new Vector3(0, B_pos, 0);}
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
    {if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Show_Bag)
            UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
     else if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Combine)
            UpAndDown();
    }
}
