using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullBehavior : MonoBehaviour {

	private Vector3 moveDirection;
	private float lifeTime;

	void Start () {
		
		DetermineInitialValue();
		Setup();
	}

	void DetermineInitialValue()
	{
		Vector2 unitCircle = Random.insideUnitCircle;

		moveDirection = new Vector3(unitCircle.x,0,unitCircle.y);
		lifeTime = Random.Range(30,60);
	}

	void Setup()
	{
		transform.rotation = Quaternion.LookRotation(moveDirection * -1);
	}
	
	void Update () {
		Move();

		lifeTime -= Time.deltaTime;

		if(lifeTime <= 0)
		{
			Destroy(gameObject);
		}
	}

	void Move()
	{
		transform.position += moveDirection * Time.deltaTime * 1f;
	}
}
