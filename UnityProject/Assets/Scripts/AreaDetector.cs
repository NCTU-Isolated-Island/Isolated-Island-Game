using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetector : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AreaProbe")
        {
            if (!PlayerController.Instance.InArea.Contains(this.gameObject))
                PlayerController.Instance.InArea.Add(this.gameObject);
        }
        if(other.tag == "SelfVessel")
        {
            BGMController.Instance.ChangeBGM(name);
        }
    }
}
