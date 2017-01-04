using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Combine_pos : MonoBehaviour {

    public GameObject CombinePanel;
    float A_pos ;
    float B_pos;
    public GameObject BAG;
    public GameObject canvas;
    public GameObject CombineArea;

    // Use this for initialization
    void Start () {
         A_pos = -canvas.GetComponent<RectTransform>().rect.height / 2 - BAG.GetComponent<RectTransform>().rect.height / 2;
         B_pos = -canvas.GetComponent<RectTransform>().rect.height / 2 + BAG.GetComponent<RectTransform>().rect.height / 2;
         this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos,0);
    }

    void Update()
    {
        if (CombinePanel.activeInHierarchy == false)
        {
            this.enabled = false;
        }
    }
        
        // Update is called once per frame
	public void UpAndDown ()
    {
        if(Mathf.Abs( this.GetComponent<RectTransform>().localPosition.y - A_pos)<1)
        { this.GetComponent<RectTransform>().localPosition = new Vector3(0, B_pos, 0); }
        else if(Mathf.Abs(this.GetComponent<RectTransform>().localPosition.y -B_pos) < 1)
        { this.GetComponent<RectTransform>().localPosition = new Vector3(0, A_pos, 0); }
    }
    public void SetPicture(GameObject ItemSelect)
    {
        if (CombineArea.GetComponent<Combine_block>().ItemInHere != null)
        { CombineArea.GetComponent<Combine_block>().ItemInHere.GetComponent<Combine_ItemSelect>().CombineAreaILocate = null; }

        CombineArea.GetComponent<Combine_block>().ItemInHere = ItemSelect;
        CombineArea.transform.GetChild(0).GetComponent<Image>().sprite = ItemSelect.GetComponent<Image>().sprite as Sprite;
    }
}
