using IsolatedIslandGame.Library;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandController : MonoBehaviour
{
    public static IslandController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            UIManager.Instance.SwapPage(UIManager.UIPageType.IsolatedIsland);
        }
    }
}