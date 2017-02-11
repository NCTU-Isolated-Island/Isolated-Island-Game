using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
	private float appliedGyroYAngle = 0f;
	private float calibrationYAngle = 0f;

	public static FirstPersonCameraController Instance;

	void Awake()
	{
		if(Instance == null)
			Instance = this;
	}

	void Start ()
    {
		Input.gyro.enabled = true;
	}
	
	void Update()
	{
		ApplyGyroRotation();
	}

	void ApplyGyroRotation()
	{
		transform.rotation = Input.gyro.attitude;

        transform.Rotate(0f, 0f, 180f, Space.Self);
        #if UNITY_ANDROID
            transform.Rotate( 270f, 180f, 180f, Space.World );
        #else
		    transform.Rotate( 90f, 180f, 0f, Space.World );
        #endif
	}

}
