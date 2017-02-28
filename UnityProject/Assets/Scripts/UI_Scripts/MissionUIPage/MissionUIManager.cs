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
//    [SerializeField]
//    private GameObject missionPrizeContent;
//    [SerializeField]
//    private GameObject missionPrizeSet;

    private QuestRecordInformation viewingQuest;

    //

    public void OpenMissionDetailPage(QuestRecordInformation information)
    {
        missionDetailScrollView.SetActive(true);

		Text missionName = missionDetailContent.transform.Find ("MissionName").GetComponent<Text> ();
		Text missionDescription = missionDetailContent.transform.Find ("MissionDescription").GetComponent<Text> ();
		Text missionRequirement = missionDetailContent.transform.Find ("MissionRequirementTitle/MissionRequirementContent").GetComponent<Text> ();
		Text missionReward = missionDetailContent.transform.Find ("MissionRewardTitle/MissionRewardContent").GetComponent<Text> ();

//		print (information.questName);
//		print (information.questDescription);
//		print (information.requirementsDescription);
//		print (information.rewardsDescription);

        missionName.text = information.questName;
        missionDescription.text = information.questDescription;
		missionRequirement.text = information.requirementsDescription;
		missionReward.text = information.rewardsDescription;

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
			if (questRecordInformation.isFinished && questRecordInformation.hasGottenReward)
				continue;

            GameObject tmp = Instantiate(missionSet);
            //
            tmp.GetComponent<MissionSetBehavior>().SetQuestInfo(questRecordInformation);
            //
            Text missionName = tmp.transform.Find("MissionName").GetComponent<Text>();
            Text missionType = tmp.transform.Find("MissionType").GetComponent<Text>();

			if (questRecordInformation.isFinished == false)
				missionName.text = questRecordInformation.questName;
			else
				missionName.text = "已完成 點擊以領取獎勵";

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
                case QuestType.Trade:
                    missionType.text = "完成交易";
                    break;
                default:
                    missionType.text = "";
                    break;
            }

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
