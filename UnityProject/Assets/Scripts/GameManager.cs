using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }

	public List<GameObject> elementModels; //Using itemID to sort
	public List<GameObject> ShipModels;

	public Dictionary<int,GameObject> UserGameObject = new Dictionary<int, GameObject>(); //UserID to GO
	public Dictionary<int,GameObject> VesselIDGameObject = new Dictionary<int, GameObject>(); //VesselID to GO
	public Dictionary<int,Dictionary<int,GameObject>> UserDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // PlayerID to decorationID-decorationGO
	public Dictionary<int,Dictionary<int,GameObject>> VesselDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // VesselID to decorationID-decorationGO
	public GameObject PlayerGameObject { get; private set; }
	public int PlayerID;

	private bool isInMainScene;

	public enum CameraStat
	{
		Near,
		Far
	}
	private CameraStat cameraStat;

	#region Setup

	void Awake ()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		StartCoroutine(SlowUpdate());

		PhotonService.Instance.OnConnectChange += DebugLogin;
		//FacebookService.LoginWithFacbook();

		UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;
		SceneManager.sceneLoaded += OnSceneLoaded;

		VesselManager.Instance.OnVesselTransformUpdated += OnVesselTransformUpdated;
		VesselManager.Instance.OnVesselDecorationChange += OnVesselDecorationChange;
		VesselManager.Instance.OnVesselChange += OnVesselChange;
	}

	void DebugLogin(bool status)
	{
		if(status == true)
		{
			//TODO need to REMOVE before beta!!!
			UserManager.Instance.User.OperationManager.PlayerIDLogin(22,"TestServer");
		}
	}


	void OnDestroy()
	{
		PhotonService.Instance.OnConnectChange -= DebugLogin;

		UserManager.Instance.User.OnPlayerOnline -= OnPlayerOnline;
		UserManager.Instance.User.Player.OnCreateCharacter -= OnCreateCharacter;
		SceneManager.sceneLoaded -= OnSceneLoaded;
		VesselManager.Instance.OnVesselTransformUpdated -= OnVesselTransformUpdated;
		VesselManager.Instance.OnVesselDecorationChange -= OnVesselDecorationChange;
		VesselManager.Instance.OnVesselChange -= OnVesselChange;
	}

	IEnumerator SlowUpdate()
	{
		while(true)
		{
			yield return new WaitForSeconds(5f);
		}
	}

	#endregion

	void OnPlayerOnline(Player player)
	{
		UserManager.Instance.User.Player.OnCreateCharacter += OnCreateCharacter;

		if (UserManager.Instance.User.Player.GroupType == GroupType.No)
		{
			//SceneManager.LoadScene("RegisterScene");
			UImanager.Instance.LoadResult(1);

			//Create Charater by Uimanager ? (probably
			UserManager.Instance.User.Player.OperationManager.CreateCharacter("ABC","signature", GroupType.Businessman);
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

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if(scene == SceneManager.GetSceneByName("MainScene"))
		{
			isInMainScene = true;

			LocationSystem.Instance.StartLocationService();
			SystemManager.Instance.OperationManager.FetchDataResolver.FetchAllVessels();

			foreach (Vessel vessel in VesselManager.Instance.Vessels)
			{
				OnVesselChange(DataChangeType.Add, vessel);
			}

			GetPlayerVesselGameObject();
			GetPlayerID();

			PlayerController.Instance.gameObject.SetActive(true);
			CameraManager.Instance.ToNearAnchor(PlayerGameObject);
		}
		else
		{
			isInMainScene = false;
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

	void GetPlayerVesselGameObject()
	{
		if(UserGameObject.ContainsKey(UserManager.Instance.User.Player.PlayerID))
		{
			PlayerGameObject = UserGameObject[UserManager.Instance.User.Player.PlayerID];
		}
		else
		{
			Debug.LogError("GetPlayerVesselGameObject Error!");
		}
	}

	void GetPlayerID()
	{
		PlayerID = UserManager.Instance.User.Player.PlayerID;
	}

	#region Vessel

	void OnVesselTransformUpdated(int vesselID, float locationX, float locationZ, float rotationEulerAngleY)
	{
		if(isInMainScene)
		{
			GameObject userVesselGameObject;
			if (VesselIDGameObject.TryGetValue(vesselID, out userVesselGameObject))
			{
				userVesselGameObject.GetComponent<PlayerBehavior>().UpdateLocation
				(
					new Vector3(locationX, 0f, locationZ),
					rotationEulerAngleY
				);
			}
			else
			{
				Debug.LogFormat("Vessel: {0}, Not Existed", vesselID);
			}
		}
	}

	void OnVesselDecorationChange(DataChangeType changeType, int vesselID, Decoration decoration)
	{
		if (isInMainScene)
		{
			GameObject userVesselGameObject;
			if (VesselIDGameObject.TryGetValue(vesselID, out userVesselGameObject) && VesselDecoration.ContainsKey(vesselID))
			{
				switch (changeType)
				{
				case DataChangeType.Add:
					{
						GameObject decorationGameObject = Instantiate(
							elementModels[decoration.Material.ItemID],
							userVesselGameObject.transform
						) as GameObject;

						decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
						decorationGameObject.transform.localEulerAngles = new Vector3(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
						decorationGameObject.name = decoration.DecorationID.ToString();

						if (!VesselDecoration[vesselID].ContainsKey(decoration.DecorationID))
						{
							//Find OwnerPlayerID by VesselID
							Vessel v;
							VesselManager.Instance.FindVessel(vesselID,out v);

							VesselDecoration[vesselID].Add(decoration.DecorationID, decorationGameObject);

							//TODO for some reason UserDecoration[v.PlayerInformation.playerID] already
							// contain key "decoration.DecorationID"

						}
						else
						{
							Debug.LogFormat("Add Decoration to Vessel Fail, Decoration Already Existed, VesselID: {0}, DecorationID: {1}", vesselID, decoration.DecorationID);
						}
					}
					break;
				case DataChangeType.Remove:
					{
						if (VesselDecoration[vesselID].ContainsKey(decoration.DecorationID))
						{
							//Find OwnerPlayerID by VesselID
							Vessel v;
							VesselManager.Instance.FindVessel(vesselID,out v);

							Destroy(VesselDecoration[vesselID][decoration.DecorationID]);

							UserDecoration[v.OwnerPlayerID].Remove(decoration.DecorationID);
							VesselDecoration[vesselID].Remove(decoration.DecorationID);
						}
						else
						{
							Debug.LogFormat("Destroy Decoration Fail Decoration Not On Vessel, VesselID: {0}, DecorationID: {1}", vesselID, decoration.DecorationID);
						}
					}
					break;
				case DataChangeType.Update:
					{
						GameObject decorationGameObject;
						if (VesselDecoration[vesselID].TryGetValue(decoration.DecorationID, out decorationGameObject))
						{
							decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
							decorationGameObject.transform.localRotation = Quaternion.Euler(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
						}
						else
						{
							Debug.LogFormat("Decoration Not On Vessel, VesselID: {0}, DecorationID: {1}", vesselID, decoration.DecorationID);
						}
					}
					break;
				}
			}
			else
			{
				Debug.LogFormat("VesselID: {0}, GameObject Not Existed", vesselID);
			}
		}
	}//當船上的裝飾物更新時的回調事件

	void OnVesselChange(DataChangeType changeType, Vessel vessel)
	{
		if (isInMainScene)
		{
			switch (changeType)
			{
			case DataChangeType.Add:
				{
					Dictionary<int, GameObject> decorationDictionary = new Dictionary<int, GameObject>();

					int groupType = 0;
					PlayerInformation playerInformation;
					if(PlayerInformationManager.Instance.FindPlayerInformation(vessel.OwnerPlayerID, out playerInformation))
					{
						groupType = (int)playerInformation.groupType;
					}

					GameObject userVesselGameObject = Instantiate
						(
							ShipModels[groupType],
							new Vector3(vessel.LocationX, 0f, vessel.LocationZ),
							Quaternion.Euler(0f, vessel.RotationEulerAngleY, 0f)
						) as GameObject;

					userVesselGameObject.name = string.Format("OwnerID: {0}", vessel.OwnerPlayerID);

					foreach (Decoration decoration in vessel.Decorations)
					{
						if (!decorationDictionary.ContainsKey(decoration.DecorationID))
						{
							GameObject decorationGameObject = Instantiate(
								elementModels[decoration.Material.ItemID],
								userVesselGameObject.transform
							) as GameObject;

							decorationGameObject.transform.localPosition = new Vector3(decoration.PositionX, decoration.PositionY, decoration.PositionZ);
							decorationGameObject.transform.localEulerAngles = new Vector3(decoration.RotationEulerAngleX, decoration.RotationEulerAngleY, decoration.RotationEulerAngleZ);
							decorationGameObject.name = decoration.DecorationID.ToString();

							decorationDictionary.Add(decoration.DecorationID, decorationGameObject);
						}
					}
					if (!UserGameObject.ContainsKey(vessel.OwnerPlayerID))
						UserGameObject.Add(vessel.OwnerPlayerID, userVesselGameObject);
					if (!VesselIDGameObject.ContainsKey(vessel.VesselID))
						VesselIDGameObject.Add(vessel.VesselID, userVesselGameObject);
					if (!UserDecoration.ContainsKey(vessel.OwnerPlayerID))
						UserDecoration.Add(vessel.OwnerPlayerID, decorationDictionary);
					if (!VesselDecoration.ContainsKey(vessel.VesselID))
						VesselDecoration.Add(vessel.VesselID, decorationDictionary);
				}
				break;
			case DataChangeType.Remove:
				{
					GameObject userVesselGameObject;
					if (VesselIDGameObject.TryGetValue(vessel.VesselID, out userVesselGameObject))
					{
						UserGameObject.Remove(vessel.OwnerPlayerID);
						VesselIDGameObject.Remove(vessel.VesselID);
						UserDecoration.Remove(vessel.OwnerPlayerID);
						VesselDecoration.Remove(vessel.VesselID);

						Destroy(userVesselGameObject);
					}
				}
				break;
			case DataChangeType.Update:
				{
					Debug.LogError("OnVesselChange ChangeType = Update");
				}
				break;
			}
		}
	} //當船物件有變化時的回調事件

	#endregion


	void OnGUI()
	{
		//        if(UserManager.Instance.User.Player != null && UserManager.Instance.User.Player.Inventory != null)
		//        {
		//            foreach (InventoryItemInfo info in UserManager.Instance.User.Player.Inventory.ItemInfos)
		//            {
		//                GUILayout.Label(info.Item.ItemName + " : " + info.Count + " ID: " + info.Item.ItemID);
		//            }
		//            foreach(Vessel vessel in VesselManager.Instance.Vessels)
		//            {
		//                GUILayout.Label(string.Format("VesselName: {0}", vessel.PlayerInformation.nickname));
		//                foreach (Decoration decoration in vessel.Decorations)
		//                {
		//                    GUILayout.Label(string.Format("DecorationID: {0}, MaterialName: {1}", decoration.DecorationID, decoration.Material.ItemName));
		//                }
		//            }
		//        }
		GUI.contentColor = Color.black;
		foreach (Dictionary<int,GameObject> vessel in UserDecoration.Values)
		{
			foreach (KeyValuePair<int,GameObject> decoration in vessel)
			{
				GUILayout.Label("ID: " + decoration.Key);
			}

			GUILayout.Label("------------");
		}
	}

	void ABC()
	{
		print(one());
	}

	int one()
	{
		return 1;
	}


}
