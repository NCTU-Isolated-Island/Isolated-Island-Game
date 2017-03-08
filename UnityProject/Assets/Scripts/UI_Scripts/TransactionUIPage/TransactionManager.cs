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
		
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        TransactionUIPage.SetActive(false);
        ReceiveTransactionRequestPanel.SetActive(false);
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

	public void ShowOpponentVessel()
	{
		AuxCameraSystem.Instance.ShowPlayerGameObject(GameManager.Instance.UserGameObject[opponentPlayerID] , new Vector3(0,-5.98f,31.5f) , Quaternion.Euler(15f,0,0));

	}

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
    }

    public void SendTransactionRequest(int accepterPlayerID)
    {
        UserManager.Instance.User.Player.OperationManager.TransactionRequest(accepterPlayerID);
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
        
        ReceiveTransactionRequestPanel.SetActive(true);
    }

    public void AcceptTransaction()
    {
        UserManager.Instance.User.Player.OperationManager.AcceptTransaction(opponentPlayerID);
        ReceiveTransactionRequestPanel.SetActive(false);
    }

    public void OnTransactionStart(Transaction transaction)
    {
        // Event Register
        thisTransaction = transaction;
        thisTransaction.OnTransactionItemChange += OnTransactionItemChange;
        thisTransaction.OnTransactionConfirmStatusChange += OnTransactionConfirmStatusChange;
        thisTransaction.OnTransactionEnd += OnTransactionEnd;

        UIManager.Instance.SwapPage(UIManager.UIPageType.Transaction);

        if (transaction.AccepterPlayerID != UserManager.Instance.User.Player.PlayerID)
            opponentPlayerID = transaction.AccepterPlayerID;
        else
            opponentPlayerID = transaction.RequesterPlayerID;

        // Ui Init Setting
        InitialSetting();
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

            if (playerID == UserManager.Instance.User.Player.PlayerID)
                ConfirmTransactionButton.interactable = false;

            UnlockTransactionSlotButton.interactable = true;
        }
        else
        {
            foreach (TransactionSlotBehavior slot in MyTransactionItem)
                UnLockTransactionItemSlot(slot);
            foreach (TransactionSlotBehavior slot in OpponentTransactionItem)
                UnLockTransactionItemSlot(slot);

            if (playerID == UserManager.Instance.User.Player.PlayerID)
                ConfirmTransactionButton.interactable = true;

            UnlockTransactionSlotButton.interactable = false;
        }
    }

    private void LockTransactionItemSlot(TransactionSlotBehavior slot)
    {
        Color tmp = slot.itemImage.color;
        tmp.a = 0.5f;
        slot.itemImage.color = tmp;
        if (slot.gameObject.GetComponent<Button>() != null)
            slot.gameObject.GetComponent<Button>().interactable = false;
    }

    private void UnLockTransactionItemSlot(TransactionSlotBehavior slot)
    {
        Color tmp = slot.itemImage.color;
        tmp.a = 1;
        slot.itemImage.color = tmp;
        if (slot.gameObject.GetComponent<Button>() != null)
            slot.gameObject.GetComponent<Button>().interactable = true;
    }

    public void OnTransactionEnd(int transactionID, bool isSuccessful)
    {
        //TODO 
        // If confirmed , Pop a UI indicating transaction complete
        // If not , Pop a UI indicating transaction cancelled

        UIManager.Instance.ToMainPage();

        thisTransaction.OnTransactionItemChange -= OnTransactionItemChange;
        thisTransaction.OnTransactionConfirmStatusChange -= OnTransactionConfirmStatusChange;
        thisTransaction.OnTransactionEnd -= OnTransactionEnd;

        if (isSuccessful)
        {
            UserManager.Instance.User.UserInform("交易通知", "交易成功");
        }
        else
        {
            UserManager.Instance.User.UserInform("交易通知", "交易失敗");
        }
        //AuxCameraSystem.Instance.UnShow ();
    }

    // UI API

    public void UploadPutInItem(TransactionItemInfo info)
    {
        //if (thisTransaction.IsLocked) return;

        if (MyTransactionItem[info.PositionIndex].amount == 0)
            ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Add, info);
        else
            ChangeTransactionItem(thisTransaction.TransactionID, DataChangeType.Update, info);
    }

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
        foreach (TransactionSlotBehavior slot in MyTransactionItem)
            UnLockTransactionItemSlot(slot);
        foreach (TransactionSlotBehavior slot in OpponentTransactionItem)
            UnLockTransactionItemSlot(slot);

        ConfirmTransactionButton.interactable = true;
        UnlockTransactionSlotButton.interactable = false;
    }
    
}
