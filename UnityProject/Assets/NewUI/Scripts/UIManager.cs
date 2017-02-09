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
    public GameObject[] UIPageList = new GameObject [10];

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

    public void SwapPage(UIPageType nextPage
        , InventoryPanel.InventoryUsageType usageType = InventoryPanel.InventoryUsageType.CheckInventoryItemDetail)
    {
        if (nextPage == currentUIPage) return;

        StartCoroutine(MovingPageToCenter(UIPageList[(int)nextPage]));

        if (nextPage == UIPageType.Inventory)
            InventoryPanel.Instance.ShowPanel(usageType);

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
        if (page == null) yield break;

        print(page.name);
        page.SetActive(true);

        // Move page to the center
        #region need Implement

        float passTime = 0;
        RectTransform rectTransform = page.GetComponent<RectTransform>();
        //// base on "right" property
        //while (rectTransform.offsetMax.y < 1)
        //{
        //    Vector2 nextPosition = rectTransform.offsetMax;
        //    nextPosition.y = Mathf.Lerp(0 , 1 , passTime / 1f );
        //    rectTransform.offsetMax = nextPosition;

        //    passTime += Time.deltaTime;
        //    yield return null;
        //}

        #endregion
        //rectTransform.offsetMax = new Vector2(1, rectTransform.offsetMax.y);
        yield return null;
    }
    IEnumerator RemovingPage(GameObject page)
    {
        if (page == null) yield break;
        print("Removing " + page.name);
        page.SetActive(false);
        //float passTime = 0;
        //RectTransform rectTransform = page.GetComponent<RectTransform>();
        // base on "right" property
        // Remove page from the center
        #region need Implement
        //while (rectTransform.offsetMax.y > 0)
        //{
        //    Vector2 nextPosition = rectTransform.offsetMax;
        //    nextPosition.y = Mathf.Lerp(1, 0, passTime / 1f);
        //    rectTransform.offsetMax = nextPosition;

        //    passTime += Time.deltaTime;
        //    yield return null;
        //}
        #endregion
        //rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
        yield return null;
    }

    public void ToMainPage()
    {
        SwapPage(UIPageType.Main);
    }

    // TESTING
    /*
    public void Test()
    {
        print("inTest");
        SwapPage(UIPageType.Login);
    }
    */
}
