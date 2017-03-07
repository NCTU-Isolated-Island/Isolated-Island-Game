using UnityEngine;

public class ConbinationalSubOceanController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.tag == "SelfVessel")
        {
            CombinationalOceanController.Instance.UpdateSubCenter(transform.position);
        }
    }
}
