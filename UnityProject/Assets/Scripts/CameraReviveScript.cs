using IsolatedIslandGame.Library;
using UnityEngine;

public class CameraReviveScript : MonoBehaviour
{
    bool isClosed = false;
    void OnApplicationQuit()
    {
        isClosed = true;
    }

    void OnDestroy()
    {
        if (isClosed)
            return;
        GameObject tmp = Instantiate(gameObject);
        tmp.SetActive(true);
        tmp.name = gameObject.name;
        if (UserManager.Instance != null && UserManager.Instance.User.IsOnline)
        {
            tmp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform.Find("CameraAnchor"));
            CameraManager.Instance.Camera = tmp;
            CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
            print("Revive!");
        }
        else
        {
            UserManager.Instance.User.OnPlayerOnline += (player) =>
            {
                GameManager.Instance.GetPlayerVesselGameObject();
                tmp.transform.SetParent(GameManager.Instance.PlayerGameObject.transform.Find("CameraAnchor"));
                CameraManager.Instance.Camera = tmp;
                CameraManager.Instance.ToNearAnchor(GameManager.Instance.PlayerGameObject);
                print("Revive!");
            };
        }

        FirstPersonCameraController.Instance = tmp.GetComponent<FirstPersonCameraController>();
    }
}
