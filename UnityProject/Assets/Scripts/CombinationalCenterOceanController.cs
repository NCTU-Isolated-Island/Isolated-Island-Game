using UnityEngine;

public class CombinationalCenterOceanController : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.gameObject.tag == "SelfVessel")
        {
            CombinationalOceanController.Instance.UpdateCenter(transform.position);
        }
    }
}
