using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Transform default_transform; // the position of your own island
    public float smoothTime = 0.3F;     // the time spend on moving camera

    public Transform target;

    private bool focusing; // you can only focus one thing at a time , if focusing is true , can't call focus
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (focusing == true)
        {
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 3, -10));

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //
            transform.rotation = Quaternion.Slerp(transform.rotation , target.rotation , Time.deltaTime * 5f);

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

    public void EndFocus()
    {
        focusing = false;
    }
}
