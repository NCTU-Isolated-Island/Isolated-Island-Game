using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using IsolatedIslandGame.Library;

public class Combine_FuseButton : MonoBehaviour {
    public Button yourButton;
    public GameObject CombineArea1;
    public GameObject CombineArea2;
    public GameObject CombineArea3;
    public GameObject CombineAreaOUT;
    // Use this for initialization
    void Start () {
        yourButton = this.gameObject.GetComponent<Button>();
        yourButton.onClick.AddListener(Fuse);
    }
	
    void Fuse()
    {
        //寫死
        if (
            CombineArea1.GetComponent<Combine_block>().ItemInHere == null
        &&  CombineArea2.GetComponent<Combine_block>().ItemInHere.GetComponent<Combine_ItemSelect>().ItemNo == 1
        &&  CombineArea3.GetComponent<Combine_block>().ItemInHere.GetComponent<Combine_ItemSelect>().ItemNo == 3
            )
        {
            CombineAreaOUT.SetActive(true);
            CombineAreaOUT.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Metalmon", typeof(Sprite)) as Sprite;
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
