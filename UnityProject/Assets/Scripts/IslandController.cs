using UnityEngine;

public class IslandController : MonoBehaviour
{
    private void OnMouseDown()
    {
        UIManager.Instance.SwapPage(UIManager.UIPageType.IsolatedIsland);
    }
}
