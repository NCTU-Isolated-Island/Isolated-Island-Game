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

	public List<GameObject> elementModels;
    public List<GameObject> ShipModels;

	private Dictionary<int,GameObject> UserGameObject = new Dictionary<int, GameObject>(); //UserID to GO
    private Dictionary<int,GameObject> VesselIDGameObject = new Dictionary<int, GameObject>(); //VesselID to GO
	public Dictionary<int,Dictionary<int,GameObject>> UserDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // PlayerID to decorationID-decorationGO
	public Dictionary<int,Dictionary<int,GameObject>> VesselDecoration = new Dictionary<int, Dictionary<int,GameObject>>(); // VesselID to decorationID-decorationGO
    public GameObject PlayerGameObject { get; private set; }

    private bool isInMainScene;

    public enum CameraStat
    {
        Near,
        Far
    }
    private CameraStat cameraStat;

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
        FacebookService.LoginWithFacbook();
        UserManager.Instance.User.OnPlayerOnline += OnPlayerOnline;
        SceneManager.sceneLoaded += OnSceneLoaded;

        VesselManager.Instance.OnVesselTransformUpdated += OnVesselTransformUpdated;
        VesselManager.Instance.OnVesselDecorationChange += OnVesselDecorationChange;
        VesselManager.Instance.OnVesselChange += OnVesselChange;
    }
    void OnDestroy()
	{
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

	void OnPlayerOnline(Player player)
	{
		UserManager.Instance.User.Player.OnCreateCharacter += OnCreateCharacter;

        if (UserManager.Instance.User.Player.GroupType == GroupType.No)
		{
			//SceneManager.LoadScene("RegisterScene");
			UImanager.Instance.LoadResult(1);

			//Create Charater by Uimanager ? (probably
			UserManager.Instance.User.Player.OperationManager.CreateCharacter("ABC","signature", GroupType.B);
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

	#region Vessel

	void OnVesselTransformUpdated(int vesselID, float locationX, float locationZ, float rotationEulerAngleY)
	{
        if(isInMainScene)
        {
            print(vesselID + "UPDATED");
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
                            decorationGameObject.name = string.Format("ID: {0}", decoration.DecorationID);
                            if (!VesselDecoration[vesselID].ContainsKey(decoration.DecorationID))
                            {
                                VesselDecoration[vesselID].Add(decoration.DecorationID, decorationGameObject);
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
                                Destroy(VesselDecoration[vesselID][decoration.DecorationID]);
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

						int groupType;
						switch (vessel.PlayerInformation.groupType) 
						{
						case GroupType.A:
							groupType = 1;
							break;
						case GroupType.B:
							groupType = 2;
							break;
						case GroupType.C:
							groupType = 3;
							break;
						default:
							groupType = 0;
							break;
						}
						
                        GameObject userVesselGameObject = Instantiate
						(
							ShipModels[groupType],
                            new Vector3(vessel.LocationX, 0f, vessel.LocationZ),
                            Quaternion.Euler(0f, vessel.RotationEulerAngleY, 0f)
                        ) as GameObject;
                        userVesselGameObject.name = string.Format("OwnerID: {0}", vessel.PlayerInformation.playerID);

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
                                decorationGameObject.name = "ID: " + decoration.DecorationID;

                                decorationDictionary.Add(decoration.DecorationID, decorationGameObject);
                            }
                        }
                        if (!UserGameObject.ContainsKey(vessel.PlayerInformation.playerID))
                            UserGameObject.Add(vessel.PlayerInformation.playerID, userVesselGameObject);
                        if (!VesselIDGameObject.ContainsKey(vessel.VesselID))
                            VesselIDGameObject.Add(vessel.VesselID, userVesselGameObject);
                        if (!UserDecoration.ContainsKey(vessel.PlayerInformation.playerID))
                            UserDecoration.Add(vessel.PlayerInformation.playerID, decorationDictionary);
                        if (!VesselDecoration.ContainsKey(vessel.VesselID))
                            VesselDecoration.Add(vessel.VesselID, decorationDictionary);
                    }
                    break;
                case DataChangeType.Remove:
                    {
                        GameObject userVesselGameObject;
                        if (VesselIDGameObject.TryGetValue(vessel.VesselID, out userVesselGameObject))
                        {
                            UserGameObject.Remove(vessel.PlayerInformation.playerID);
                            VesselIDGameObject.Remove(vessel.VesselID);
                            UserDecoration.Remove(vessel.PlayerInformation.playerID);
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
                GUILayout.Label(string.Format("VesselName: {0}", vessel.PlayerInformation.nickname));
                foreach (Decoration decoration in vessel.Decorations)
                {
                    GUILayout.Label(string.Format("DecorationID: {0}, MaterialName: {1}", decoration.DecorationID, decoration.Material.ItemName));
                }
            }
        }
	}
}
