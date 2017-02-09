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

    void InitSetting()
    {

    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void OnEnable()
    {
        LoadFriends();
    }

    public void LoadFriends()
    {
        foreach(Transform renderedFriend in friendSetContent.transform)
        {
            Destroy(renderedFriend.gameObject);
        }

        foreach (var friend in UserManager.Instance.User.Player.FriendInformations)
        {
            GameObject tmp;
            if (friend.isConfirmed == false)
            {
                tmp = Instantiate(invitingFriendSetPrefab);
                tmp.transform.parent = friendSetContent.transform;
            }
            else
            {
                tmp = Instantiate(confirmedFriendSetPrefab);
                tmp.transform.parent = friendSetContent.transform;
            }
            // Write info into prefab

            Text friendName = tmp.transform.FindChild("FriendName").GetComponent<Text>();
            Text friendGroup = tmp.transform.FindChild("FriendGroup").GetComponent<Text>();

            PlayerInformation friendInformation;
            if (PlayerInformationManager.Instance.FindPlayerInformation(friend.friendPlayerID, out friendInformation))
            {
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
}
