using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private GameObject confirmItemPanel;
    //
    private bool expandStatus;

    private float ver_ori;
    private IEnumerator coroutine;

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
        Vector3 verTmp = interactionButtonsContent.GetComponent<RectTransform>().offsetMax;
        ver_ori = verTmp.y;
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
        });
    }

    public void OpenScoreBoardPage()
    {
        GameObject newPage = Instantiate(scoreBoardPage);
        AdjustPageStatus(newPage);
    }

    public void OpenFriendScoreBoardPage()
    {
        GameObject newPage = Instantiate(friendScoreBoardPage);
        AdjustPageStatus(newPage);
    }

    public void OpenThrowMaterialPage()
    {
        //TODO
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
        confrimPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 30, 0);
    }

    private void ThrowMaterialToIsland(Item item)
    {
        // call Throw Item API;
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

}