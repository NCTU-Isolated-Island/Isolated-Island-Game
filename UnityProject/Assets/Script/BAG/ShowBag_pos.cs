using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowBag_pos : MonoBehaviour {
    public GameObject UIControl;
    public GameObject ShowBagPanel;
    public GameObject BAG;
    public GameObject canvas;
    public Button BackButton;
    public GameObject MainBoat;
    float A_pos;
    float B_pos;
    // Use this for initialization
    void Start () {
        UIControl = GameObject.FindWithTag("UImanager");
        A_pos = -canvas.GetComponent<RectTransform>().rect.height / 2 - BAG.GetComponent<RectTransform>().rect.height / 2;
        B_pos = -canvas.GetComponent<RectTransform>().rect.height / 2 + BAG.GetComponent<RectTransform>().rect.height / 2;
        BackButton = this.gameObject.transform.GetChild(4).GetComponent<Button>();
        BackButton.onClick.AddListener(BACK);
    }
   public bool BackOnce = false;
	// Update is called once per frame
	void Update () {
        if (ShowBagPanel.activeInHierarchy == false)
        {
            if (!BackOnce)
            {
                this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0); 

                BackOnce = true;
            }
        }
        else if (ShowBagPanel.activeInHierarchy == true )
        {
            this.GetComponent<RectTransform>().localPosition = new Vector3(0, B_pos, 0);
            BackOnce = false;
        }
      
    }
    void BACK()
    {
        UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;

        //MainBoat.active = true;
        //this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0);
        //ShowBagPanel.active = false;
        //this.enabled = false;
    }
}
