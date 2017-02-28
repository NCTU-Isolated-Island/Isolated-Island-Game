using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuxCameraSystem : MonoBehaviour {

	public static AuxCameraSystem Instance;
	public Camera AuxCamera;
	public float RotationSpeed = 30;

	private bool hasModel = false;
	private GameObject model;


	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}

		AuxCamera = GetComponentInChildren<Camera>();

	}

	void Update()
	{
		if(hasModel)
		{
			model.transform.Rotate(0,RotationSpeed * Time.deltaTime,0);
		}
	}

	public void UnShow()
	{
		hasModel = false;
		Destroy(model);
		AuxCamera.enabled = false;
	}

	public void ShowPlayerGameObject(GameObject player, Vector3 modelPosition, Quaternion cameraRotation)
	{
		AuxCamera.enabled = true;

		this.model = Instantiate(player,this.transform) as GameObject;
		if(player == PlayerController.Instance.CurrentFocusPlayerGameObject)
		{
			if(this.model.transform.GetComponentInChildren<Camera>() != null)
			{
				Destroy(this.model.GetComponentInChildren<Camera>().gameObject);
			}
		}
		this.model.GetComponent<PlayerBehavior>().enabled = false;
		Destroy(this.model.transform.Find("CameraAnchor").gameObject); 
		this.model.transform.localPosition = modelPosition;
		this.model.layer = LayerMask.NameToLayer("SecondCamera");
		AuxCamera.transform.localRotation = cameraRotation;
		hasModel = true;
	}

	public void ShowIsland()
	{
		AuxCamera.enabled = true;
		
		GameObject island = Instantiate(Resources.Load<GameObject> ("IsolatedIsland"),this.transform) as GameObject;

		model = island;
		this.model.layer = LayerMask.NameToLayer ("SecondCamera");
		this.model.transform.localPosition = new Vector3 (0, -2.2f, 4.62f);
		AuxCamera.transform.localRotation = Quaternion.Euler (15f, 0, 0);
		hasModel = true;
	}
}
