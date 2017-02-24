using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendUIManager : MonoBehaviour {

    public static FriendUIManager Instance { get; private set; }

    //
    [SerializeField]
    private GameObject confirmedFriendSetPrefab;
    [SerializeField]
    private GameObject invitingFriendSetPrefab;
    //

    // UI Variable
    [SerializeField]
    private GameObject friendSetContent;
    //

    void OnEnable()
    {
        LoadFriends();
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
    }

    void RegisterPlayerEvents(Player player)
    {
        player.OnFriendInformationChange += OnFriendInformationChange;
        VesselManager.Instance.OnVesselChange += (changeType, vessel) =>
        {
            if(player.ContainsFriend(vessel.OwnerPlayerID))
                LoadFriends();
        };
        LoadFriends();
    }

    void OnFriendInformationChange(DataChangeType dataChangeType, FriendInformation information)
    {
        LoadFriends();
    }

    public void LoadFriends()
    {
        //print("Loading Friends");
        foreach(Transform renderedFriend in friendSetContent.transform)
        {
            Destroy(renderedFriend.gameObject);
        }

        foreach (var friend in UserManager.Instance.User.Player.FriendInformations)
        {
            GameObject tmp;
            PlayerInformation friendInformation;
            if (PlayerInformationManager.Instance.FindPlayerInformation(friend.friendPlayerID, out friendInformation))
            {
                if (friend.isConfirmed == false && friend.isInviter == true)
                {
                    tmp = Instantiate(invitingFriendSetPrefab);
                    tmp.transform.SetParent(friendSetContent.transform);
                    tmp.gameObject.GetComponent<InvitingFriendSetBehavior>().information = friendInformation;
                }
                else
                {
                    tmp = Instantiate(confirmedFriendSetPrefab);
                    tmp.transform.SetParent(friendSetContent.transform);
                    tmp.gameObject.GetComponent<ConfirmedFriendSetBehavior>().information = friendInformation;
                }

                tmp.GetComponent<RectTransform>().localScale = Vector2.one;
                // Write info into prefab

                Text friendName = tmp.transform.FindChild("FriendName").GetComponent<Text>();
                Text friendGroup = tmp.transform.FindChild("FriendGroup").GetComponent<Text>();
                Text friendSpeech;
                Image image = tmp.transform.FindChild("Image").GetComponent<Image>();

                if (tmp.transform.FindChild("FriendSpeech") != null)
                {
                    friendSpeech = tmp.transform.FindChild("FriendSpeech").GetComponent<Text>();
                    friendSpeech.text = friendInformation.signature;
                }

                friendName.text = friendInformation.nickname;

                switch (friendInformation.groupType)
                {
                    case GroupType.Animal:
                        friendGroup.text = "信仰";
                        image.sprite = Resources.Load<Sprite>("GroupIcon/animal");
                        break;
                    case GroupType.Businessman:
                        friendGroup.text = "科技";
                        image.sprite = Resources.Load<Sprite>("GroupIcon/businessman");
                        break;
                    case GroupType.Farmer:
                        friendGroup.text = "自然";
                        image.sprite = Resources.Load<Sprite>("GroupIcon/farmer");
                        break;
                }
            }
        }
    }

	void LoadFriendList()
	{
		
	}
}
