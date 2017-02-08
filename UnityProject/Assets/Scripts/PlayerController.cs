using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;

public class PlayerController : MonoBehaviour {

	private bool finishPlacing = false; // 完成放置素材
	private bool placingMaterial = false; //正在放置素材
	private float lastTimeClick = -99f ;
	private bool doneDecorating = false;

	public static PlayerController Instance;
	public List<GameObject> InArea;
	public GameObject CurrentSelectDecoration;

	public enum State { Default, Move, Rotate }
	public State CurrentState = State.Default;
	public int ModelOrientMode = 0;
	public enum ViewMode{ FirstPerson, BirdView, NormalView }
	public ViewMode CurrenViewMode = ViewMode.NormalView;

	public delegate void PlayerAction();
	public static event PlayerAction OnGetArea;

	void Awake()
	{
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		Input.gyro.enabled = true;

		//After Setting Up, Deactivate PlayerController, and wait for into MainScene
		gameObject.SetActive(false);



	}

	void Update()
	{
		

	
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			ChangeViewMode(ViewMode.BirdView);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			ChangeViewMode(ViewMode.FirstPerson);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			ChangeViewMode(ViewMode.NormalView);
		}
			


		CheckDoubleClick();

		if(CurrentState == State.Default)
		{
			AdjustViewAngle();
			PinchToZoom();
		}

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

	public void StartPlaceDecoration()
	{
		//Check have that material
		finishPlacing = false;
		StartCoroutine(PlaceMaterial(3));

	}

	public void FinishPlaceDecoration()
	{
		finishPlacing = true;
	}

	IEnumerator PlaceMaterial(int itemID)
	{
		placingMaterial = true;

		Vector3 position;
		Quaternion rotation;

		GameObject temp = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity) as GameObject;
		temp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform);
		Transparentize(temp);

		RaycastHit hitInfo = new RaycastHit();

		while(!finishPlacing)
		{
			//TODO 必須把Player模型設定成 "PlayerModel" Layer
			Physics.Raycast
			(
				Camera.main.ScreenPointToRay(Input.mousePosition),
				out hitInfo,
				99999f,
				LayerMask.GetMask("PlayerModel")
			);
			temp.transform.position = hitInfo.point;
			yield return null;}


		position = temp.transform.localPosition;
		rotation = temp.transform.localRotation;

		UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
		(
			itemID,
			position.x, position.y, position.z,
			rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z
		);


		Destroy(temp);
		placingMaterial = false;

	}

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
		if(hit && !GameObject.Equals(hitInfo.transform.root.gameObject,GameManager.Instance.PlayerGameObject))
		{
			print("Select " + hitInfo.collider.transform.root.name + " Vessel");

			CameraManager.Instance.ToNearAnchor(hitInfo.transform.root.gameObject);
			UImanager.Instance.GameUI = UImanager.UI.Other_Boat;
		}

	}

	public void RemoveDecoration(GameObject target)
	{
		UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(target.name));
	}

	void AdjustViewAngle()
	{

		if(Input.touchCount == 1 && !placingMaterial)
		{
			Touch touch = Input.GetTouch(0);
			float x = touch.deltaPosition.x * 0.15f;
			float y = touch.deltaPosition.y * -0.1f;

			CameraManager.Instance.CameraRotate(x);
			CameraManager.Instance.CameraRotateHorizontal(y);
		}

	}

	public void ToPlayerFarAnchor()
	{
		CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
		UImanager.Instance.GameUI = UImanager.UI.Map;
	}

	public void ToPlayerNearAnchor()
	{
		CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
		UImanager.Instance.GameUI = UImanager.UI.Main_Boat;
	}

	void SelectDecoration()
	{

		RaycastHit hitInfo = new RaycastHit();

		bool hit = Physics.Raycast(
			Camera.main.ScreenPointToRay(Input.mousePosition),
			out hitInfo,
			99999f,
			LayerMask.GetMask("Decoration")
		);

		if(hit)
		{

			if(CurrentSelectDecoration)
			{
				DeTransparentize(CurrentSelectDecoration);
			}


			CurrentSelectDecoration = hitInfo.transform.gameObject;

			Transparentize(CurrentSelectDecoration);

		}
		else
		{
			//CurrentSelectDecoration = null;


		}


	}

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

	//
	//	//Still Have Bugs
	//	void TwoFingersRotate()
	//	{
	//		if(Input.touchCount == 2)
	//		{
	//			Touch touchZero = Input.GetTouch(0);
	//			Touch touchOne = Input.GetTouch(1);
	//
	//			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
	//			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
	//
	//			Vector2 previousVector = (touchZeroPrevPos - touchOnePrevPos);
	//			Vector2 currentVector = (touchZero.position - touchOne.position);
	//
	//			float rotationAngle = Mathf.Acos(Vector2.Dot(previousVector,currentVector) / (previousVector.magnitude * currentVector.magnitude));
	//			print(rotationAngle);
	//			islandGameObject.transform.Rotate(0f,0f,rotationAngle * 10f,Space.World);
	//		}
	//	}

//	IEnumerator Decorate()
//	{
//		CurrentState = State.Decorating;
//
//		GameObject temp = Instantiate(GameManager.Instance.elementModels[1],Vector3.zero,Quaternion.identity) as GameObject;
//		temp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform);
//
//		RaycastHit hitInfo = new RaycastHit();
//
//		while(!doneDecorating)
//		{
//			if(CurrentState == State.Decorating)
//			{
//				Physics.Raycast
//				(
//					Camera.main.ScreenPointToRay(Input.mousePosition),
//					out hitInfo,
//					99999f,
//					LayerMask.GetMask("PlayerModel")
//				);
//				temp.transform.position = hitInfo.point;
//			}
//			if(CurrentState == State.Rotating)
//			{
//				temp.transform.rotation = Quaternion.Euler(90,0,0) * Quaternion.Euler
//					(
//						Input.gyro.attitude.eulerAngles.x * -1,
//						Input.gyro.attitude.eulerAngles.y * -1,
//						Input.gyro.attitude.eulerAngles.z
//					) * Quaternion.Euler
//					(
//						Input.gyro.attitude.eulerAngles.x * -1,
//						Input.gyro.attitude.eulerAngles.y * -1,
//						Input.gyro.attitude.eulerAngles.z
//					);
//			}
//
//			yield return null;
//
//		}
//
//		print("DONE");
//		CurrentState = State.Default;
//		//finalize
//	}
//
	public void ChangeModelOrientation()
	{
		switch (ModelOrientMode) {
		case 1:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(0,0,0);
			ModelOrientMode = 2;
			break;
		case 2:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(180,0,0);
			ModelOrientMode = 3;
			break;
		case 3:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(0,0,90);
			ModelOrientMode = 4;
			break;
		case 4:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(0,0,-90);
			ModelOrientMode = 5;
			break;
		case 5:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(90,0,0);
			ModelOrientMode = 6;
			break;
		case 6:
			CurrentSelectDecoration.transform.rotation = Quaternion.Euler(-90,0,0);
			ModelOrientMode = 1;
			break;
		default:
			break;
		}
	}

	public void RotateArountY(float degree)
	{
		CurrentSelectDecoration.transform.Rotate(0,degree,0,Space.World);
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
			Camera.main.transform.position = GameManager.Instance.PlayerGameObject.transform.Find("FirstPersonAnchor").position;
			FirstPersonCameraController.Instance.enabled = true;
			CurrenViewMode = ViewMode.FirstPerson;
			break;

		case ViewMode.NormalView:
			CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
			CurrenViewMode = ViewMode.NormalView;
			break;

		case ViewMode.BirdView:
			CameraManager.Instance.ToFarAnchor(GameManager.Instance.PlayerGameObject);
			CurrenViewMode = ViewMode.BirdView;
			break;
		default:
			break;
		}

	}

	IEnumerator something(int itemID)
	{

		bool hit;
		RaycastHit hitInfo = new RaycastHit();

		GameObject dec = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity) as GameObject;


		// moving mode
		while(true)
		{
			//if change to rotate mode

			hit = Physics.Raycast
				(
					Camera.main.ScreenPointToRay(Input.mousePosition),
					out hitInfo,
					99999f,
					LayerMask.GetMask("PlayerModel")
				);

			if(hit)
			{
				dec.transform.position = hitInfo.point;
			}
			else
			{
				// not keycode.a is touch up
				if(Input.GetKeyUp(KeyCode.A))
				{
					//Destory Decoration;
					dec.SetActive(false);
					//TODO end this function and call inventory show up 
				}
				else
				{
					dec.transform.position = Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction * 5;
				}

			}



			yield return null;
		}


		// rotate mode

		while(true)
		{
			//if click change rotation
			ChangeModelOrientation();

			if(Input.touchCount == 2)
			{
				// adjust view
			}

			yield return null;
		}

	}

	public List<GameObject> ModifiedDecorations = new List<GameObject>();
	public List<GameObject> AddedDecorations = new List<GameObject>();
	public List<GameObject> RemovedDecorations = new List<GameObject>();

	// Code templete , cant use directorly 
	void AddModifiedDecorations(GameObject entry)
	{
		if(!ModifiedDecorations.Contains(entry))
			ModifiedDecorations.Add(entry);
		if(!RemovedDecorations.Contains(entry))
			RemovedDecorations.Add(entry);
		if(!AddedDecorations.Contains(entry))
			AddedDecorations.Add(entry);
	}

	//Final Stage After Clicking Done Button
	void UpdateModifiedDecorationsToServer()
	{
		foreach(GameObject entry in ModifiedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.UpdateDecorationOnVessel
			(
				System.Int32.Parse(entry.name),
				entry.transform.localPosition.x, entry.transform.localPosition.y, entry.transform.localPosition.z,
				entry.transform.rotation.eulerAngles.x, entry.transform.rotation.eulerAngles.y, entry.transform.rotation.eulerAngles.z
			);
		}

		foreach(GameObject entry in AddedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
			(
				System.Int32.Parse(entry.name),
				entry.transform.localPosition.x, entry.transform.localPosition.y, entry.transform.localPosition.z,
				entry.transform.rotation.eulerAngles.x, entry.transform.rotation.eulerAngles.y, entry.transform.rotation.eulerAngles.z
			);
		}

		foreach(GameObject entry in RemovedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(entry.name));
		}
	}

	public void DecorationFinished()
	{
		
	}



}

