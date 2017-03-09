using IsolatedIslandGame.Library;
using UnityEngine;

public class CombinationalOceanController : MonoBehaviour
{
    public static CombinationalOceanController Instance { get; private set; }
    private Vector3 lastestSubCenter;

    private void Awake()
    {
        Instance = this;
        lastestSubCenter = transform.position;
        if(UserManager.Instance.User.IsOnline)
        {
            UserManager.Instance.User.Player.OnBindVessel += (vessel) => 
            {
                transform.position = new Vector3(vessel.LocationX, transform.position.y, vessel.LocationZ);
            };
        }
        else
        {
            UserManager.Instance.User.OnPlayerOnline += (player) => 
            {
                UserManager.Instance.User.Player.OnBindVessel += (vessel) =>
                {
                    transform.position = new Vector3(vessel.LocationX, transform.position.y, vessel.LocationZ);
                };
            };
        }
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
