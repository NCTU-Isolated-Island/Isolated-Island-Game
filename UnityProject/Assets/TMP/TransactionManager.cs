using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransactionManager : MonoBehaviour
{

    // Basic Usage Variable
    public static TransactionManager Instance { get; private set; }
    public GameObject ConformPanel, CancelPanel;
    public bool ButtonActive = true;
    [System.Serializable]
    public class TransactionItemSlot
    {
        public Image itemImage;
        public Item item;
        public Text amount;

        public int Amount
        {
            get
            {
                return Amount;
            }
            set
            {
                amount.text = Amount.ToString();
                Amount = value;
            }
        }
    }

    // UI Variable
    [SerializeField]
    private TransactionItemSlot[] MyTransactionItem;
    [SerializeField]
    private TransactionItemSlot[] OpponentTransactionItem;
    [SerializeField]
    private GameObject ReceiveTransactionRequestPanel;
    [SerializeField]
    private GameObject MessagePanel;
    [SerializeField]
    private GameObject TransactionConfirmedPanel;

    // Custom needed variable
    private Dictionary<int, Sprite> ID2ImageDict;

    float width;

    #region Setup

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        //UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;

        // Get GameObject

        MyTransactionItem = new TransactionItemSlot[4];
        OpponentTransactionItem = new TransactionItemSlot[4];

        for (int i=0;i<4;i++)

        {
            MyTransactionItem[i] = new TransactionItemSlot();
            MyTransactionItem[i].itemImage = GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).GetComponent<Image>();
            MyTransactionItem[i].amount = GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).transform.FindChild("Amount").GetComponent<Text>();

            OpponentTransactionItem[i] = new TransactionItemSlot();
            OpponentTransactionItem[i].itemImage = GameObject.Find("OpponentTransactionItem/Viewport/Content/Item" + i.ToString()).GetComponent<Image>();
            OpponentTransactionItem[i].amount = GameObject.Find("OpponentTransactionItem/Viewport/Content/Item" + i.ToString()).transform.FindChild("Amount").GetComponent<Text>();


        }

        if (!ConformPanel)
            ConformPanel = this.transform.GetChild(8).gameObject;
        if (!CancelPanel)
            CancelPanel = this.transform.GetChild(6).gameObject;
        this.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(TradeCancel);
        this.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(TradeConfirm);
        ConformPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(TradeConfirmYes);
        ConformPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(TradeConfirmNo);
        CancelPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(TradeCancelYes);
        CancelPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(TradeCancelNo);
        width = (UImanager.Instance.Canvas.GetComponent<RectTransform>().rect.width) * 2;

        // testing , should be removed
        ID2ImageDict = new Dictionary<int, Sprite>();
    }

    void OnPlayerOnline(Player player)//<-學起來

    {
        // Event Register
        UserManager.Instance.User.Player.OnTransactionRequest += OnReceiveTransactionRequest;
        UserManager.Instance.User.Player.OnTransactionStart += OnTransactionStart;

        // UI Listener Register
    }
    void OnDestroy()
    {
        UserManager.Instance.User.Player.OnTransactionRequest -= OnReceiveTransactionRequest;
        UserManager.Instance.User.Player.OnTransactionStart -= OnTransactionStart;
    }
    #endregion
    // Server Service API
    public void SendTransactionRequest(int accepterPlayerID)
    {
        UserManager.Instance.User.Player.OperationManager.TransactionRequest(accepterPlayerID);
        //TODO
    }
    public void OnReceiveTransactionRequest(int requesterPlayerID)
    {
        //TODO
        // Pop Receive Transaction UI , ask user to accept or not
        StartCoroutine(OnReceiveTransactionRequest_PopUI(true)); // Pop the UI
    }
    IEnumerator OnReceiveTransactionRequest_PopUI(bool OnOff) // true to pop , false to move back
    {
        float pass_time = 0;
        Vector3 ori = ReceiveTransactionRequestPanel.GetComponent<RectTransform>().anchoredPosition;
        while (pass_time < 0.5f)
        {
            if (OnOff == true)
                ReceiveTransactionRequestPanel.GetComponent<RectTransform>().anchoredPosition =
                    Vector3.Lerp(ori, Vector3.zero, pass_time / 0.5f);
            else
                ReceiveTransactionRequestPanel.GetComponent<RectTransform>().anchoredPosition =
                    Vector3.Lerp(ori, new Vector3(0, 105, 0), pass_time / 0.5f);

            pass_time += Time.deltaTime;
            yield return null;
        }
    }
    public void AcceptTransaction(int requesterPlayerID)
    {
        //UserManager.Instance.User.Player.OperationManager.AcceptTransaction(requesterPlayerID);
        //TODO
        StartCoroutine(OnReceiveTransactionRequest_PopUI(false));
    }
    public void OnTransactionStart(Transaction transaction)
    {
        // Event Register
        transaction.OnTransactionItemChange += OnTransactionItemChange;
        transaction.OnTransactionConfirmed += OnTransactionConfirmed;
        transaction.OnTransactionEnd += OnTransactionEnd;

        //TODO
    }
    public void ChangeTransactionItem(int transactionID, DataChangeType changeType, TransactionItemInfo info)
    {
        UserManager.Instance.User.Player.OperationManager.ChangeTransactionItem(transactionID, changeType, info);
        //TODO
    }
    public void OnTransactionItemChange(int transactionID, int playerID, DataChangeType changeType, TransactionItemInfo info)
    {
        //TODO
        // Change the info of the OpponentItem
        int index = info.PositionIndex;
        OpponentTransactionItem[index].item = info.Item;
        OpponentTransactionItem[index].itemImage.sprite = ID2ImageDict[info.Item.ItemID];
        OpponentTransactionItem[index].Amount = info.Count;
    }
    public void ConfirmTransaction(int transactionID)
    {
        UserManager.Instance.User.Player.OperationManager.ConfirmTransaction(transactionID);
        //TODO
    }
    public void OnTransactionConfirmed(int transactionID, int playerID)
    {
        //TODO
        // Pop a UI indicating Transaction done
    }
    public void OnTransactionEnd(int transactionID, bool isSuccessful)
    {
        //TODO 
        // If confirmed , Pop a UI indicating transaction complete
        // If not , Pop a UI indicating transaction cancelled
        if (isSuccessful)
            MessagePanel.transform.FindChild("TransactionMessage").GetComponent<Text>().text = "交易成功";
        else
            MessagePanel.transform.FindChild("TransactionMessage").GetComponent<Text>().text = "交易取消";
        MessagePanel.SetActive(true);
    }
    // UI API
    public void OnPutInItem(TransactionItemInfo info, TransactionItemSlot itemSlot)
    {
        itemSlot.itemImage.sprite = ID2ImageDict[info.Item.ItemID];
        itemSlot.item = info.Item;
        itemSlot.Amount = info.Count;
    }
    public void OnRemoveItemFromSlot(TransactionItemSlot itemSlot)
    {
        itemSlot.item = null;
        itemSlot.itemImage.sprite = null;
        itemSlot.Amount = 0;
    }
    public void PopConfirmedPanel()
    {
        TransactionConfirmedPanel.SetActive(true);
    }
    void TradeConfirm()
    {
        if (ButtonActive)
        {
            ConformPanel.SetActive(true);
            ButtonActive = false;
        }

    }
    void TradeConfirmYes()  //輸出組合
    {
        CancelPanel.SetActive(false);
        ButtonActive = true;
    }
    void TradeConfirmNo()
    {
        ConformPanel.SetActive(false);
        ButtonActive = true;
    }
    void TradeCancel()
    {
        if (ButtonActive)
        {
            CancelPanel.SetActive(true);
            ButtonActive = false;
        }
    }
    void TradeCancelYes() //取消交易
    {
        CancelPanel.SetActive(false);
        ButtonActive = true;
        for(int i =0;i<4;i++)
        {
            MyTransactionItem[i].itemImage = null;
            MyTransactionItem[i].amount = null;

            OpponentTransactionItem[i].itemImage = null;
            OpponentTransactionItem[i].amount = null;

            GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).GetComponent<Trade_Block>().ItemInHere = 0;
            GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).GetComponent<Trade_Block>().AmountInHere = 0;
        }
        UImanager.Instance.ChangeUI(1);
        this.gameObject.transform.localPosition = new Vector3(width, 0, 0);
    }
    void TradeCancelNo()
    {

        CancelPanel.SetActive(false);
        ButtonActive = true;
    }
}
