﻿using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{

    public static MissionUIManager Instance { get; private set; }

    [SerializeField]
    private GameObject questSet;
    [SerializeField]
    private GameObject missionSetContent;
    [SerializeField]
    private GameObject missionDetailScrollView;
    [SerializeField]
    private GameObject missionDetailContent;

    private QuestRecordInformation viewingQuest;


    public void OpenMissionDetailPage(QuestRecordInformation information)
    {
        missionDetailScrollView.SetActive(true);

        Text missionName = missionDetailContent.transform.Find("MissionName").GetComponent<Text>();
        Text missionDescription = missionDetailContent.transform.Find("MissionDescription").GetComponent<Text>();
        Text missionRequirement = missionDetailContent.transform.Find("MissionRequirementTitle/MissionRequirementContent").GetComponent<Text>();
        Text missionReward = missionDetailContent.transform.Find("MissionRewardTitle/MissionRewardContent").GetComponent<Text>();
        Image missionIcon = missionDetailContent.transform.Find("MissionIcon").GetComponent<Image>();

        Button takeRewardButton = missionDetailScrollView.transform.Find("TakeRewardButton").GetComponent<Button>();

        //		print (information.questName);
        //		print (information.questDescription);
        //		print (information.requirementsDescription);
        //		print (information.rewardsDescription);

        if (information.isFinished)
        {
            takeRewardButton.gameObject.SetActive(true);

            takeRewardButton.onClick.RemoveAllListeners();
            takeRewardButton.onClick.AddListener(delegate
            {
                UserManager.Instance.User.Player.OperationManager.TakeQuestReward(information.questRecordID);
                missionDetailScrollView.SetActive(false);
            });
        }
        else
        {
            takeRewardButton.gameObject.SetActive(false);
        }

        missionName.text = information.questName;
        missionDescription.text = information.questDescription;
        missionRequirement.text = information.requirementsDescription;
        missionReward.text = information.rewardsDescription;

        switch (information.questType)
        {
            case QuestType.QR_Code:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestQR");
                break;
            case QuestType.MutiplayerSynthesize:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestStranger");
                break;
            case QuestType.Message:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestFriend");
                break;
            case QuestType.Trade:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestFriend");
                break;
            case QuestType.Synthesize:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                break;
            case QuestType.Achievement:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                break;
            case QuestType.CollectItem:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                break;
            case QuestType.Friend:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestStranger");
                break;
            case QuestType.Island:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                break;
            case QuestType.Login:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                break;
            case QuestType.Ocean:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestLocation");
                break;
            default:
                missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                break;
        }
        viewingQuest = information;
    }

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
        LoadQuestList();
    }

    void RegisterPlayerEvents(Player player)
    {
        player.OnQuestRecordInformationUpdated += OnQuestRecordInformationUpdated;
    }

    void OnQuestRecordInformationUpdated(QuestRecordInformation information)
    {
        LoadQuestList();
    }

    void LoadQuestList()
    {
        foreach (Transform questRecordObject in missionSetContent.transform)
        {
            Destroy(questRecordObject.gameObject);
        }

        foreach (QuestRecordInformation questRecordInformation in UserManager.Instance.User.Player.QuestRecordInformations.OrderBy(quest => quest.questType).OrderBy(quest => quest.isFinished).Reverse())
        {
            if (questRecordInformation.hasGottenReward)
                continue;

            GameObject questRow = Instantiate(questSet);
            QuestRecordInformation record = questRecordInformation;
            questRow.GetComponent<MissionSetBehavior>().SetQuestInfo(record);
            Text missionName = questRow.transform.Find("MissionName").GetComponent<Text>();
            Text missionType = questRow.transform.Find("MissionType").GetComponent<Text>();
            Image missionIcon = questRow.transform.Find("MissionIcon").GetComponent<Image>();
            Image finishedImage = questRow.transform.Find("FinishedImage").GetComponent<Image>();

            finishedImage.gameObject.SetActive(questRecordInformation.isFinished);

            missionName.text = questRecordInformation.questName;

            switch (questRecordInformation.questType)
            {
                case QuestType.QR_Code:
                    missionType.text = "掃描 QR code";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestQR");
                    break;
                case QuestType.MutiplayerSynthesize:
                    missionType.text = "多人合成";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestStranger");
                    break;
                case QuestType.Message:
                    missionType.text = "傳送訊息";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestFriend");
                    break;
                case QuestType.Trade:
                    missionType.text = "完成交易";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestFriend");
                    break;
                case QuestType.Synthesize:
                    missionType.text = "個人合成";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                    break;
                case QuestType.Achievement:
                    missionType.text = "解除成就";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                    break;
                case QuestType.CollectItem:
                    missionType.text = "收集素材";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                    break;
                case QuestType.Friend:
                    missionType.text = "加入好友";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestStranger");
                    break;
                case QuestType.Island:
                    missionType.text = "島嶼";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                    break;
                case QuestType.Login:
                    missionType.text = "登入";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                    break;
                case QuestType.Ocean:
                    missionType.text = "海洋";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestLocation");
                    break;
                default:
                    missionType.text = "";
                    missionIcon.sprite = Resources.Load<Sprite>("QuestIcon/QuestIndependent");
                    break;
            }

            questRow.transform.SetParent(missionSetContent.transform);
            questRow.transform.localScale = Vector3.one;
        }
    }

    //public void AcceptQuest()
    //{
    //    throw new NotImplementedException();
    //    // TODO : let player accept this quest
    //}
}
