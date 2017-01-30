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

	public class TransactionItemSlot
	{
		public Image itemImage;
		public Item item;
		public Text amount;
	}

	// UI Variable
	[SerializeField]
	private Button requestTransactionBtn;
	[SerializeField]
	private TransactionItemSlot[] MyTransactionItem;
	[SerializeField]
	private TransactionItemSlot[] OpponentTransactionItem;

	// Custom needed variable
	private Dictionary<int, Sprite> ID2ImageDict;

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

		for (int i=0;i<4;i++)
		{
			MyTransactionItem[i] = new TransactionItemSlot();
			MyTransactionItem[i].itemImage = GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).GetComponent<Image>();
			MyTransactionItem[i].amount = GameObject.Find("MyTransactionItem/Viewport/Content/MyItem" + i.ToString()).transform.FindChild("Amount").GetComponent<Text>();

			OpponentTransactionItem[i] = new TransactionItemSlot();
			OpponentTransactionItem[i].itemImage = GameObject.Find("OpponentTransactionItem/Viewport/Content/Item" + i.ToString()).GetComponent<Image>();
			OpponentTransactionItem[i].amount = GameObject.Find("OpponentTransactionItem/Viewport/Content/Item" + i.ToString()).transform.FindChild("Amount").GetComponent<Text>();
		}

		// testing , should be removed
		ID2ImageDict = new Dictionary<int, Sprite>();
	}

	void OnPlayerOnline(Player player)
	{
		// Event Register
		UserManager.Instance.User.Player.OnTransactionRequest += OnReceiveTransactionRequest;
		UserManager.Instance.User.Player.OnTransactionStart += OnTransactionStart;

		// UI Listener Register
		//requestTransactionBtn.onClick.AddListener(delegate { UserManager.Instance.User.Player.OperationManager.TransactionRequest(accepterPlayerID); });
	}

	void OnDestroy()
	{
		UserManager.Instance.User.Player.OnTransactionRequest -= OnReceiveTransactionRequest;
		UserManager.Instance.User.Player.OnTransactionStart -= OnTransactionStart;
	}

	#endregion

	// Server Service API

	void SendTransactionRequest(int accepterPlayerID)
	{
		UserManager.Instance.User.Player.OperationManager.TransactionRequest(accepterPlayerID);
		//TODO
	}

	void OnReceiveTransactionRequest(int requesterPlayerID)
	{
		//TODO
		// Pop Receive Transaction UI , ask user to accept or not
	}

	void AcceptTransaction(int requesterPlayerID)
	{
		UserManager.Instance.User.Player.OperationManager.AcceptTransaction(requesterPlayerID);
		//TODO
	}

	void OnTransactionStart(Transaction transaction)
	{
		// Event Register
		transaction.OnTransactionItemChange += OnTransactionItemChange;
		transaction.OnTransactionConfirmed += OnTransactionConfirmed;
		transaction.OnTransactionEnd += OnTransactionEnd;

		//TODO
		// Pop Transaction UI
	}

	void ChangeTransactionItem(int transactionID, DataChangeType changeType, TransactionItemInfo info)
	{
		UserManager.Instance.User.Player.OperationManager.ChangeTransactionItem(transactionID, changeType, info);
		//TODO
		// Change the image of the item
	}

	void OnTransactionItemChange(int transactionID, int playerID, DataChangeType changeType, TransactionItemInfo info)
	{
		//TODO
		// Change the image of the OpponentItem
	}

	void ConfirmTransaction(int transactionID)
	{
		UserManager.Instance.User.Player.OperationManager.ConfirmTransaction(transactionID);
		//TODO
		// Pop A Waiting UI , wait for opponent to confirm
		// If confirmed , Pop a UI indicating transaction complete
		// If not , Pop a UI indicating transaction cancelled
	}

	void OnTransactionConfirmed(int transactionID, int playerID)
	{
		//TODO
		// Pop a UI indicating Transaction done
	}

	void OnTransactionEnd(int transactionID, bool isSuccessful)
	{
		//TODO
	}

	// UI API

	void OnPutInItem(InventoryItemInfo inventoryItem, TransactionItemSlot itemSlot)
	{
		itemSlot.itemImage.sprite = ID2ImageDict[inventoryItem.Item.ItemID];
		itemSlot.item = inventoryItem.Item;
		itemSlot.amount.text = inventoryItem.Count.ToString();
	}

	void OnRemoveItemFromSlot(TransactionItemSlot itemSlot)
	{
		itemSlot.item = null;
		itemSlot.itemImage.sprite = null;
		itemSlot.amount.text = "";
	}

}
