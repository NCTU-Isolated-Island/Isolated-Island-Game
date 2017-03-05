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
		Image missionIcon = missionDetailContent.transform.Find ("MissionIcon").GetComponent<Image> ();

//		print (information.questName);
//		print (information.questDescription);
//		print (information.requirementsDescription);
//		print (information.rewardsDescription);

        missionName.text = information.questName;
        missionDescription.text = information.questDescription;
		missionRequirement.text = information.requirementsDescription;
		missionReward.text = information.rewardsDescription;

		switch (information.questType) {
		case QuestType.QR_Code:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestQR");
			break;
		case QuestType.MutiplayerSynthesize:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestStranger");
			break;
		case QuestType.Message:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestFriend");
			break;
		case QuestType.Trade:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestFriend");
			break;
		case QuestType.Synthesize:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
			break;
		case QuestType.Achievement:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
			break;
		case QuestType.CollectItem:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
			break;
		case QuestType.Friend:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestStranger");
			break;
		case QuestType.Island:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
			break;
		case QuestType.Login:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
			break;
		case QuestType.Ocean:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestLocation");
			break;
		default:
			missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
			break;
		}

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
			Destroy (questRecordObject.gameObject);
		}

		foreach (QuestRecordInformation questRecordInformation in UserManager.Instance.User.Player.QuestRecordInformations)
        {
			if (questRecordInformation.isFinished && questRecordInformation.hasGottenReward)
				continue;

			GameObject tmp = Instantiate (missionSet);
			//
			tmp.GetComponent<MissionSetBehavior> ().SetQuestInfo (questRecordInformation);
			//
			Text missionName = tmp.transform.Find ("MissionName").GetComponent<Text> ();
			Text missionType = tmp.transform.Find ("MissionType").GetComponent<Text> ();
			Image missionIcon = tmp.transform.Find ("MissionIcon").GetComponent<Image> ();
			Image finishedImage = tmp.transform.Find ("FinishedImage").GetComponent<Image> ();

			if (questRecordInformation.isFinished == false)
				finishedImage.gameObject.SetActive (false);
			else
				finishedImage.gameObject.SetActive (true);

            missionName.text = questRecordInformation.questName;


            switch (questRecordInformation.questType) {
			case QuestType.QR_Code:
				missionType.text = "掃描 QR code";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestQR");
				break;
			case QuestType.MutiplayerSynthesize:
				missionType.text = "多人合成";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestStranger");
				break;
			case QuestType.Message:
				missionType.text = "傳送訊息";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestFriend");
				break;
			case QuestType.Trade:
				missionType.text = "完成交易";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestFriend");
				break;
			case QuestType.Synthesize:
				missionType.text = "個人合成";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
				break;
			case QuestType.Achievement:
				missionType.text = "解除成就";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
				break;
			case QuestType.CollectItem:
				missionType.text = "收集素材";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
				break;
			case QuestType.Friend:
				missionType.text = "加入好友";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestStranger");
				break;
			case QuestType.Island:
				missionType.text = "島嶼";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
				break;
			case QuestType.Login:
				missionType.text = "登入";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
				break;
			case QuestType.Ocean:
				missionType.text = "海洋";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestLocation");
				break;
			default:
				missionType.text = "";
				missionIcon.sprite = Resources.Load<Sprite> ("QuestIcon/QuestIndependent");
				break;
			}

			tmp.transform.SetParent (missionSetContent.transform);
			tmp.transform.localScale = Vector3.one;
		}
	}

    public void AcceptQuest()
    {
        throw new NotImplementedException();
        // TODO : let player accept this quest
    }
}
