using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationUIManager : MonoBehaviour {

	public static DecorationUIManager Instance { get; private set; }

	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
	}

	//after press two sec on item
	public void Decorate()
	{
		//something


	}

	public void PressRotateButton()
	{
		PlayerController.Instance.ChangeModelOrientation();
	}

	public void PressDone()
	{
		PlayerController.Instance.UpdateModifiedDecorationsToServer();
	}

}
