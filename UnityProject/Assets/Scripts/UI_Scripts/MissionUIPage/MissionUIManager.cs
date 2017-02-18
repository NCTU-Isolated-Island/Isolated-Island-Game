using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
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

    //

    public void OpenMissionDetailPage(Quest quest)
    {
        missionDetailScrollView.SetActive(true);

        Text missionName = missionDetailContent.transform.Find("MissionName").GetComponent<Text>();
        //Text missionCountDown = missionDetailContent.transform.Find("MissionCountDown").GetComponent<Text>();
        Text missionDescription = missionDetailContent.transform.Find("MissionDescription").GetComponent<Text>();

        missionName.text = quest.QuestName;
        missionDescription.text = quest.QuestDescription;

        foreach (Transform trash in missionPrizeContent.transform)
        {
            Destroy(trash.gameObject);
        }

        foreach (QuestReward prize in quest.Rewards)
        {
            if (prize.QuestRewardType == QuestRewardType.GiveItem)
            {
                GameObject tmp = Instantiate(missionPrizeSet);
                tmp.transform.SetParent(missionPrizeContent.transform);
                Item item;
                ItemManager.Instance.FindItem(prize.QuestRewardID, out item);
                tmp.transform.Find("prizeItemName").GetComponent<Text>().text = item.ItemName;

                tmp.transform.localScale = Vector3.one;
            }
        }

        // Add Detail Page Listener
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
        LoadQuestList(null);
    }

    void RegisterPlayerEvents(Player player)
    {
        player.OnQuestRecordUpdated += LoadQuestList;
    }

    void LoadQuestList(QuestRecord obj)
    {
        //print("LoadQuestList");
        foreach (Transform questRecord in missionSetContent.transform)
        {
            Destroy(questRecord.gameObject);
        }

        foreach (QuestRecord questRecord in UserManager.Instance.User.Player.QuestRecords)
        {
            GameObject tmp = Instantiate(missionSet);
            //
            tmp.GetComponent<MissionSetBehavior>().SetQuestInfo(questRecord.Quest);
            //
            Text missionName = tmp.transform.Find("MissionName").GetComponent<Text>();
            Text missionCountDown = tmp.transform.Find("MissionCountDown").GetComponent<Text>();
            Text missionType = tmp.transform.Find("MissionType").GetComponent<Text>();
            //Text missionLocation = tmp.transform.Find("MissionLocation").GetComponent<Text>();
            //Text missionPrize = tmp.transform.Find("MissionPrize").GetComponent<Text>();

            missionName.text = questRecord.Quest.QuestName;
            //missionCountDown = questRecord.Quest;
            switch (questRecord.Quest.QuestType)
            {
                case QuestType.QR_Code:
                    missionType.text = "掃描 QR code";
                    break;
                case QuestType.MutiplayerSynthesize:
                    missionType.text = "多人合成";
                    break;
                case QuestType.SendMessage:
                    missionType.text = "傳送訊息";
                    break;
                case QuestType.CloseDeal:
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
}
