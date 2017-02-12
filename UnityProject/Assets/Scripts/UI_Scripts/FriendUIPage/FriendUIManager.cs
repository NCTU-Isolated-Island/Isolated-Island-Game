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
        print("Loading Friends");
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
                    tmp.transform.parent = friendSetContent.transform;
                    tmp.gameObject.GetComponent<InvitingFriendSetBehavior>().information = friendInformation;
                }
                else
                {
                    tmp = Instantiate(confirmedFriendSetPrefab);
                    tmp.transform.parent = friendSetContent.transform;
                    tmp.gameObject.GetComponent<ConfirmedFriendSetBehavior>().information = friendInformation;
                }
                // Write info into prefab

                Text friendName = tmp.transform.FindChild("FriendName").GetComponent<Text>();
                Text friendGroup = tmp.transform.FindChild("FriendGroup").GetComponent<Text>();

                friendName.text = friendInformation.nickname;

                switch (friendInformation.groupType)
                {
                    case GroupType.Animal:
                        friendGroup.text = "信仰";
                        break;
                    case GroupType.Businessman:
                        friendGroup.text = "動物";
                        break;
                    case GroupType.Farmer:
                        friendGroup.text = "農夫";
                        break;
                }
            }
        }
    }

	void LoadFriendList()
	{
		
	}
}
