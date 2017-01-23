using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bp_Button : MonoBehaviour {
    public Button yourButton;
    public GameObject Bp_list;
    public GameObject canvas;
    // Use this for initialization
    void Start()
    {
        
        // this.GetComponent<RectTransform>().localPosition = new Vector3(0, 90, 0);
        yourButton = this.gameObject.transform.GetChild(1).GetComponent<Button>();
        Bp_list.GetComponent<RectTransform>().localPosition = new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2 + Bp_list.GetComponent<RectTransform>().rect.width / 2, 20, 0);
        yourButton.onClick.AddListener(move);
    }

    // Update is called once per frame
    void move () {
        float A_pos = canvas.GetComponent<RectTransform>().rect.width / 2 - Bp_list.GetComponent<RectTransform>().rect.width / 2;
        float B_pos = canvas.GetComponent<RectTransform>().rect.width / 2 + Bp_list.GetComponent<RectTransform>().rect.width / 2;
        if (Mathf.Abs(Bp_list.GetComponent<RectTransform>().localPosition.x - A_pos) <1)
        { Bp_list.transform.localPosition = new Vector3(B_pos, 20, 0);  }
        else if(Mathf.Abs(Bp_list.GetComponent<RectTransform>().localPosition.x - B_pos) < 1)
        { Bp_list.transform.localPosition = new Vector3(A_pos, 20, 0); }
    }
}
