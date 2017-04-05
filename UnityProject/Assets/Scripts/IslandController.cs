using UnityEngine;
using UnityEngine.EventSystems;

public class IslandController : MonoBehaviour
{
    private void OnMouseDown()
    {
		if(Vector3.Distance(transform.position,GameManager.Instance.PlayerGameObject.transform.position) > 80f)
			//insert pop up info here
			return;

        UIManager.Instance.SwapPage(UIManager.UIPageType.IsolatedIsland);
    }
}
