using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	private Vector3 TargetPosition;
	private Quaternion TargetRotaion;

	private Vector3 velocity;

	public int playerID;


	void Start()
	{
		TargetPosition = transform.position;
		TargetRotaion = transform.rotation;
	}

	void Update () {
		transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref velocity, 1f, 50);
		transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotaion ,Time.deltaTime);
	}

	public void UpdateLocation(Vector3 position,float eulurAngle)
	{
		TargetPosition = position;
		TargetRotaion = Quaternion.Euler(0f,eulurAngle,0f);
	}

}
