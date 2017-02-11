using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorationUIManager : MonoBehaviour {

	public static DecorationUIManager Instance { get; private set; }

	public GameObject RotateButton;

	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}

	void Update()
	{
		if(PlayerController.Instance.CurrentControlMode == PlayerController.ControlMode.Rotate)
		{
			RotateButton.SetActive(true);
		}
		else
		{
			RotateButton.SetActive(false);
		}

	}

	//after press two sec on item
	public void Decorate(int itemID)
	{
		PlayerController.Instance.BeginDec(itemID);
	}

	public void PressRotateButton()
	{
		PlayerController.Instance.ChangeModelOrientation();
	}

	public void PressDone()
	{
		PlayerController.Instance.UpdateModifiedDecorationsToServer();
		//Show Inventory or something
	}

}
