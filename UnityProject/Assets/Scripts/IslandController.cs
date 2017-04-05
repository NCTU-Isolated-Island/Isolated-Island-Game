using IsolatedIslandGame.Library;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandController : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Vector3.Distance(transform.position, GameManager.Instance.PlayerGameObject.transform.position) > 80f)
            {
                //insert pop up info here
                UserManager.Instance.User.UserInform("通知", "你必須要再靠近一點才能進入孤島頁面");
                return;
            }
            UIManager.Instance.SwapPage(UIManager.UIPageType.IsolatedIsland);
        }
    }
}