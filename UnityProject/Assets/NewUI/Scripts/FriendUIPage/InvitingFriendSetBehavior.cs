using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitingFriendSetBehavior : MonoBehaviour {

    public PlayerInformation information;

    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
    public void AcceptFriendInvite()
    {
        UserManager.Instance.User.Player.OperationManager.AcceptFriend(information.playerID);
    }

    public void RefuseFriendInvite()
    {
        UserManager.Instance.User.Player.OperationManager.DeleteFriend(information.playerID);
        Destroy(gameObject);
    }

}
