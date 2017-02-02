using IsolatedIslandGame.Library;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIslandController : MonoBehaviour {

	public static PlayerIslandController Instance;

	void Awake()
	{
		if(Instance == null)
			Instance = this;
	}



	public IEnumerator ContributeIsland(int itemID)
	{
		
		yield return PlayerController.Instance.GetCurrentArea();

		bool containTrigger = false;
		foreach(GameObject entry in PlayerController.Instance.InArea)
		{
			if(entry.name == "IslandTrigger")
			{
				containTrigger = true;
			}
		}

		if(!containTrigger)
			yield return null;

		UserManager.Instance.User.Player.OperationManager.SendMaterialToIsland(itemID);

		//Refresh item list?
	}

}
