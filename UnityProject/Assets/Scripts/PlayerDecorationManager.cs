using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IsolatedIslandGame.Client;
using IsolatedIslandGame.Client.Communication;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol;
using UnityEngine.EventSystems;

public class PlayerDecorationManager : MonoBehaviour {

	public static PlayerDecorationManager Instance;

	public GameObject CurrentSelectDecoration;

	public enum ControlMode{ Normal, Decorate, Rotate}
	public ControlMode CurrentControlMode = ControlMode.Normal;

	[HideInInspector]public int ModelOrientMode = 1;

	bool InSelectMode = false;
	float DecorationClickTime;
	float ShortClickTime = -99f;
	int SelectDecorationID;

	public List<GameObject> ModifiedDecorations = new List<GameObject>();
	public List<GameObject> AddedDecorations = new List<GameObject>();
	public List<GameObject> RemovedDecorations = new List<GameObject>();

	void Awake () {
		if(Instance  == null)
			Instance = this;
		
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			UpdateModifiedDecorationsToServer();
		}

		if(CurrentControlMode == ControlMode.Rotate && CurrentSelectDecoration != null)
		{
			if(Input.touchCount == 1)
			{
				RotateArountY(Input.touches[0].deltaPosition.x * -0.2f);

			}
		}

		if(EventSystem.current.currentSelectedGameObject == null)
		{
			if(CurrentControlMode == ControlMode.Decorate || CurrentControlMode == ControlMode.Rotate)
			{
				DecorateProcess();
			}
		}

		if(InSelectMode)
		{
			if(Input.GetMouseButtonUp(0))
			{
				InSelectMode = false;
			}

			if(Time.time - DecorationClickTime > 1.3f)
			{
				InSelectMode = false;
				//Decorate

				UIManager.Instance.RemoveCurrentPage();
				UIManager.Instance.SwapPage(UIManager.UIPageType.PutItem);
				BeginDecorate(SelectDecorationID);

			}
		}
	}

	public void ClickDecoration(int Id)
	{
		DecorationClickTime = Time.time;
		InSelectMode = true;
		SelectDecorationID = Id;
	}


	public void RotateArountY(float degree)
	{
		CurrentSelectDecoration.transform.Rotate(0,degree,0,Space.World);
	}

	public void BeginDecorate(int itemID)
	{
		itemID = 2;
		print("BEgin");

		CurrentSelectDecoration = Instantiate(GameManager.Instance.elementModels[itemID],Vector3.zero,Quaternion.identity,
			GameManager.Instance.PlayerGameObject.transform) as GameObject;
		CurrentSelectDecoration.name = itemID.ToString();

		if(!AddedDecorations.Contains(CurrentSelectDecoration))
			AddedDecorations.Add(CurrentSelectDecoration);

		CurrentControlMode = ControlMode.Decorate;

	}

	public void UpdateModifiedDecorationsToServer()
	{
		foreach(GameObject entry in AddedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.AddDecorationToVessel
			(
				System.Int32.Parse(entry.name),
				entry.transform.localPosition.x, entry.transform.localPosition.y, entry.transform.localPosition.z,
				entry.transform.localRotation.eulerAngles.x, entry.transform.localRotation.eulerAngles.y, entry.transform.localRotation.eulerAngles.z
			);

		}

		foreach(GameObject entry in ModifiedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.UpdateDecorationOnVessel
			(
				System.Int32.Parse(entry.name),
				entry.transform.localPosition.x, entry.transform.localPosition.y, entry.transform.localPosition.z,
				entry.transform.localRotation.eulerAngles.x, entry.transform.localRotation.eulerAngles.y, entry.transform.localRotation.eulerAngles.z
			);
		}

		foreach(GameObject entry in RemovedDecorations)
		{
			UserManager.Instance.User.Player.OperationManager.RemoveDecorationFromVessel(System.Int32.Parse(entry.name));
		}

		print("Update");

		//Clear All 
		ModifiedDecorations.Clear();
		AddedDecorations.Clear();
		RemovedDecorations.Clear();

		foreach(GameObject entry in AddedDecorations)
		{
			Destroy(entry);
		}

	}

	void DecorateProcess()
	{
		if(Input.touchCount == 1)
		{
			#region HitRaycaster
			bool entireHit;
			RaycastHit entireHitInfo = new RaycastHit();

			entireHit = Physics.Raycast
				(
					Camera.main.ScreenPointToRay(Input.mousePosition),
					out entireHitInfo,
					99999f
				);

			bool vesselHit;
			RaycastHit vesselHitInfo = new RaycastHit();
			vesselHit = Physics.Raycast
				(
					Camera.main.ScreenPointToRay(Input.mousePosition),
					out vesselHitInfo,
					99999f,
					LayerMask.GetMask("PlayerModel")
				);
			#endregion

			if(Input.touches[0].phase == TouchPhase.Began)
			{
				ShortClickTime = Time.time;


				if(entireHit && entireHitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Decoration"))
				{
					print(entireHitInfo.transform.name);

					CurrentSelectDecoration = entireHitInfo.transform.gameObject;
					CurrentControlMode = ControlMode.Decorate;
					if(!ModifiedDecorations.Contains(CurrentSelectDecoration))
						ModifiedDecorations.Add(CurrentSelectDecoration);
				}

			}

			if(Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved)
			{

			}

			if(Input.touches[0].phase == TouchPhase.Ended)
			{
				float deltaTime = Time.time - ShortClickTime;
				ShortClickTime = -99f;


				if(deltaTime < 0.3f) // 短按
				{
					CurrentControlMode = ControlMode.Rotate;
				}
				else // 長按
				{
					if(CurrentControlMode == ControlMode.Decorate)
					{
						if(vesselHit)
						{
							CurrentSelectDecoration = null;
						}
						else
						{
							if(!RemovedDecorations.Contains(CurrentSelectDecoration))
								RemovedDecorations.Add(CurrentSelectDecoration);

							CurrentSelectDecoration.SetActive(false);

							CurrentSelectDecoration = null;
						}
					}

				}



			}

			//Move Item
			if(CurrentSelectDecoration != null && CurrentControlMode == ControlMode.Decorate)
			{
				if(Time.time - ShortClickTime > 0.3f)
				{
					if(vesselHit)
					{
						CurrentSelectDecoration.transform.position = vesselHitInfo.point;
					}
					else
					{
						CurrentSelectDecoration.transform.position = 
							Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction * 9;
					}
				}
			}
		}

		if(Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			float x = (touchZero.deltaPosition.x + touchOne.deltaPosition.x) * 0.075f;
			float y = (touchZero.deltaPosition.x + touchOne.deltaPosition.x) * -0.05f;

			CameraManager.Instance.CameraRotate(x);
			CameraManager.Instance.CameraRotateHorizontal(y);

		}
	}

	public void ChangeModelOrientation()
	{
		if(CurrentSelectDecoration == null)
			return;

		switch (ModelOrientMode) {
		case 1:
			CurrentSelectDecoration.transform.localRotation = Quaternion.Euler(0,0,0);
			ModelOrientMode = 2;
			break;
		case 2:
			CurrentSelectDecoration.transform.localRotation = Quaternion.Euler(180,0,0);
			ModelOrientMode = 3;
			break;
		case 3:
			CurrentSelectDecoration.transform.localRotation = Quaternion.Euler(0,0,90);
			ModelOrientMode = 4;
			break;
		case 4:
			CurrentSelectDecoration.transform.localRotation = Quaternion.Euler(0,0,-90);
			ModelOrientMode = 5;
			break;
		case 5:
			CurrentSelectDecoration.transform.localRotation = Quaternion.Euler(90,0,0);
			ModelOrientMode = 6;
			break;
		case 6:
			CurrentSelectDecoration.transform.localRotation = Quaternion.Euler(-90,0,0);
			ModelOrientMode = 1;
			break;
		default:
			break;
		}
	}

}
