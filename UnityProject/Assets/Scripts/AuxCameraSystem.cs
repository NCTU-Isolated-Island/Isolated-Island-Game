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

	public void Show3DObject(GameObject model, Vector3 modelPosition, Quaternion cameraRotation)
	{
		this.model = Instantiate(model,this.transform) as GameObject;
		this.model.transform.localPosition = modelPosition;
		this.model.layer = LayerMask.NameToLayer("SecondCamera");
		AuxCamera.transform.localRotation = cameraRotation;
		hasModel = true;
	}


}
