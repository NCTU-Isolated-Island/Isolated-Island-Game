using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	Vector3 initialPos;

	void Start()
	{
		Input.gyro.enabled = true;
		StartCoroutine(AB());
	}

	IEnumerator AB ()
	{
		while(true)
		{
			transform.rotation = Quaternion.
										
		}
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
