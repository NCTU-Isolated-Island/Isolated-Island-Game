using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using IsolatedIslandGame.Library;

public class Test : MonoBehaviour {




	void Start()
	{
		
	}

	public bool InSelectMode;

	void Update()
	{
		
		if(Input.GetMouseButtonDown(0))
		{
			// check if click decoration
			lastTimeClick = Time.time;
			InSelectMode = true;
		}


		if(InSelectMode)
		{
			if(Input.GetMouseButtonUp(0))
			{
				InSelectMode = false;
			}

			if(Time.time - lastTimeClick > 2)
			{
				InSelectMode = false;
				//Decorate

			}
		}



	}

	float lastTimeClick = -99f;

	void CheckDoubleClick()
	{
		


	}


	public bool HaveEnoughElement(Blueprint blueprint)
	{
		bool result = true;
		foreach(Blueprint.ElementInfo elementInfo in blueprint.Requirements)
		{
			if(elementInfo.itemCount > UserManager.Instance.User.Player.Inventory.ItemCount(elementInfo.itemID))
				result = false;
		}
		return result;
	}


}
