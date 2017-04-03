using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IsolatedIslandGame.Protocol;
using UnityEngine.UI;

public class IsolatedIslandUIManager : MonoBehaviour
{
    public static IsolatedIslandUIManager Instance { get; private set; }

    // UI Variables
    [SerializeField]
    private GameObject expandButton;
    [SerializeField]
    private GameObject interactionButtonsContent;

    [SerializeField]
    private Button scoreBoardButton;
    [SerializeField]
    private Button friendScoreBoardButton;
    [SerializeField]
    private Button throwMaterialButton;

    [SerializeField]
    private GameObject scoreBoardPage;
    [SerializeField]
    private GameObject friendScoreBoardPage;
    [SerializeField]
    private GameObject materialScoreBoardPage;

    [SerializeField]
    private GameObject confirmItemPanel;
    //
    private bool expandStatus;

    private float ver_ori;
    private IEnumerator coroutine;

    void OnDisable()
    {
        try
        {
            AuxCameraSystem.Instance.UnShow();
            Destroy(FriendScoreManager.Instance.currentScoreBoardPage);
            Destroy(ScoreBoardManager.Instance.currentScoreBoardPage);
            Destroy(MaterialScoreBoardManager.Instance.currentScoreBoardPage);
        }
        catch
        {
        }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        gameObject.SetActive(false);
    }

    void InitialSetting()
    {
        expandStatus = false;
    }

    void Start()
    {
        Island.Instance.OnTotalScoreUpdated += OnTotalScoreUpdated;
        Vector3 verTmp = interactionButtonsContent.GetComponent<RectTransform>().offsetMax;
        ver_ori = verTmp.y;


		// Temp 
		throwMaterialButton.interactable = true;
		print(ClientFunctionCheckTable.Instance.IsFunctionOpened(ClientFunctionCode.Island));

    }

    private void AdjustPageStatus(GameObject page)
    {
        page.transform.SetParent(transform);
        page.GetComponent<RectTransform>().localScale = Vector3.one;
        page.GetComponent<RectTransform>().offsetMax = new Vector2(0, -50);
        page.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        page.transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            Destroy(page);
            ShowIsland();
        });
    }

    void OnEnable()
    {
        LoadIslandPieChart();
        transform.Find("BackButton").gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            AuxCameraSystem.Instance.UnShow();
        });
    }

    public void OpenScoreBoardPage()
    {
        AuxCameraSystem.Instance.UnShow();

        GameObject newPage = Instantiate(scoreBoardPage);
        AdjustPageStatus(newPage);
        ScoreBoardManager.Instance.TargetCurrentScoreBoardPage(newPage);
        ScoreBoardManager.Instance.LoadScoreBoard();
    }

    public void OpenFriendScoreBoardPage()
    {
        AuxCameraSystem.Instance.UnShow();

        GameObject newPage = Instantiate(friendScoreBoardPage);
        AdjustPageStatus(newPage);
        // Target to newPage
        FriendScoreManager.Instance.TargetCurrentScoreBoardPage(newPage);
        FriendScoreManager.Instance.LoadScoreBoard();
    }

    public void OpenMaterialScoreBoardPage()
    {
        AuxCameraSystem.Instance.UnShow();

        GameObject newPage = Instantiate(materialScoreBoardPage);
        AdjustPageStatus(newPage);
        MaterialScoreBoardManager.Instance.TargetCurrentScoreBoardPage(newPage);
        MaterialScoreBoardManager.Instance.LoadScoreBoard();
    }

    public void OpenThrowMaterialPage()
    {
        UIManager.Instance.SwapPage(UIManager.UIPageType.Inventory);
    }

    public void WaitForUserConfim(Item item)
    {
        GameObject confrimPanel = Instantiate(confirmItemPanel);

        confrimPanel.transform.SetParent(gameObject.transform);
        confrimPanel.transform.Find("Message").gameObject.GetComponent<Text>().text
            = string.Format("確定要將 {0} 投放至島上嗎? \n 一天只能進行一次投放喔!", item.ItemName);
        confrimPanel.transform.Find("Cancel").gameObject.GetComponent<Button>().onClick.AddListener(delegate { Destroy(confrimPanel); });
        confrimPanel.transform.Find("Confirm").gameObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            ThrowMaterialToIsland(item);
            Destroy(confrimPanel);
        });

        confrimPanel.transform.localScale = new Vector3(0.5213f, 0.5213f, 0.5213f);
        confrimPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -30, 0);
    }

    private void ThrowMaterialToIsland(Item item)
    {
        // call Throw Item API;
        AuxCameraSystem.Instance.UnShow();

        UserManager.Instance.User.Player.OperationManager.SendMaterialToIsland(item.ItemID);
        print("ThrowMaterialToIsland");
    }

    public void OnClickExpandButton()
    {
        if (expandStatus == false)
            ExpandInteractionButtons();
        else
            WithdrawInteractionButtons();
    }

    public void ExpandInteractionButtons()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(true);
        StartCoroutine(coroutine);
    }

    public void WithdrawInteractionButtons()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(false);
        StartCoroutine(coroutine);
    }

    IEnumerator ExpandBtnCoroutine(bool isOn)
    {
        float passTime = 0f;
        Vector3 verTmp = interactionButtonsContent.GetComponent<RectTransform>().offsetMax;

        float ver_st = verTmp.y;
        float intervalTime = 0.5f;

        while (passTime < intervalTime)
        {
            passTime += Time.deltaTime;

            float lerpAlpha = 0.1f * passTime / intervalTime + 0.9f * Mathf.Sqrt(1 - Mathf.Pow(passTime / intervalTime - 1, 2));
            if (isOn)
            {
                verTmp.y = Mathf.Lerp(ver_st, 0, lerpAlpha);
                interactionButtonsContent.GetComponent<RectTransform>().offsetMax = verTmp;
            }
            else
            {
                verTmp.y = Mathf.Lerp(ver_st, ver_ori, lerpAlpha);
                interactionButtonsContent.GetComponent<RectTransform>().offsetMax = verTmp;
            }

            yield return null;
        }
        expandStatus = isOn;
    }

    public void LoadIslandPieChart()
    {
        // TMP : r as animal , b as businessman , g as farmer
        
        ShowIsland();

        float r, g, b;
        float totalScore;
        r = Island.Instance.GetTotalScore(GroupType.Animal);
        b = Island.Instance.GetTotalScore(GroupType.Businessman);
        g = Island.Instance.GetTotalScore(GroupType.Farmer);
        
        totalScore = r + g + b;
        r = r / totalScore;
        g = g / totalScore;
        b = b / totalScore;

        Image redArea = transform.Find("PieChart/Pie_RedArea").GetComponent<Image>();
        Image greenArea = transform.Find("PieChart/Pie_GreenArea").GetComponent<Image>();
        Image blueArea = transform.Find("PieChart/Pie_BlueArea").GetComponent<Image>();
        redArea.fillAmount = r + g;
        greenArea.fillAmount = g;
        blueArea.fillAmount = b;
    }

    private void ShowIsland()
    {
        //AuxCameraSystem.Instance.ShowIsland ();
    }

    void OnTotalScoreUpdated(GroupType type, int score)
    {
        LoadIslandPieChart();
    }
}