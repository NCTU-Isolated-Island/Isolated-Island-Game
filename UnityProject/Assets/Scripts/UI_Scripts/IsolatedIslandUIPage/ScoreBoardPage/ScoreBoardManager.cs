using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public static ScoreBoardManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    [SerializeField]
    private GameObject currentScoreBoardPage;
    [SerializeField]
    private GameObject scoreSetContent;
    [SerializeField]
    private GameObject scoreSet;

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
        scoreSetContent = currentScoreBoardPage.transform.GetComponentInChildren<ContentSizeFitter>().gameObject;
    }

    public void LoadScoreBoard()
    {
        if (currentScoreBoardPage == null) return;

        foreach (Transform oldSet in scoreSetContent.transform)
        {
            Destroy(oldSet.gameObject);
        }

        foreach (Island.PlayerScoreInfo info in Island.Instance.PlayerScoreRanking.OrderBy(x => x.score).Reverse())
        {
            PlayerInformation playerInformation;

            if (!PlayerInformationManager.Instance.FindPlayerInformation(info.playerID, out playerInformation)) continue;

            GameObject score = Instantiate(scoreSet);
            Image image = score.transform.Find("Image").gameObject.GetComponent<Image>();
            Text playerGroup = score.transform.Find("PlayerGroup").gameObject.GetComponent<Text>();
            Text playerName = score.transform.Find("PlayerName").gameObject.GetComponent<Text>();
            Text playerSpeech = score.transform.Find("PlayerSpeech").gameObject.GetComponent<Text>();
            Text playerScore = score.transform.Find("PlayerScore").gameObject.GetComponent<Text>();

            playerName.text = playerInformation.nickname;
            playerSpeech.text = playerInformation.signature;
            playerScore.text = string.Format("得分 : {0}", info.score.ToString());

            switch (playerInformation.groupType)
            {
                case GroupType.Animal:
                    playerGroup.text = "信仰";
                    image.sprite = Resources.Load<Sprite>("GroupIcon/animal");
                    break;
                case GroupType.Businessman:
                    playerGroup.text = "科技";
                    image.sprite = Resources.Load<Sprite>("GroupIcon/businessman");
                    break;
                case GroupType.Farmer:
                    playerGroup.text = "自然";
                    image.sprite = Resources.Load<Sprite>("GroupIcon/farmer");
                    break;
            }

            score.transform.SetParent(scoreSetContent.transform);
            score.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    private void UpdateScoreBoard(Island.PlayerScoreInfo info)
    {
        LoadScoreBoard();
    }

}
