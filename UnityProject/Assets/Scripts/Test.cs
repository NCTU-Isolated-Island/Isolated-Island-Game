using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public GameObject test;

	void Start()
	{
		Input.gyro.enabled = true;

		AuxCameraSystem.Instance.Show3DObject(test, new Vector3(0,-3,7));

	}



//	void Update()
//	{
//		transform.rotation = Quaternion.Euler(90,0,0) * Quaternion.Euler
//			(
//				Input.gyro.attitude.eulerAngles.x * -1,
//				Input.gyro.attitude.eulerAngles.y * -1,
//				Input.gyro.attitude.eulerAngles.z
//			) * Quaternion.Euler
//			(
//				Input.gyro.attitude.eulerAngles.x * -1,
//				Input.gyro.attitude.eulerAngles.y * -1,
//				Input.gyro.attitude.eulerAngles.z
//			);
//
//
//
//	}


}
