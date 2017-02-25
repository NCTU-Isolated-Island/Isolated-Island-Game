using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaterialScoreBoardManager : MonoBehaviour
{
    public static MaterialScoreBoardManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    [SerializeField]
    private GameObject currentScoreBoardPage;
    [SerializeField]
    private GameObject materialScoreSetContent;
    [SerializeField]
    private GameObject materialScoreSet;

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

    void RegisterPlayerEvents(Player player)
    {
        Island.Instance.OnPlayerScoreRankingUpdated += UpdateScoreBoard;
    }

    public void TargetCurrentScoreBoardPage(GameObject scoreBoardPage)
    {
        currentScoreBoardPage = scoreBoardPage;
        materialScoreSetContent = currentScoreBoardPage.transform.GetComponentInChildren<ContentSizeFitter>().gameObject;
    }

    public void LoadScoreBoard()
    {
        if (currentScoreBoardPage == null) return;

        foreach (Transform oldSet in materialScoreSetContent.transform)
        {
            Destroy(oldSet.gameObject);
        }

        foreach (Island.PlayerMaterialInfo info in Island.Instance.TodayMaterialRanking.OrderBy(x => x).Reverse())
        {
            Item item;

            if (!ItemManager.Instance.FindItem(info.materialItemID, out item)) continue;

            GameObject score = Instantiate(materialScoreSet);
            Image image = score.transform.Find("Image").gameObject.GetComponent<Image>();
            Text materialName = score.transform.Find("MaterialName").gameObject.GetComponent<Text>();
            Text materialDescription = score.transform.Find("MaterialDescription").gameObject.GetComponent<Text>();
            Text materialScore = score.transform.Find("MaterialScore").gameObject.GetComponent<Text>();
            Text playerName = score.transform.Find("PlayerName").gameObject.GetComponent<Text>();

            image.sprite = Resources.Load<Sprite>("2D/" + item.ItemID);
            materialName.text = item.ItemName;
            materialDescription.text = item.Description;
            materialScore.text = string.Format("分數 : {0}", (item as IsolatedIslandGame.Library.Items.Material).Score);

            PlayerInformation playerInformation;
            if (!PlayerInformationManager.Instance.FindPlayerInformation(info.playerID, out playerInformation)) continue;

            playerName.text = string.Format("投放玩家 : {0}", playerInformation.nickname);

            score.transform.SetParent(materialScoreSetContent.transform);
            score.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    private void UpdateScoreBoard(Island.PlayerScoreInfo info)
    {
        LoadScoreBoard();
    }
}
