using UnityEngine;

namespace IsolatedIslandGame.Client.Scripts.SystemScripts
{
    public class EternalGameObject : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
