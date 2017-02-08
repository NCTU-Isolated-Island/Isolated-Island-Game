using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour {

	private float initialYAngle = 0f;
	private float appliedGyroYAngle = 0f;
	private float calibrationYAngle = 0f;

	public static FirstPersonCameraController Instance;

	void Awake()
	{
		if(Instance == null)
			Instance = this;
	}

	void Start () {
		Input.gyro.enabled = true;

	}
	
	void Update()
	{
		ApplyGyroRotation();

		//AndroidVersion();
	}




	void ApplyGyroRotation()
	{
		transform.rotation = Input.gyro.attitude;
		transform.Rotate( 0f, 0f, 180f, Space.Self ); // Swap "handedness" of quaternion from gyro.
		transform.Rotate( 90f, 180f, 0f, Space.World ); // Rotate to make sense as a camera pointing out the back of your device.
		appliedGyroYAngle = transform.eulerAngles.y; // Save the angle around y axis for use in calibration.
	}

	void AndroidVersion()
	{
		transform.rotation = Input.gyro.attitude * new Quaternion(0,0,1,0);
	}
}
