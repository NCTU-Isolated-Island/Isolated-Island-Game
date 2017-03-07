using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    // Setting Variables
    [SerializeField]
	public float SwapPageTimeInterval;
    //
    // enum all UI pages
    public enum UIPageType
    {
        Login, Main, OtherBoat, Inventory, Combine, Mission, Friend, Chat_Message, Chat_Record, Transaction, BluePrint, PutItem, IsolatedIsland
    }

    public GameObject[] UIPageList = new GameObject[10];
    public Stack<UIPageType> PageStack = new Stack<UIPageType>();

    // DEBUG
    void OnGUI()
    {
        //GUI.Label(new Rect(50, 50, 100, 100), (1 / Time.deltaTime).ToString());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SwapPage(UIPageType.IsolatedIsland);
    }


    // Variables for creating character
    private int CampNum;
    [SerializeField]
    private UserInformPanel userInformPanelPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initial Variable Setting
        PageStack.Push(UIPageType.Login);
        // Get GameObjects for the Dictionary
        UIPageList[(int)UIPageType.Login] = GameObject.Find("UI/LogIn");

        UserManager.Instance.User.OnUserInform += RenderUserInform;

        SwapPageTimeInterval = 0.5f;
        //SwapPageTimeInterval = 0.000001f;
    }

    public void SwapPage(UIPageType nextPage)
    {
        if (nextPage == PageStack.Peek()) return;
        //
        if (PageStack.Count > 1)
            UIPageList[(int)PageStack.Peek()].SetActive(false);
        if (nextPage == UIPageType.Chat_Record)
        {
            ChatUIManager.Instance.LoadChatRecord();
        }
        //
        if (nextPage == UIPageType.Inventory)
        {
            switch (PageStack.Peek())
            {
                case UIPageType.Combine:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.PutInCombineSlot);
                    break;
                case UIPageType.Transaction:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.PutInTransactionSlot);
                    break;
                case UIPageType.PutItem:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.PutItemOnVessel);
                    break;
                case UIPageType.OtherBoat:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.Donation);
                    break;
                case UIPageType.IsolatedIsland:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.ThrowMaterialToIsland);
                    break;
                default:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.CheckInventoryItemDetail);
                    break;
            }
        }
        if (PageStack.Peek() != UIPageType.Login)
            StartCoroutine(MovingPageToCenter(UIPageList[(int)nextPage]));

        PageStack.Push(nextPage);

        //RemoveCurrentPage();
    }

    public void RemoveCurrentPage()
    {
        // Remove Inventory -> use InventoryPanel.ClosePanel()
        UIPageType tmp = PageStack.Pop();
        //
        UIPageList[(int)PageStack.Peek()].SetActive(true);
        if (PageStack.Peek() == UIPageType.Chat_Record)
        {
            ChatUIManager.Instance.LoadChatRecord();
        }
        //
        StartCoroutine(RemovingPage(UIPageList[(int)tmp]));
    }

    public void ToPreviousPage()
    {
        //SwapPage(previosUIPage);
        RemoveCurrentPage();
    }

    IEnumerator MovingPageToCenter(GameObject page)
    {
        if (page == null)
            yield break;
        //print("Swapping " + page.name);
        page.SetActive(true);

        float passTime = 0;
        RectTransform rectTransform = page.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -transform.root.GetComponent<RectTransform>().rect.height);

        while (passTime < SwapPageTimeInterval)
        {
            passTime += Time.deltaTime;

            Vector2 nextPosition = rectTransform.anchoredPosition;
            nextPosition.y = Mathf.Lerp(-transform.root.GetComponent<RectTransform>().rect.height, 0, passTime / SwapPageTimeInterval);
            rectTransform.anchoredPosition = nextPosition;

            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        yield return null;
    }
    IEnumerator RemovingPage(GameObject page)
    {
        if (page == null)
            yield break;
        //print("Removing " + page.name);
        float passTime = 0;
        RectTransform rectTransform = page.GetComponent<RectTransform>();
        float targetY = rectTransform.anchoredPosition.y;

        while (passTime < SwapPageTimeInterval)
        {
            passTime += Time.deltaTime;

            Vector2 nextPosition = rectTransform.anchoredPosition;
            nextPosition.y = Mathf.Lerp(targetY, -transform.root.GetComponent<RectTransform>().rect.height, passTime / SwapPageTimeInterval);
            rectTransform.anchoredPosition = nextPosition;

            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -transform.root.GetComponent<RectTransform>().rect.height);

        page.SetActive(false);
        yield return null;
    }

    public void ToMainPage()
    {
        while (PageStack.Count > 2)
            RemoveCurrentPage();
    }

    private void RenderUserInform(string title, string content)
    {
        UserInformPanel panel = Instantiate(userInformPanelPrefab);
        panel.RenderUserInform(title, content);
        panel.transform.SetParent(transform);
        panel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        panel.GetComponent<RectTransform>().localScale = new Vector3(0.5213f, 0.5213f, 0.5213f);
    }
}