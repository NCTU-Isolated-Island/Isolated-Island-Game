using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggablePanel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool canDrag;
    private Vector3 originMousePosition;
    private Vector3 originPosition;
    private RectTransform self;
    private CanvasScaler canvasScaler;

    void Start()
    {
        self = GetComponent<RectTransform>();
        canvasScaler = UIManager.Instance.GetComponent<CanvasScaler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            Vector3 newPosition = originPosition + (Input.mousePosition - originMousePosition) * canvasScaler.referenceResolution.x / Screen.width;
            self.anchoredPosition = Vector3.Min(Vector3.Max(newPosition, 
                new Vector2(-canvasScaler.referenceResolution.x / 2 + self.rect.width / 2, -canvasScaler.referenceResolution.y / 2 + self.rect.height / 2)), 
                new Vector2(canvasScaler.referenceResolution.x / 2 - self.rect.width / 2, canvasScaler.referenceResolution.y / 2 - self.rect.height / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        canDrag = true;
        originMousePosition = Input.mousePosition;
        originPosition = self.anchoredPosition;
        transform.SetAsLastSibling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canDrag = false;
    }
}
