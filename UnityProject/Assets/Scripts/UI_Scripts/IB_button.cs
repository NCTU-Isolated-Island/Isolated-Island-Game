using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IB_button : MonoBehaviour {
    public Button yourButton;
    // Use this for initialization
    void Start () {
        this.GetComponent<RectTransform>().localPosition = new Vector3(0,90,0);
        yourButton = this.gameObject.GetComponent<Button>();
       
        yourButton.onClick.AddListener(UpAndDown);
    }
	
	// Update is called once per frame

    void UpAndDown()
    {
        float A = -this.transform.parent.parent.GetComponent<RectTransform>().rect.height / 2 + this.transform.parent.GetComponent<RectTransform>().rect.height/2;
        float B = -this.transform.parent.parent.GetComponent<RectTransform>().rect.height / 2 - this.transform.parent.GetComponent<RectTransform>().rect.height / 2;

        if (this.transform.parent.GetComponent<RectTransform>().localPosition.y == B)
        {

            this.transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(0, A, 0);
            Debug.Log(A);
        }
        else
        {
            this.transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(0, B, 0);
            Debug.Log(B);
        }
    }
}
