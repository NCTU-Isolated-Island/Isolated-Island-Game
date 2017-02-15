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

    // UI Variable
    public GameObject TransactionUIPage;

    [SerializeField]
    private TransactionSlotBehavior[] MyTransactionItem;
    [SerializeField]
    private TransactionSlotBehavior[] OpponentTransactionItem;
    [SerializeField]
    private GameObject ReceiveTransactionRequestPanel;
    [SerializeField]
    private GameObject MessagePanel;
    [SerializeField]
    private GameObject TransactionConfirmedPanel;
    [SerializeField]
    private GameObject ChooseAmountPanel;

    [SerializeField]
    private Button ConfirmTransactionButton;
    [SerializeField]
    private Button UnlockTransactionSlotButton;
    [SerializeField]
    private Button CancelTransactionButton;
    //

    public Transaction thisTransaction;

    public int puttingIndex;

    private int opponentPlayerID;

    //TEST
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //        UIManager.Instance.SwapPage(UIManager.UIPageType.Transaction);
    //}
    //

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //gameObject.SetActive(false);
    }

    void Start()
    {
        if (UserManager.Instance.User.IsOnline)
        {
            RegisterPlayerEvents(UserManager.Instance.User.Player);
        }
        else
        {
            UserManager.Instance.User.OnPlayerOnline += RegisterPlayerEvents;
        }
    }

    //

    public void PutInItemFromInventory(Item puttingItem)
    {
        ChooseAmountPanel.SetActive(true);
        
        MyTransactionItem[puttingIndex].SetSlotItem(puttingItem);
    }

    public void ConfirmItemAmount()
    {
        Dropdown dropdown = ChooseAmountPanel.transform.Find("Dropdown").GetComponent<Dropdown>();
        MyTransactionItem[puttingIndex].SetSlotAmount(dropdown.value + 1);
        ChooseAmountPanel.SetActive(false);
    }

    //

    void RegisterPlayerEvents(Player player)
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
        opponentPlayerID = requesterPlayerID;
        PlayerInformation opponentPlayer;
        PlayerInformationManager.Instance.FindPlayerInformation(requesterPlayerID, out opponentPlayer);
        ReceiveTransactionRequestPanel.transform.Find("Message").GetComponent<Text>().text
             = opponentPlayer.nickname + "發出交易請求";

        //StartCoroutine(OnReceiveTransactionRequest_PopUI(true)); // Pop the UI
        ReceiveTransactionRequestPanel.SetActive(true);
    }

    //IEnumerator OnReceiveTransactionRequest_PopUI(bool OnOff) // true to pop , false to move back
    //{
    //    float pass_time = 0;
    //    Vector3 ori = ReceiveTransactionRequestPanel.GetComponent<RectTransform>().anchoredPosition;
    //    while (pass_time < 0.5f)
    //    {
    //        if (OnOff == true)
    //            ReceiveTransactionRequestPanel.GetComponent<RectTransform>().anchoredPosition =
    //                Vector3.Lerp(ori, Vector3.zero, pass_time / 0.5f);
    //        else
    //            ReceiveTransactionRequestPanel.GetComponent<RectTransform>().anchoredPosition =
    //                Vector3.Lerp(ori, new Vector3(0, 105, 0), pass_time / 0.5f);

    //        pass_time += Time.deltaTime;
    //        yield return null;
    //    }
    //}

    public void AcceptTransaction()
    {
        UserManager.Instance.User.Player.OperationManager.AcceptTransaction(opponentPlayerID);
        //TODO
        //StartCoroutine(OnReceiveTransactionRequest_PopUI(false));
        ReceiveTransactionRequestPanel.SetActive(false);
    }

    public void OnTransactionStart(Transaction transaction)
    {
        // Swap to Transaction Page
        UIManager.Instance.SwapPage(UIManager.UIPageType.Transaction);

        // Ui Init Setting
        InitialSetting();

        // Event Register
        thisTransaction = transaction;
        thisTransaction.OnTransactionItemChange += OnTransactionItemChange;
        thisTransaction.OnTransactionConfirmStatusChange += OnTransactionConfirmStatusChange;
        thisTransaction.OnTransactionEnd += OnTransactionEnd;
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
            OpponentTransactionItem[index].amount = info.Count;
            OpponentTransactionItem[index].amountText.text = info.Count.ToString();
        }
        else
        {
            int index = info.PositionIndex;
            MyTransactionItem[index].item = info.Item;
            MyTransactionItem[index].itemImage.sprite = Resources.Load<Sprite>("2D/" + info.Item.ItemID);
            MyTransactionItem[index].amount = info.Count;
            MyTransactionItem[index].amountText.text = info.Count.ToString();
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
            foreach (TransactionSlotBehavior slot in MyTransactionItem)
                LockTransactionItemSlot(slot);
            foreach (TransactionSlotBehavior slot in OpponentTransactionItem)
                LockTransactionItemSlot(slot);
        }
        else
        {
            foreach (TransactionSlotBehavior slot in MyTransactionItem)
                UnLockTransactionItemSlot(slot);
            foreach (TransactionSlotBehavior slot in OpponentTransactionItem)
                UnLockTransactionItemSlot(slot);
        }
    }

    private void LockTransactionItemSlot(TransactionSlotBehavior slot)
    {
        //Color tmp = slot.itemImage.color;
        //tmp.a = 0.5f;
        //slot.itemImage.color = tmp;
        slot.gameObject.GetComponent<Button>().interactable = false;
    }

    private void UnLockTransactionItemSlot(TransactionSlotBehavior slot)
    {
        //Color tmp = slot.itemImage.color;
        //tmp.a = 1;
        //slot.itemImage.color = tmp;
        slot.gameObject.GetComponent<Button>().interactable = true;
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
        UIManager.Instance.ToPreviousPage();
    }

    // UI API

    public void UploadPutInItem(TransactionItemInfo info)
    {
        //if (thisTransaction.IsLocked) return;

        if (MyTransactionItem[info.PositionIndex].item == null)
            ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Add, info);
        else
            ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Update, info);
    }

    //public void UpLoadRemoveItem(TransactionItemInfo info)
    //{
    //    if (thisTransaction.IsLocked) return;

    //    ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Remove, info);
    //}

    //public void PopConfirmedPanel()
    //{
    //    TransactionConfirmedPanel.SetActive(true);
    //}

    // Setting

    private void InitialSetting()
    {
        foreach (TransactionSlotBehavior slot in MyTransactionItem)
        {
            slot.amount = 0;
            slot.item = null;
            slot.itemImage.sprite = null;
            slot.amountText.text = "";
        }
        foreach (TransactionSlotBehavior slot in OpponentTransactionItem)
        {
            slot.amount = 0;
            slot.item = null;
            slot.itemImage.sprite = null;
            slot.amountText.text = "";
        }
        thisTransaction = null;

        ConfirmTransactionButton.onClick.AddListener(delegate
        {
            ConfirmTransaction();
        });
        CancelTransactionButton.onClick.AddListener(delegate
        {
            CancelTransaction();
        });
        UnlockTransactionSlotButton.onClick.AddListener(delegate
        {
            UnLockTransaction();
        });
    }

    // TESTING

}
