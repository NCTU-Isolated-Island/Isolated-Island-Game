using UnityEngine;

public class OceanController : MonoBehaviour
{
    public static OceanController Instance { get; private set; }
    [SerializeField]
    private float positionY;
    [SerializeField]
    private float relocatedRange;

    public Transform SelfVesselTransform { private get; set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update ()
    {
		if(SelfVesselTransform != null)
        {
            Vector3 distance = transform.position - SelfVesselTransform.position;
            distance.y = 0;
            if(distance.magnitude > relocatedRange)
            {
                Vector3 newPosition = SelfVesselTransform.position;
                newPosition.y = positionY;
                transform.position = newPosition;

            }
        }
	}
}
