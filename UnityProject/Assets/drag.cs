using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

	// Use this for initialization
	void Start () {
	
	}
    public void OnBeginDrag(PointerEventData eventData)
    {  }
    public void OnDrag(PointerEventData eventData)
    {this.transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    { }

        // Update is called once per frame
        void Update () {
	
	}
}
