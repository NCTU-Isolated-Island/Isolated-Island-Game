using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;

public class GameManager : MonoBehaviour {

	public static GameManager Instance; // ASK need to implement standard singleton?
	public GameObject DefaultShipModel;
	public List<GameObject> ElementModel;
	public Dictionary<int,GameObject> UserGameObject;

	void Awake () {
		
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);


		//FacebookService.InitialFacebook();
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
		UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;
		UserManager.Instance.User.Player.OnCreateCharacter += OnCreateCharacter;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
		UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
	}

	void Start()
	{
		StartCoroutine(SlowUpdate());
	}
		
	void OnPlayerOnline(Player player)
	{print("this?");
		if(UserManager.Instance.User.Player.GroupType == IsolatedIslandGame.Protocol.GroupType.No)
		{
			UserManager.Instance.User.Player.OperationManager.CreateCharacter("nickname","signature",IsolatedIslandGame.Protocol.GroupType.A);
		}
		else
		{
			SceneManager.LoadScene("MainScene");
		}

	}

	//TODO make sure回傳值是nickname
	void OnCreateCharacter(Player player)
	{
		SceneManager.LoadScene("MainScene");
	}



	void OnSceneLoaded(Scene a,LoadSceneMode b){
		

		//InstantiateUserGameObject


		//設定完之後再把顯示出遊戲畫面（在這之前可能顯示loading bar
	}

	public void OnPlayerLocationChange()
	{
		//Send LocationInfo to server
	}

	IEnumerator SlowUpdate()
	{
		while(true)
		{
			yield return new WaitForSeconds(5f);
		}
	}

	/*
	void InstantiateUserGameObject(){

		// foreach user in userList

		GameObject user = Instantiate(
			defaultShipGameObject,
			User.Position,
			Quaternion.identity
		) as GameObject;
		
		user.name = userId; //or maybe Nickname


		// foreach user.PlacedElement
		Instantiate(
			elementGameObject[element.id],
			element.position,
			Quaternion.identity,
			user,
		);

		UserGameObject.Add(user.Id,user);

	}
	*/

	void OnUserLocationChange(int userId, Vector3 position)
	{
		//改變對應的User位置
		GameObject user;
		UserGameObject.TryGetValue(userId, out user);

		//TODO 目前讓他直接瞬移 之後再優化成smoothDamp
		user.transform.position = position;

	}

	void OnUserPlaceElement(int itemId, int userId, Vector3 localPosition)
	{
		
		GameObject user;
		UserGameObject.TryGetValue(userId, out user);

		Instantiate
		(
			ElementModel[itemId],
			user.transform.position + localPosition,
			Quaternion.identity,
			user.transform
		);

	}

}
