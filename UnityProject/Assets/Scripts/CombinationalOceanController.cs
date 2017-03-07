using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationalOceanController : MonoBehaviour
{
    public static CombinationalOceanController Instance { get; private set; }
    private Vector3 lastestSubCenter;

    private void Awake()
    {
        Instance = this;
        lastestSubCenter = transform.position;
    }
    public void UpdateCenter(Vector3 orginCenter)
    {
        transform.position = new Vector3(lastestSubCenter.x, transform.position.y, lastestSubCenter.z);
        lastestSubCenter = orginCenter;
    }
    public void UpdateSubCenter(Vector3 center)
    {
        lastestSubCenter = center;
    }
}
