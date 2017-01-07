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
	public GameObject DefaultShipModel;
	public List<GameObject> ElementModel;
	public List<GameObject> ShipModel;
	public Dictionary<int,GameObject> UserGameObject = new Dictionary<int, GameObject>(); //UserID to GO
	public Dictionary<int,GameObject> VesselIDGameObject = new Dictionary<int, GameObject>(); //VesselID to GO
	public Dictionary<int,Dictionary<int,GameObject>> UserDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // PlayerID to decorationID-decorationGO
	public Dictionary<int,Dictionary<int,GameObject>> VesselDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // VesselID to decorationID-decorationGO
	public GameObject PlayerGameObject;

	public CameraStat cameraStat;
	public enum CameraStat{
		Near,
		Far
	}

	void Awake () {
		
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		DefaultShipModel = Resources.Load<GameObject>("ShipGameObject");



	}

	void Update()
	{


	}

	void OnEnable()
	{
		UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
		UserManager.Instance.User.Player.OnCreateCharacter -= OnCreateCharacter;
		SceneManager.sceneLoaded -= OnSceneLoaded;
		VesselManager.Instance.OnVesselTransformUpdated -= OnVesselTransformUpdated;
		VesselManager.Instance.OnVesselDecorationChange -= OnVesselDecorationChange;
		VesselManager.Instance.OnVesselChange -= OnVesselChange;
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
		VesselManager.Instance.OnVesselDecorationChange += OnVesselDecorationChange;
		VesselManager.Instance.OnVesselChange += OnVesselChange;

		if(UserManager.Instance.User.Player.GroupType == IsolatedIslandGame.Protocol.GroupType.No)
		{
			//SceneManager.LoadScene("RegisterScene");
			UImanager.Instance.LoadResult(1);

			//Create Charater by Uimanager ? (probably
			UserManager.Instance.User.Player.OperationManager.CreateCharacter("ABC","signature",IsolatedIslandGame.Protocol.GroupType.B);
		}
		else
		{
			UImanager.Instance.LoadResult(0);
			//SceneManager.LoadScene("MainScene");
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
			SystemManager.Instance.OperationManager.FetchDataResolver.FetchAllVessels();
			InstantiateUserGameObject();
			GetPlayerVesselGameObject();

			PlayerController.Instance.gameObject.SetActive(true);
			CameraManager.Instance.ToNearAnchor(PlayerGameObject);

		}

		//TODO 設定完之後再把顯示出遊戲畫面（在這之前可能顯示loading bar
	}

	public void OnPlayerLocationChange(Vector3 position,float eulerAngleY)
	{
		PlayerGameObject.GetComponent<PlayerBehavior>().UpdateLocation(position,eulerAngleY);

		UserManager.Instance.User.Player.OperationManager.UpdateVesselTransform
		(
			position.x,
			position.z,
			eulerAngleY
		);
	}

	void Register(string nickname, string signature, GroupType groupType)
	{
		UserManager.Instance.User.Player.OperationManager.CreateCharacter(nickname,signature,groupType);
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
				
			UserGameObject.Add(vessel.OwnerPlayerID,user);
			VesselIDGameObject.Add(vessel.VesselID,user);
			user.name = "OwnerID: " + vessel.OwnerPlayerID;


			foreach(Decoration decoration in vessel.Decorations)
			{	
				
				GameObject dec = Instantiate(
					ElementModel[decoration.Material.MaterialID],
					user.transform
				) as GameObject;

				dec.transform.localPosition = new Vector3
					(
						decoration.PositionX,
						decoration.PositionY,
						decoration.PositionZ
					);
				dec.transform.localEulerAngles = new Vector3
					(
						decoration.RotationEulerAngleX,
						decoration.RotationEulerAngleY,
						decoration.RotationEulerAngleZ
					);

				decorationDic.Add(decoration.DecorationID,dec);
				dec.name = "ID: " + decoration.DecorationID;
			}

			UserDecoration.Add(vessel.OwnerPlayerID,decorationDic);
			VesselDecoration.Add(vessel.VesselID,decorationDic);
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
		print(vesselID + "UPDATED");
		GameObject user;
		VesselIDGameObject.TryGetValue(vesselID,out user);
		user.transform.position = new Vector3(locationX,0f,locationZ);
		user.transform.rotation = Quaternion.Euler(0f,rotationEulerAngleY,0f);
	}

	void OnVesselDecorationChange(int vesselID, Decoration decoration, DataChangeType changeType)
	{ 
		GameObject user;
		VesselIDGameObject.TryGetValue(vesselID,out user);

		if(changeType == DataChangeType.Add)
		{
			if(!user)
				print("ABC");
			GameObject dec = Instantiate(
				ElementModel[decoration.Material.MaterialID],
				user.transform
			) as GameObject;

			dec.transform.localPosition = new Vector3
				(
					decoration.PositionX,
					decoration.PositionY,
					decoration.PositionZ
				);
			dec.transform.localEulerAngles = new Vector3
				(
					decoration.RotationEulerAngleX,
					decoration.RotationEulerAngleY,
					decoration.RotationEulerAngleZ
				);
			dec.name = "ID: " + decoration.DecorationID;

			VesselDecoration[vesselID].Add(decoration.DecorationID,dec);


		}
		else if(changeType == DataChangeType.Remove)
		{
			Destroy(VesselDecoration[vesselID][decoration.DecorationID]); 
		}
		else if(changeType == DataChangeType.Update)
		{
			GameObject dec;
			dec = VesselDecoration[vesselID][decoration.DecorationID];
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
	}//當船上的裝飾物更新時的回調事件

	void OnVesselChange (Vessel vessel, DataChangeType changeType)
	{
		if(changeType == DataChangeType.Add)
		{
			Dictionary<int,GameObject> decorationDic = new Dictionary<int, GameObject>();

			GameObject user = Instantiate(
				DefaultShipModel,
				new Vector3(vessel.LocationX,0f,vessel.LocationZ),
				Quaternion.Euler(0f,vessel.RotationEulerAngleY,0f)
			) as GameObject;

			UserGameObject.Add(vessel.OwnerPlayerID,user);
			VesselIDGameObject.Add(vessel.VesselID,user);
			user.name = "OwnerID: " + vessel.OwnerPlayerID;

			foreach(Decoration decoration in vessel.Decorations)
			{	
				GameObject dec = Instantiate(
					ElementModel[decoration.Material.MaterialID],
					user.transform
				) as GameObject;

				dec.transform.localPosition = new Vector3
					(
						decoration.PositionX,
						decoration.PositionY,
						decoration.PositionZ
					);
				dec.transform.localEulerAngles = new Vector3
					(
						decoration.RotationEulerAngleX,
						decoration.RotationEulerAngleY,
						decoration.RotationEulerAngleZ
					);

				decorationDic.Add(decoration.DecorationID,dec);
				dec.name = "ID: " + decoration.DecorationID;
			}


			UserDecoration.Add(vessel.OwnerPlayerID,decorationDic);
			VesselDecoration.Add(vessel.VesselID,decorationDic);

		}
		else if(changeType == DataChangeType.Remove)
		{
			GameObject user;
			if(VesselIDGameObject.TryGetValue(vessel.VesselID, out user))
			{

				UserGameObject.Remove(vessel.OwnerPlayerID);
				VesselIDGameObject.Remove(vessel.VesselID);
				UserDecoration.Remove(vessel.OwnerPlayerID);
				VesselDecoration.Remove(vessel.VesselID);

				Destroy(user);
			}
		}
		else
		{
			Debug.LogError("OnVesselChange ChangeType = Update");
		}

	} //當船物件有變化時的回調事件

	#endregion

	internal void InsVesselGO()
	{
		
	}

	void OnGUI()
    {
        if(UserManager.Instance.User.Player != null && UserManager.Instance.User.Player.Inventory != null)
        {
            foreach (InventoryItemInfo info in UserManager.Instance.User.Player.Inventory.ItemInfos)
            {
                GUILayout.Label(info.Item.ItemName + " : " + info.Count + " ID: " + info.Item.ItemID);
            }
            foreach(Vessel vessel in VesselManager.Instance.Vessels)
            {
                GUILayout.Label(string.Format("VesselName: {0}", vessel.Name));
                foreach (Decoration decoration in vessel.Decorations)
                {
                    GUILayout.Label(string.Format("DecorationID: {0}, MaterialName: {1}", decoration.DecorationID, decoration.Material.ItemName));
                }
            }
        }
	}

	int GroupTypeToInt(GroupType type)
	{
		if(type == GroupType.A)
		{
			return 1;
		}
		if(type == GroupType.B)
		{
			return 2;
		}
		if(type == GroupType.C)
		{
			return 3;
		}

		return 0;
	}
}
