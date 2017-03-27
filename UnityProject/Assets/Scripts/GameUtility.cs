using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtility : MonoBehaviour {

	public static GameUtility Instance;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

	}


	public void LoadItemGameObject()
	{
		GameManager.Instance.ElementModels.Add(   1, Resources.Load("Ingredients/" + "trash") as GameObject);

		GameManager.Instance.ElementModels.Add(1005, Resources.Load("Ingredients/" + "stone") as GameObject);
		GameManager.Instance.ElementModels.Add(1006, Resources.Load("Ingredients/" + "Wheat") as GameObject);
		GameManager.Instance.ElementModels.Add(1007, Resources.Load("Ingredients/" + "Egg") as GameObject);
		GameManager.Instance.ElementModels.Add(1008, Resources.Load("Ingredients/" + "CoffeeBean") as GameObject);
		GameManager.Instance.ElementModels.Add(1009, Resources.Load("Ingredients/" + "pineapple") as GameObject);
		GameManager.Instance.ElementModels.Add(1010, Resources.Load("Ingredients/" + "apple") as GameObject);
		GameManager.Instance.ElementModels.Add(1011, Resources.Load("Ingredients/" + "water") as GameObject);
		GameManager.Instance.ElementModels.Add(1013, Resources.Load("Ingredients/" + "fire") as GameObject);

		GameManager.Instance.ElementModels.Add(1016, Resources.Load("Ingredients/" + "coal") as GameObject);


		GameManager.Instance.ElementModels.Add(1018, Resources.Load("Ingredients/" + "Patato") as GameObject);
		GameManager.Instance.ElementModels.Add(1019, Resources.Load("Ingredients/" + "wood") as GameObject);

		GameManager.Instance.ElementModels.Add(1021, Resources.Load("Ingredients/" + "oil") as GameObject);
		GameManager.Instance.ElementModels.Add(1022, Resources.Load("Ingredients/" + "fish") as GameObject);
		GameManager.Instance.ElementModels.Add(1023, Resources.Load("Ingredients/" + "Sponge") as GameObject);

		GameManager.Instance.ElementModels.Add(2001, Resources.Load("Ingredients/" + "Silicone") as GameObject);
		GameManager.Instance.ElementModels.Add(2002, Resources.Load("Ingredients/" + "Ball") as GameObject);
		GameManager.Instance.ElementModels.Add(2003, Resources.Load("Ingredients/" + "rope") as GameObject);
		GameManager.Instance.ElementModels.Add(2005, Resources.Load("Ingredients/" + "StoneMill") as GameObject);
		GameManager.Instance.ElementModels.Add(2006, Resources.Load("Ingredients/" + "milk") as GameObject);
		GameManager.Instance.ElementModels.Add(2007, Resources.Load("Ingredients/" + "badminton") as GameObject);
		GameManager.Instance.ElementModels.Add(2008, Resources.Load("Ingredients/" + "knife") as GameObject);
		GameManager.Instance.ElementModels.Add(2009, Resources.Load("Ingredients/" + "Sugar") as GameObject);

		GameManager.Instance.ElementModels.Add(2011, Resources.Load("Ingredients/" + "pigment") as GameObject);
		GameManager.Instance.ElementModels.Add(2012, Resources.Load("Ingredients/" + "Pot") as GameObject);
		GameManager.Instance.ElementModels.Add(2013, Resources.Load("Ingredients/" + "Flour") as GameObject);
		GameManager.Instance.ElementModels.Add(2014, Resources.Load("Ingredients/" + "pencil") as GameObject);

		GameManager.Instance.ElementModels.Add(2016, Resources.Load("Ingredients/" + "electricity") as GameObject);
		GameManager.Instance.ElementModels.Add(2017, Resources.Load("Ingredients/" + "Propeller") as GameObject);

		GameManager.Instance.ElementModels.Add(2019, Resources.Load("Ingredients/" + "gold") as GameObject);
		GameManager.Instance.ElementModels.Add(2020, Resources.Load("Ingredients/" + "Candle") as GameObject);

		GameManager.Instance.ElementModels.Add(3001, Resources.Load("Ingredients/" + "Soup") as GameObject);
		GameManager.Instance.ElementModels.Add(3002, Resources.Load("Ingredients/" + "steel") as GameObject);
		GameManager.Instance.ElementModels.Add(3003, Resources.Load("Ingredients/" + "Chip") as GameObject);

		GameManager.Instance.ElementModels.Add(3005, Resources.Load("Ingredients/" + "Pillow") as GameObject);

		GameManager.Instance.ElementModels.Add(3008, Resources.Load("Ingredients/" + "Latte") as GameObject);
		GameManager.Instance.ElementModels.Add(3009, Resources.Load("Ingredients/" + "Coke") as GameObject);
		GameManager.Instance.ElementModels.Add(3011, Resources.Load("Ingredients/" + "Painter") as GameObject);
		GameManager.Instance.ElementModels.Add(3012, Resources.Load("Ingredients/" + "guitar") as GameObject);
		GameManager.Instance.ElementModels.Add(3013, Resources.Load("Ingredients/" + "Ghost") as GameObject);
		GameManager.Instance.ElementModels.Add(3014, Resources.Load("Ingredients/" + "small_light") as GameObject);

		GameManager.Instance.ElementModels.Add(3017, Resources.Load("Ingredients/" + "Oven") as GameObject);
		GameManager.Instance.ElementModels.Add(3018, Resources.Load("Ingredients/" + "take_copter_1") as GameObject);
		GameManager.Instance.ElementModels.Add(3019, Resources.Load("Ingredients/" + "Bread") as GameObject);

		GameManager.Instance.ElementModels.Add(4004, Resources.Load("Ingredients/" + "badminton_racket") as GameObject);
		GameManager.Instance.ElementModels.Add(4005, Resources.Load("Ingredients/" + "Computer") as GameObject);
		GameManager.Instance.ElementModels.Add(4006, Resources.Load("Ingredients/" + "Motor") as GameObject);

		GameManager.Instance.ElementModels.Add(4009, Resources.Load("Ingredients/" + "ShineMood") as GameObject);
		GameManager.Instance.ElementModels.Add(4010, Resources.Load("Ingredients/" + "black_horse") as GameObject);

		GameManager.Instance.ElementModels.Add(4013, Resources.Load("Ingredients/" + "imac") as GameObject);


		GameManager.Instance.ElementModels.Add(4016, Resources.Load("Ingredients/" + "Fries") as GameObject);


		// check
		foreach (var element in GameManager.Instance.ElementModels)
		{
			if (element.Value == null)
				print(element.Key);
		}



	}
}
