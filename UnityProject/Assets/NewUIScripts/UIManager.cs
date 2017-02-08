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
        Login, Main_Boat, Other_Boat, Show_Bag, Combine, Mission, Friend, Chat, Transaction
            , BluePrint , PutItem
    }

    //public Dictionary<UIPageType, GameObject> UIPageDictionary = new Dictionary<UIPageType, GameObject>();
    public GameObject[] UIPageList = new GameObject [10];

    public UIPageType currentUIPage;

    // Variables for creating character
    private int CampNum;
    private string player_name, speech;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        // Initial Variable Setting
        currentUIPage = UIPageType.Main_Boat;

        // Get GameObjects for the Dictionary
        UIPageList[(int)UIPageType.Login] = GameObject.Find("UI/LogIn");

        // TESTING
        //if (UIPageList[(int)UIPageType.Login] == null) print("LogIn is NULL");
        //Invoke("Test", 3f);
    }

    public void SwapPage(UIPageType nextPage)
    {
        if (nextPage == currentUIPage) return;

        StartCoroutine(MovingPageToCenter(UIPageList[(int)nextPage]));
        UIPageType tmp = currentUIPage;
        StartCoroutine(RemovingPage(UIPageList[(int)tmp]));

        currentUIPage = nextPage;
    }

    public void RemoveCurrentPage()
    {
        StartCoroutine(RemovingPage(UIPageList[(int)currentUIPage]));
    }

    IEnumerator MovingPageToCenter(GameObject page)
    {
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
        float passTime = 0;
        RectTransform rectTransform = page.GetComponent<RectTransform>();
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
        page.SetActive(false);
        //rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
        yield return null;
    }

    // Update Legacy API from old UImanager

    public void LoadResult(int Result)
    {
        UIPageList[(int)UIPageType.Login].GetComponent<L_Loading>().LoadResult(Result);
        //UIPageDictionary[UIPageType.Login].GetComponent<L_Loading>().LoadResult(Result);
    }

    public void Create_Charter()
    {
        switch (CampNum)
        {
            case 0: UserManager.Instance.User.Player.OperationManager.CreateCharacter(player_name, speech, IsolatedIslandGame.Protocol.GroupType.Farmer); break;
            case 1: UserManager.Instance.User.Player.OperationManager.CreateCharacter(player_name, speech, IsolatedIslandGame.Protocol.GroupType.Businessman); break;
            case 2: UserManager.Instance.User.Player.OperationManager.CreateCharacter(player_name, speech, IsolatedIslandGame.Protocol.GroupType.Animal); break;
        }
    }

    public void ToMainPage()
    {
        SwapPage(UIPageType.Main_Boat);
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
