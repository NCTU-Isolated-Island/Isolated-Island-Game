using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShowBag_ItemSelect : MonoBehaviour, IPointerClickHandler
{
    public bool Null = false;

    public GameObject UIControl;
    public GameObject DetailPanel;
    public GameObject Bag;
    public GameObject CombineAreaILocate;
    public int ItemNo;
    public int PositionIndex;
    public int inventoryItemInfoID;
    public string ItemName;
    public string ItemDescribe;
    public int Amount;
    public bool FavoriteBool;
    //trade相關
    public int LocateArea;
    public int UsedAmount;
    public GameObject AmountChosePanel;
    public int AmountOnPanel;
    bool AmountChanging = false;

    // Use this for initialization
    void Awake()
    {
        SetGameObject();

    }
    public void SetGameObject()
    {
        if (!UIControl)
            UIControl = GameObject.FindWithTag("UImanager");
        if (!Bag)
        { Bag = UIControl.GetComponent<UImanager>().Bag; }
        if (!DetailPanel)
            DetailPanel = Bag.transform.parent.GetChild(1).gameObject;
        if(!AmountChosePanel)
            AmountChosePanel = Bag.transform.GetChild(5).gameObject;
        AmountChosePanel.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(Right);
        AmountChosePanel.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(Left);
        AmountOnPanel = 1;
        AmountChosePanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Confirm);
        AmountChosePanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(Cancel);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Show_Bag)
        { Show_Choose(); }
        else if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Combine)
        { Combine_Choose(); }
        else if (UIControl.GetComponent<UImanager>().GameUI == UImanager.UI.Trade)
        { Trade_Choose(); }
    }
    void Show_Choose()
    {
        DetailPanel.SetActive(true);
        DetailPanel.GetComponent<ShowBag_DetailPanel>().ItemCallMe = this.gameObject;
        DetailPanel.GetComponent<ShowBag_DetailPanel>().FavoriteBool = FavoriteBool;
        DetailPanel.GetComponent<ShowBag_DetailPanel>().inventoryItemInfoID = inventoryItemInfoID;
        DetailPanel.GetComponent<ShowBag_DetailPanel>().Text.GetComponent<Text>().text = ItemName;
        DetailPanel.GetComponent<ShowBag_DetailPanel>().Text.transform.GetChild(0).GetComponent<Text>().text = ItemDescribe;
        DetailPanel.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/" + ItemNo);
    }
    void Combine_Choose()
    {
        if (this.gameObject.GetComponent<Image>().sprite != null)
        {
            if (CombineAreaILocate != null)
            {
                CombineAreaILocate.transform.GetChild(0).GetComponent<Image>().sprite = null;
                CombineAreaILocate.GetComponent<Combine_block>().ItemInHere = null;
            }
            CombineAreaILocate = Bag.GetComponent<ShowBag_pos>().CombineArea;
            Bag.GetComponent<ShowBag_pos>().SetPicture(this.gameObject);
            Bag.GetComponent<ShowBag_pos>().UpAndDown();
        }
    }

    //以下Trade相關
    void Trade_Choose()
    {if (!Null)
        {
            AmountChosePanel.SetActive(true);
            AmountChosePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (AmountOnPanel.ToString());
            AmountChanging = true;
        }
    else
        {
            ShowBag_pos.Instance.TradeArea.GetComponent<Trade_Block>().ItemInHere = 0;
            ShowBag_pos.Instance.TradeArea.GetComponent<Trade_Block>().AmountInHere = 0;
            ShowBag_pos.Instance.TradeArea.transform.GetChild(0).GetComponent<Text>().text = ("");
            ShowBag_pos.Instance.TradeArea.GetComponent<Image>().sprite = null;
            ShowBag_pos.Instance.TradeArea = null;
            ShowBag_pos.Instance.UpAndDown();
        }
    }
    void Update()
    {
        if (AmountChanging)
        {
            if (AmountOnPanel >= Amount)
            { AmountChosePanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(false); }
            else if (!AmountChosePanel.transform.GetChild(1).GetChild(1).gameObject.activeInHierarchy)
            {
                AmountChosePanel.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            }

            if (AmountOnPanel <= 1)
            { AmountChosePanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(false); }
            else if (!AmountChosePanel.transform.GetChild(1).GetChild(2).gameObject.activeInHierarchy)
            {
                AmountChosePanel.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            }
        }
      
    }
    void Right()
    {
        AmountOnPanel++;
        AmountChosePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (AmountOnPanel.ToString());
    }
    void Left()
    {
        AmountOnPanel--;
        AmountChosePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (AmountOnPanel.ToString());
    }
    void Confirm()
    {
        if (AmountChanging)
        {
            ShowBag_pos.Instance.TradeArea.GetComponent<Trade_Block>().ItemInHere = ItemNo;
            ShowBag_pos.Instance.TradeArea.GetComponent<Trade_Block>().AmountInHere = AmountOnPanel;
            ShowBag_pos.Instance.TradeArea.transform.GetChild(0).GetComponent<Text>().text = (AmountOnPanel.ToString());
            ShowBag_pos.Instance.TradeArea.GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/" + ItemNo);
            LocateArea = ShowBag_pos.Instance.TradeArea.GetComponent<Trade_Block>().Position;
        }
       
        
        AmountChanging = false;
        AmountChosePanel.SetActive(false);
        ShowBag_pos.Instance.UpAndDown();
       
    }
    void Cancel()
    {
        AmountOnPanel = 1;
        AmountChanging = false;
        AmountChosePanel.SetActive(false);
    }
}
