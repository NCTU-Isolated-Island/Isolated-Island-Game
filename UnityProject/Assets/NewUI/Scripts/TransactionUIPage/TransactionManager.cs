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
                int result;
                int.TryParse(amount.text, out result);
                return result;
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

    private Transaction thisTransaction;
    public int WhereIfrom;

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
        UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;

        // Get GameObject
        MyTransactionItem = new TransactionItemSlot[4];
        OpponentTransactionItem = new TransactionItemSlot[4];

        for (int i = 0; i < 4; i++)
        {
            MyTransactionItem[i] = new TransactionItemSlot();
            MyTransactionItem[i].itemImage = GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).GetComponent<Image>();
            MyTransactionItem[i].amount = GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).transform.FindChild("Amount").GetComponent<Text>();

            OpponentTransactionItem[i] = new TransactionItemSlot();
            OpponentTransactionItem[i].itemImage = GameObject.Find("OpponentTransactionItem/Viewport/Content/Item" + i.ToString()).GetComponent<Image>();
            OpponentTransactionItem[i].amount = GameObject.Find("OpponentTransactionItem/Viewport/Content/Item" + i.ToString()).transform.FindChild("Amount").GetComponent<Text>();
        }

        // testing , should be removed
    }

    void OnPlayerOnline(Player player)
    {
        // Event Register
        UserManager.Instance.User.Player.OnTransactionRequest += OnReceiveTransactionRequest;
        UserManager.Instance.User.Player.OnTransactionStart += OnTransactionStart;

        // UI Listener Register
    }

    void OnDestroy()
    {
        UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
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
        UserManager.Instance.User.Player.OperationManager.AcceptTransaction(requesterPlayerID);
        //TODO
        StartCoroutine(OnReceiveTransactionRequest_PopUI(false));
    }

    public void OnTransactionStart(Transaction transaction)
    {
        // Ui Init Setting
        InitUISetting();

        // Event Register
        thisTransaction = transaction;
        thisTransaction.OnTransactionItemChange += OnTransactionItemChange;
        thisTransaction.OnTransactionConfirmStatusChange += OnTransactionConfirmStatusChange;
        thisTransaction.OnTransactionEnd += OnTransactionEnd;

        //TODO
        WhereIfrom = (int)UImanager.Instance.GameUI;
        UImanager.Instance.ChangeUI(9);
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
        if (UserManager.Instance.User.Player.PlayerID != playerID)
        {
            int index = info.PositionIndex;
            OpponentTransactionItem[index].item = info.Item;
            OpponentTransactionItem[index].itemImage.sprite = Resources.Load<Sprite>("2D/" + info.Item.ItemID);
            OpponentTransactionItem[index].Amount = info.Count;
        }
        else
        {
            int index = info.PositionIndex;
            MyTransactionItem[index].item = info.Item;
            MyTransactionItem[index].itemImage.sprite = Resources.Load<Sprite>("2D/" + info.Item.ItemID);
            MyTransactionItem[index].Amount = info.Count;
        }
    }

    public void ConfirmTransaction()
    {
        UserManager.Instance.User.Player.OperationManager.ChangeTransactionConfirmStatus(thisTransaction.TransactionID , true);
        //TODO
    }

    public void UnLockTransaction()
    {
        UserManager.Instance.User.Player.OperationManager.ChangeTransactionConfirmStatus(thisTransaction.TransactionID, false);
    }

    public void CancelTransaction()
    {
        UserManager.Instance.User.Player.OperationManager.CancelTransaction(thisTransaction.TransactionID);
    }

    public void OnTransactionConfirmStatusChange(int transactionID, int playerID , bool isConfirmed)
    {
        //TODO
        // Lock down put item in function
        print("IN");

        if(isConfirmed == true)
        {
            foreach (TransactionItemSlot slot in MyTransactionItem)
                LockTransactionItemSlot(slot);
            foreach (TransactionItemSlot slot in OpponentTransactionItem)
                LockTransactionItemSlot(slot);
        }
        else
        {
            foreach (TransactionItemSlot slot in MyTransactionItem)
                UnLockTransactionItemSlot(slot);
            foreach (TransactionItemSlot slot in OpponentTransactionItem)
                UnLockTransactionItemSlot(slot);
        }
    }

    private void LockTransactionItemSlot(TransactionItemSlot slot)
    {
        Color tmp = slot.itemImage.color;
        tmp.a = 0.5f;
        slot.itemImage.color = tmp;
    }

    private void UnLockTransactionItemSlot(TransactionItemSlot slot)
    {
        Color tmp = slot.itemImage.color;
        tmp.a = 1;
        slot.itemImage.color = tmp;
    }

    public void OnTransactionEnd(int transactionID, bool isSuccessful)
    {
        //TODO 
        // If confirmed , Pop a UI indicating transaction complete
        // If not , Pop a UI indicating transaction cancelled

        thisTransaction.OnTransactionItemChange -= OnTransactionItemChange;
        thisTransaction.OnTransactionConfirmStatusChange -= OnTransactionConfirmStatusChange;
        thisTransaction.OnTransactionEnd -= OnTransactionEnd;

        if (isSuccessful)
            MessagePanel.transform.FindChild("TransactionMessage").GetComponent<Text>().text = "交易成功";
        else
            MessagePanel.transform.FindChild("TransactionMessage").GetComponent<Text>().text = "交易取消";
        MessagePanel.SetActive(true);
        // move back to previous UI page
        UImanager.Instance.ChangeUI(WhereIfrom);
    }

    // UI API

    public void OnPutInItem(TransactionItemInfo info)
    {
        if (thisTransaction.IsLocked) return;

        if (MyTransactionItem[info.PositionIndex] == null)
            ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Add, info);
        else
            ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Update, info);
    }

    public void OnRemoveItemFromSlot(TransactionItemInfo info)
    {
        if (thisTransaction.IsLocked) return;

        ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Remove, info);
    }

    public void PopConfirmedPanel()
    {
        TransactionConfirmedPanel.SetActive(true);
    }

    // Setting

    private void InitUISetting()
    {
        foreach(TransactionItemSlot slot in MyTransactionItem)
        {
            slot.Amount = 0;
            slot.item = null;
            slot.itemImage = null;
        }
        foreach (TransactionItemSlot slot in OpponentTransactionItem)
        {
            slot.Amount = 0;
            slot.item = null;
            slot.itemImage = null;
        }
        thisTransaction = null;
    }

    // TESTING

    public void test()
    {
        OnTransactionConfirmStatusChange(0, 0, true);
    }

}
