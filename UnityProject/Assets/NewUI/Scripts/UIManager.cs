using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // enum all UI pages
    public enum UIPageType
    {
        Login, Main, OtherBoat, Inventory, Combine, Mission, Friend, Chat_Message, Chat_Record, Transaction, BluePrint, PutItem
    }

    //public Dictionary<UIPageType, GameObject> UIPageDictionary = new Dictionary<UIPageType, GameObject>();
    public GameObject[] UIPageList = new GameObject[10];

    public UIPageType previosUIPage;
    public UIPageType currentUIPage;

    // Variables for creating character
    private int CampNum;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initial Variable Setting
        previosUIPage = UIPageType.Login;
        currentUIPage = UIPageType.Login;

        // Get GameObjects for the Dictionary
        UIPageList[(int)UIPageType.Login] = GameObject.Find("UI/LogIn");
    }

    public void SwapPage(UIPageType nextPage)
    {
        if (nextPage == currentUIPage)
            return;

        if (currentUIPage != UIPageType.Login)
            StartCoroutine(MovingPageToCenter(UIPageList[(int)nextPage]));

        if (nextPage == UIPageType.Inventory)
        {
            switch (currentUIPage)
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
                default:
                    InventoryPanel.Instance.ShowPanel(InventoryPanel.InventoryUsageType.CheckInventoryItemDetail);
                    break;
            }
        }

        RemoveCurrentPage();

        // IMPORTANT - assign current page and previous page
        previosUIPage = currentUIPage;
        currentUIPage = nextPage;
        //
    }

    public void RemoveCurrentPage()
    {
        // Remove Inventory -> use InventoryPanel.ClosePanel()
        UIPageType tmp = currentUIPage;
        StartCoroutine(RemovingPage(UIPageList[(int)tmp]));
    }

    public void ToPreviousPage()
    {
        SwapPage(previosUIPage);
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

        while (passTime < 0.5f)
        {
            Vector2 nextPosition = rectTransform.anchoredPosition;
            nextPosition.y = Mathf.Lerp(-transform.root.GetComponent<RectTransform>().rect.height, 0, passTime / 0.5f);
            rectTransform.anchoredPosition = nextPosition;

            passTime += Time.deltaTime;
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

        while (passTime < 0.5f)
        {
            Vector2 nextPosition = rectTransform.anchoredPosition;
            nextPosition.y = Mathf.Lerp(targetY, -transform.root.GetComponent<RectTransform>().rect.height, passTime / 0.5f);
            rectTransform.anchoredPosition = nextPosition;

            passTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -transform.root.GetComponent<RectTransform>().rect.height);

        //page.SetActive(false);
        yield return null;
    }

    public void ToMainPage()
    {
        SwapPage(UIPageType.Main);
    }

    // TESTING
}
