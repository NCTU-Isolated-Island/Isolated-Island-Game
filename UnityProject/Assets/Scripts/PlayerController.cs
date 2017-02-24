using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	private float lastTimeClick = -99f ;

	public static PlayerController Instance;
	public List<GameObject> InArea;
	public GameObject CurrentFocusPlayerGameObject;


	public enum ViewMode{ FirstPerson, BirdView, NormalView }
	public ViewMode CurrenViewMode = ViewMode.NormalView;

	float clickTime = -99f;


	public delegate void PlayerAction();
	public static event PlayerAction OnGetArea;

	public bool InSelectMode = false;
	float lastDecorationClick;
	int SelectDecorationID;

	void Awake()
	{
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}

		Input.gyro.enabled = true;

		DontDestroyOnLoad(gameObject);
		//After Setting Up, Deactivate PlayerController, and wait for into MainScene
		gameObject.SetActive(false);



	}

	void Update()
	{

		if(PlayerDecorationManager.Instance.CurrentControlMode == PlayerDecorationManager.ControlMode.Normal)
		{
			if(CurrenViewMode != ViewMode.FirstPerson && !CameraManager.Instance.using_cor)
			{
				AdjustViewAngle();

			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			int userID = 24;

			AuxCameraSystem.Instance.ShowPlayerGameObject(GameManager.Instance.UserGameObject[userID],
				new Vector3(0,-10.3f,40.8f),Quaternion.Euler(12.8f,0,0));
		}

		if(EventSystem.current.currentSelectedGameObject == null)
		{
			CheckDoubleClick();
		}


		PinchToZoom();
	}


	public IEnumerator GetCurrentArea()
	{
		PlayerController.Instance.InArea.Clear(); // 清除上次掃描記錄
		GameObject probe = Instantiate
			(
				Resources.Load("AreaProbe"), GameManager.Instance.PlayerGameObject.transform.position, Quaternion.identity
			) as GameObject;

		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();

		Destroy(probe);

		if(OnGetArea != null)
			OnGetArea.Invoke();
	}

//	public void StartPlaceDecoration()
//	{
//		//Check have that material
//		finishPlacing = false;
//		StartCoroutine(PlaceMaterial(3));
//
//	}
//
//	public void FinishPlaceDecoration()
//	{
//		finishPlacing = true;
//	}
//
//	IEnumerator PlaceMaterial(int itemID)
//	{
//		placingMaterial = true;
//
//		Vector3 position;
//		Quaternion rotation;
//
//		GameObject temp = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity) as GameObject;
//		temp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform);
//		Transparentize(temp);
//
//		RaycastHit hitInfo = new RaycastHit();
//
//		while(!finishPlacing)
//		{
//			//TODO 必須把Player模型設定成 "PlayerModel" Layer
//			Physics.Raycast
//			(
//				Camera.main.ScreenPointToRay(Input.mousePosition),
//				out hitInfo,
//				99999f,
//				LayerMask.GetMask("PlayerModel")
//			);
//			temp.transform.position = hitInfo.point;
//			yield return null;}
//
//
//		position = temp.transform.localPosition;
//		rotation = temp.transform.localRotation;
//
//		UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
//		(
//			itemID,
//			position.x, position.y, position.z,
//			rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z
//		);
//
//
//		Destroy(temp);
//		placingMaterial = false;
//
//	}

	void CheckDoubleClick()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if((Time.time - lastTimeClick) < 0.25f) 
			{
				//Player has double clicked
				CheckIfSelectVessel();

			}	

			lastTimeClick = Time.time;
		}
	}

	void CheckIfSelectVessel()
	{
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(
			Camera.main.ScreenPointToRay(Input.mousePosition),
			out hitInfo,
			99999f,
			LayerMask.GetMask("PlayerModel")
		);

        //Select Vessel
        if (hit && !GameObject.Equals(hitInfo.transform.root.gameObject, GameManager.Instance.PlayerGameObject))
        {
            print("Select " + hitInfo.collider.transform.root.name + " Vessel");

            CameraManager.Instance.ToNearAnchor(hitInfo.transform.root.gameObject);


            string[] a = hitInfo.transform.root.name.Split(' ');
            int id = System.Int32.Parse(a[1]);

            CurrentFocusPlayerGameObject = GameManager.Instance.UserGameObject[id];
            //
            UIManager.Instance.SwapPage(UIManager.UIPageType.OtherBoat);
            OtherBoatUIManager.Instance.SetOtherPlayerInfo(id);
            //
        }

	}

	public void RemoveDecoration(GameObject target)
	{
		UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(target.name));
	}

	void AdjustViewAngle()
	{

		if(Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			float x = touch.deltaPosition.x * 0.15f;
			float y = touch.deltaPosition.y * -0.1f;

#if UNITY_ANDROID
            x *= 4.5f;
            y *= 4.5f;
#endif

            CameraManager.Instance.CameraRotate(x);
			CameraManager.Instance.CameraRotateHorizontal(y);
		}

	}

	public void ToPlayerFarAnchor()
	{
		CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
		CurrentFocusPlayerGameObject = GameManager.Instance.PlayerGameObject;
	}

	public void ToPlayerNearAnchor()
	{
		CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
		CurrentFocusPlayerGameObject = GameManager.Instance.PlayerGameObject;

	}

//	void SelectDecoration()
//	{
//
//		RaycastHit hitInfo = new RaycastHit();
//
//		bool hit = Physics.Raycast(
//			Camera.main.ScreenPointToRay(Input.mousePosition),
//			out hitInfo,
//			99999f,
//			LayerMask.GetMask("Decoration")
//		);
//
//		if(hit)
//		{
//
//			if(CurrentSelectDecoration)
//			{
//				DeTransparentize(CurrentSelectDecoration);
//			}
//
//
//			CurrentSelectDecoration = hitInfo.transform.gameObject;
//
//			Transparentize(CurrentSelectDecoration);
//
//		}
//		else
//		{
//			//CurrentSelectDecoration = null;
//
//
//		}
//
//
//	}

	void PinchToZoom()
	{
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;



            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
#if UNITY_ANDROID
            deltaMagnitudeDiff *= 4.5f;
#endif

			CameraManager.Instance.Zoom((deltaMagnitudeDiff * 0.001f) + 1);


		}
	}

	public void RemoveAllDecoration()
	{
		Vessel vessel;
		VesselManager.Instance.FindVesselByOwnerPlayerID(UserManager.Instance.User.Player.PlayerID,out vessel);

		foreach(Decoration entry in vessel.Decorations)
		{
			UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(entry.DecorationID);
		}


	}

	void Transparentize(GameObject target)
	{

		foreach(UnityEngine.Material entry in target.GetComponent<MeshRenderer>().materials)
		{
			entry.color = new Color( entry.color.r, entry.color.g, entry.color.b, 0.5f);
		}
	}

	void DeTransparentize(GameObject target)
	{
		foreach(UnityEngine.Material entry in target.GetComponent<MeshRenderer>().materials)
		{
			entry.color = new Color( entry.color.r, entry.color.g, entry.color.b, 1f);
		}
	}





	void ShowVessel(GameObject vessel)
	{
		vessel.transform.Find("ShipModel").gameObject.SetActive(true);
		vessel.transform.Find("Decorations").gameObject.SetActive(true);
	}

	void UnShowVessel(GameObject vessel)
	{
		vessel.transform.Find("ShipModel").gameObject.SetActive(false);
		vessel.transform.Find("Decorations").gameObject.SetActive(false);
	}

	public void OnlyShowFriendsVessel()
	{
		List<int> friendsID = new List<int>();
		friendsID.Add(UserManager.Instance.User.Player.PlayerID);
		foreach(FriendInformation entry in UserManager.Instance.User.Player.FriendInformations)
		{
			friendsID.Add(entry.friendPlayerID);
		}

		foreach(KeyValuePair<int,GameObject> entry in GameManager.Instance.UserGameObject)
		{
			if(!friendsID.Contains(entry.Key))
			{
				UnShowVessel(entry.Value);
			}	
		}

	}

	public void ShowAllVessel()
	{

		foreach(GameObject entry in GameManager.Instance.UserGameObject.Values)
		{
			ShowVessel(entry);	
		}

	}

	public void ChangeViewMode(ViewMode mode)
	{
		  
		switch (mode) {
		case ViewMode.FirstPerson:
			//Maybe need CameraManager.to first person anchor
			CameraManager.Instance.ToFirstPerson(GameManager.Instance.PlayerGameObject);
			//Camera.main.transform.position = GameManager.Instance.PlayerGameObject.transform.Find("FirstPersonAnchor").position;
			FirstPersonCameraController.Instance.enabled = true;
			CurrenViewMode = ViewMode.FirstPerson;
			break;

		case ViewMode.NormalView:
			CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
			FirstPersonCameraController.Instance.enabled = false;
			CurrenViewMode = ViewMode.NormalView;
			break;

		case ViewMode.BirdView:
			CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
			FirstPersonCameraController.Instance.enabled = false;
			CurrenViewMode = ViewMode.BirdView;
			break;
		default:
			break;
		}

	}



}

