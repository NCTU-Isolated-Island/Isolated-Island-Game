using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class L_CampTurn : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler {
    public Vector2 Start_Position;
    public float Turn_Distant;
    public bool AmIFirst = false;
    public Vector3 ori;
    // Update is called once per frame
    void Start () {
        if (this.transform.parent.GetChild(2) == this.gameObject.transform)
        {
            AmIFirst = true;
            Debug.Log(this.gameObject.name);
        }
        ori = this.gameObject.transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventdata)
    {
        if (AmIFirst)
        {
            Start_Position = eventdata.position;
        }
        
    }

    public void OnDrag(PointerEventData eventdata)
    {
        if (AmIFirst)
        {
            Turn_Distant = eventdata.position.x - Start_Position.x;        
            this.gameObject.transform.localPosition = new Vector3(ori.x+Turn_Distant/3, -104 +104*Mathf.Sin(-Mathf.Acos(Turn_Distant / 312)) , ori.z);
        }
        
    }
    public void OnEndDrag(PointerEventData eventdata)
    {
        Turn_Distant = 0;
        ori = this.gameObject.transform.localPosition;
    }

}
