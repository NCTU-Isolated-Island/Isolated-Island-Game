using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Transform default_transform; // the position of your own island
    public float smoothTime = 0.3F;     // the time spend on moving camera
	public static CameraManager Instance; 

    public Transform target;

    private bool focusing; // you can only focus one thing at a time , if focusing is true , can't call focus
    private Vector3 velocity = Vector3.zero;

	void Awake()
	{
		if(Instance == null){
			Instance = this;
		}else if(Instance != this){
			Destroy(gameObject);
		}
	}


    void Update()
    {
        if (focusing == true)
        {
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 3, -10));

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //
            transform.rotation = Quaternion.Slerp(transform.rotation , target.rotation , Time.deltaTime);

            if ((transform.position - targetPosition).magnitude <= 0)
            {
                focusing = false;// close enough , stop focus
            }
        }
    }

    public void Focus (Transform target)
    {
        if(focusing == false) focusing = true;
        this.target = target;
    }

    public void UnFocus()
    {
        Focus(default_transform);
    }

    public void CameraRotate(float y_angle)
    {
        GameObject tmp = new GameObject();
        tmp.transform.position = target.position;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        camera.transform.parent = tmp.transform;
        tmp.transform.Rotate(new Vector3(0, y_angle, 0));
        camera.transform.parent = null;
        Destroy(tmp);
    }

    public void EndFocus()
    {
        focusing = false;
    }

	public void AttachToChild(GameObject playerVessel)
	{
		transform.parent = playerVessel.transform;
	}


	public void ToNearAnchor(GameObject user)
	{
		//Attach to User's Camera Anchor
		//Move to User's Near Anchor

	}

	public void ToFarAnchor(GameObject user)
	{
		//Attach to User's Camera Anchor
		//Move to User's Far Anchor
	}

	public void Zoom(float amount)
	{
		//Zoom in(amount>0) and out(amount<0) 
		//Limit Zoom not too far and too close
	}

	public void Camera_Rotate(float angle)
	{
		
	}




//    void Start()
//    {
//        Invoke("tmp", 2);
//    }
//
//    void tmp()
//    {
//        CameraRotate(50);
//    }

}
