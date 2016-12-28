using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;

public class GameManager : MonoBehaviour {

	public static GameManager Instance; // ASK need to implement standard singleton?
	[HideInInspector]public GameObject DefaultShipModel;
	public List<GameObject> ElementModel;
	public Dictionary<int,GameObject> UserGameObject = new Dictionary<int, GameObject>(); //UserID to GO
	public Dictionary<int,GameObject> VesselIDGameObject = new Dictionary<int, GameObject>(); //VesselID to GO
<<<<<<< HEAD:UnityProject/Assets/Steven/GameManager.cs
	public Dictionary<int,Dictionary<int,GameObject>> UserDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // VesselID to decorationID-decorationGO
	public GameObject PlayerGameObject;
=======
	public Dictionary<int,Dictionary<int,GameObject>> VesselDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // VesselID to decorationID-decorationGO
	[HideInInspector]public GameObject PlayerGameObject;
>>>>>>> origin/PlayerBehavior:UnityProject/Assets/Scripts/GameManager.cs

	void Awake () {
		
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		DefaultShipModel = Resources.Load<GameObject>("ShipModel");



	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
		UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;

	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
		UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
		UserManager.Instance.User.Player.OnCreateCharacter -= OnCreateCharacter;
		VesselManager.Instance.OnVesselTransformUpdated -= OnVesselTransformUpdated;
	}


	void Start()
	{
		StartCoroutine(SlowUpdate());
		FacebookService.LoginWithFacbook();

	}

	IEnumerator SlowUpdate()
	{
		while(true)
		{
			yield return new WaitForSeconds(5f);
		}
	}

	void OnPlayerOnline(Player player)
	{
		
		UserManager.Instance.User.Player.OnCreateCharacter += OnCreateCharacter;
		VesselManager.Instance.OnVesselTransformUpdated += OnVesselTransformUpdated;

		print(player.GroupType.ToString());
		if(UserManager.Instance.User.Player.GroupType == IsolatedIslandGame.Protocol.GroupType.No)
		{
			SceneManager.LoadScene("RegisterScnee");

			UserManager.Instance.User.Player.OperationManager.CreateCharacter("ABC","signature",IsolatedIslandGame.Protocol.GroupType.B);
		}
		else
		{
			SceneManager.LoadScene("MainScene");
		}
	}
		
	void OnCreateCharacter(Player player)
	{
		SceneManager.LoadScene("MainScene");
	}

	void OnSceneLoaded(Scene a,LoadSceneMode b){
		
		if(a == SceneManager.GetSceneByName("MainScene"))
		{
			LocationSystem.Instance.StartLocationService();
			InstantiateUserGameObject();
			GetPlayerVesselGameObject();
		}

		//TODO 設定完之後再把顯示出遊戲畫面（在這之前可能顯示loading bar
	}

	public void OnPlayerLocationChange(Vector3 position,float eulerAngleY)
	{
		PlayerGameObject.transform.position = position;
		PlayerGameObject.transform.rotation = Quaternion.Euler(0f,eulerAngleY,0f);

		UserManager.Instance.User.Player.OperationManager.UpdateVesselTransform
		(
			position.x,
			position.z,
			eulerAngleY
		);
	}
		

	void InstantiateUserGameObject(){

		foreach(Vessel vessel in VesselManager.Instance.Vessels)
		{
			Dictionary<int,GameObject> decorationDic = new Dictionary<int, GameObject>();

			GameObject user = Instantiate(
				DefaultShipModel,
				new Vector3(vessel.LocationX,0f,vessel.LocationZ),
				Quaternion.Euler(0f,vessel.RotationEulerAngleY,0f)
			) as GameObject;

			foreach(Decoration decoration in vessel.Decorations)
			{
				GameObject dec = Instantiate(
					ElementModel[decoration.Material.MaterialID],
					new Vector3
					(
						decoration.PositionX,
						decoration.PositionY,
						decoration.PositionZ
					) + user.transform.position, 
					Quaternion.Euler
					(
						decoration.RotationEulerAngleX,
						decoration.RotationEulerAngleY,
						decoration.RotationEulerAngleZ
					),
					user.transform
				) as GameObject;

				decorationDic.Add(decoration.DecorationID,dec);
			}

			UserGameObject.Add(vessel.OwnerPlayerID,user);
			VesselIDGameObject.Add(vessel.VesselID,user);
			UserDecoration.Add(vessel.VesselID,decorationDic);
			user.name = "OwnerID: " + vessel.OwnerPlayerID;
		}
	}

	void GetPlayerVesselGameObject()
	{
		bool success = UserGameObject.TryGetValue
		(
			UserManager.Instance.User.Player.PlayerID,
			out PlayerGameObject
		);
		
		if(!success)
		{
			Debug.LogError("GetPlayerVesselGameObject Error!");	
		}
	}

	#region Vessel

	void OnVesselTransformUpdated(int vesselID, float locationX, float locationZ, float rotationEulerAngleY)
	{
		GameObject user;
		VesselIDGameObject.TryGetValue(vesselID,out user);
		user.transform.position = new Vector3(locationX,0f,locationZ);
		user.transform.rotation = Quaternion.Euler(0f,rotationEulerAngleY,0f);
	}


	void OnDecorationChange(int vesselID, Decoration decoration, DataChangeType changeType)
	{ 
		GameObject user;
		VesselIDGameObject.TryGetValue(vesselID,out user);

		if(changeType == DataChangeType.Add)
		{
			Instantiate(
				ElementModel[decoration.Material.MaterialID],
				new Vector3
				(
					decoration.PositionX,
					decoration.PositionY,
					decoration.PositionZ
				) + user.transform.position, 
				Quaternion.Euler
				(
					decoration.RotationEulerAngleX,
					decoration.RotationEulerAngleY,
					decoration.RotationEulerAngleZ
				),
				user.transform
			);
		}
		else if(changeType == DataChangeType.Remove)
		{
			
		}
		else if(changeType == DataChangeType.Update)
		{
			Dictionary<int, GameObject> decorationDic;
			if(UserDecoration.TryGetValue(vesselID,out decorationDic))
			{
				GameObject dec;
				if(decorationDic.TryGetValue(decoration.DecorationID,out dec))
				{
					dec.transform.localPosition = new Vector3
						(
							decoration.PositionX,
							decoration.PositionY,
							decoration.PositionZ
						);
					dec.transform.localRotation = Quaternion.Euler
						(
							decoration.RotationEulerAngleX,
							decoration.RotationEulerAngleY,
							decoration.RotationEulerAngleZ
						);
				}
			}
		
		}
	}//當船上的裝飾物更新時的回調事件

	#endregion

//	void OnUserOnline(Player player)
//	{
//		GameObject user = Instantiate(
//			DefaultShipModel,
//			new Vector3(player.Vessel.LocationX,0f,player.Vessel.LocationZ),
//			Quaternion.Euler(0f,player.Vessel.RotationEulerAngleY,0f)
//		) as GameObject;
//
//		foreach(Decoration decoration in player.Vessel.Decorations)
//		{
//			Instantiate(
//				ElementModel[decoration.Material.MaterialID],
//				new Vector3
//				(
//					decoration.PositionX,
//					decoration.PositionY,
//					decoration.PositionZ
//				) + user.transform.position, 
//				Quaternion.Euler
//				(
//					decoration.RotationEulerAngleX,
//					decoration.RotationEulerAngleY,
//					decoration.RotationEulerAngleZ
//				),
//				user.transform
//			);
//		}
//
//		UserGameObject.Add(player.PlayerID,user);
//		VesselIDGameObject.Add(player.Vessel.VesselID,user);
//		user.name = "OwnerID: " + player.PlayerID;
//	}

//	void OnUserOffline(Player player)
//	{
//		GameObject temp;
//		UserGameObject.TryGetValue(player.PlayerID, out temp);
//		Destroy(temp);
//
//		UserGameObject.Remove(player.PlayerID);
//		VesselIDGameObject.Remove(player.Vessel.VesselID);
//	}




	void OnGUI(){
		foreach(InventoryItemInfo info in UserManager.Instance.User.Player.Inventory.ItemInfos)
		{
			GUILayout.Label(info.Item.ItemName + " : " + info.Count + " ID: " + info.Item.ItemID);
		}
	}

}
