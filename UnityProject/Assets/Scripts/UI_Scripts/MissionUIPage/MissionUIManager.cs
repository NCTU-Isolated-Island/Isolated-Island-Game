using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{

    public static MissionUIManager Instance { get; private set; }

    // UI Variable
    [SerializeField]
    private GameObject missionSet;
    [SerializeField]
    private GameObject missionSetContent;

    //

    [SerializeField]
    private GameObject missionDetailScrollView;
    [SerializeField]
    private GameObject missionDetailContent;
    [SerializeField]
    private GameObject missionPrizeContent;
    [SerializeField]
    private GameObject missionPrizeSet;

    private QuestRecordInformation viewingQuest;

    //

    public void OpenMissionDetailPage(QuestRecordInformation information)
    {
        missionDetailScrollView.SetActive(true);

        Text missionName = missionDetailContent.transform.Find("MissionName").GetComponent<Text>();
        //Text missionCountDown = missionDetailContent.transform.Find("MissionCountDown").GetComponent<Text>();
        Text missionDescription = missionDetailContent.transform.Find("MissionDescription").GetComponent<Text>();

        missionName.text = information.questName;
        missionDescription.text = information.questDescription;

        foreach (Transform trash in missionPrizeContent.transform)
        {
            Destroy(trash.gameObject);
        }

        //foreach (QuestReward prize in quest.Rewards)
        //{
        //    print(prize.Description);
        //    GameObject tmp = Instantiate(missionPrizeSet);
        //    tmp.transform.SetParent(missionPrizeContent.transform);

        //    if (prize.QuestRewardType == QuestRewardType.GiveSpecificNumberSpecificItem)
        //    {
        //        Item item;
        //        ItemManager.Instance.FindItem(prize.QuestRewardID, out item);

        //        tmp.GetComponent<Image>().sprite = Resources.Load<Sprite>("2D/" + item.ItemID);
        //        //tmp.transform.Find("prizeItemName").GetComponent<Text>().text = item.ItemName;
        //        tmp.transform.Find("prizeItemName").GetComponent<Text>().text = prize.Description;
        //    }
        //    else if (prize.QuestRewardType == QuestRewardType.UnlockSpecificBlueprint)
        //    {
        //        // give a blurprint sprite
        //        tmp.GetComponent<Image>().sprite = null;
        //        //tmp.transform.Find("prizeItemName").GetComponent<Text>().text = "解鎖藍圖";
        //        tmp.transform.Find("prizeItemName").GetComponent<Text>().text = prize.Description;
        //    }
        //    tmp.transform.localScale = Vector3.one;
        //}

        // Add Detail Page Listener
        viewingQuest = information;
    }
    //

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        gameObject.SetActive(false);
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
        LoadQuestList(new QuestRecordInformation());
    }

    void RegisterPlayerEvents(Player player)
    {
        player.OnQuestRecordInformationUpdated += LoadQuestList;
    }

    void LoadQuestList(QuestRecordInformation information)
    {
        //print("LoadQuestList");
        foreach (Transform questRecordObject in missionSetContent.transform)
        {
            Destroy(questRecordObject.gameObject);
        }

        foreach (QuestRecordInformation questRecordInformation in UserManager.Instance.User.Player.QuestRecordInformations)
        {
            GameObject tmp = Instantiate(missionSet);
            //
            tmp.GetComponent<MissionSetBehavior>().SetQuestInfo(questRecordInformation);
            //
            Text missionName = tmp.transform.Find("MissionName").GetComponent<Text>();
            Text missionCountDown = tmp.transform.Find("MissionCountDown").GetComponent<Text>();
            Text missionType = tmp.transform.Find("MissionType").GetComponent<Text>();
            //Text missionLocation = tmp.transform.Find("MissionLocation").GetComponent<Text>();
            //Text missionPrize = tmp.transform.Find("MissionPrize").GetComponent<Text>();

            missionName.text = questRecordInformation.questName;
            //missionCountDown = questRecord.Quest;
            switch (questRecordInformation.questType)
            {
                case QuestType.QR_Code:
                    missionType.text = "掃描 QR code";
                    break;
                case QuestType.MutiplayerSynthesize:
                    missionType.text = "多人合成";
                    break;
                case QuestType.Message:
                    missionType.text = "傳送訊息";
                    break;
                case QuestType.Deal:
                    missionType.text = "完成交易";
                    break;
                default:
                    missionType.text = "";
                    break;
            }
            //missionLocation.text = questRecord.Quest;

            tmp.transform.SetParent(missionSetContent.transform);
            tmp.transform.localScale = Vector3.one;
        }
    }

    public void AcceptQuest()
    {
        throw new NotImplementedException();
        // TODO : let player accept this quest
    }
}
