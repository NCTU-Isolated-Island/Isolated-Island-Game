using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library.TextData;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageManager : MonoBehaviour {

	public static MessageManager Instance;

	public List<PlayerConversation> Conversations;

	void Awake()
	{
		if(Instance == null)
			Instance = this;
	}

	void Start()
	{
		UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;

	}

	void OnDestroy()
	{
		UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
		UserManager.Instance.User.Player.OnGetPlayerConversation -= OnGetPlayerConversation;
	}

	void OnPlayerOnline(Player player)
	{
		UserManager.Instance.User.Player.OnGetPlayerConversation += OnGetPlayerConversation;
	}

	void OnGetPlayerConversation(PlayerConversation conversation)
	{
		Conversations.Add(conversation);
	}

}
