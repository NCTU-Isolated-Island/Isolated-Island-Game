using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IsolatedIslandGame.Library;

public class Combine_FuseButton : MonoBehaviour {

    public bool StopForTest;
    public Button yourButton;
    public GameObject CombineArea1;
    public GameObject CombineArea2;
    public GameObject CombineArea3;
    public GameObject CombineAreaOUT;
    // Use this for initialization
    void Start () {
        SetGameObject();
    }
	
    void Fuse()
    {
        if(!StopForTest)
        {
            //合成

             int ThingInA=0, ThingInB = 0, ThingInC = 0; //0沒東西 1,2,3物品

            if (!CombineArea1.GetComponent<Combine_block>().ItemInHere)
            {
                ThingInA = CombineArea1.GetComponent<Combine_block>().ItemInHere.GetComponent<ShowBag_ItemSelect>().ItemNo;
            }
            if (!CombineArea2.GetComponent<Combine_block>().ItemInHere)
            {
                ThingInB = CombineArea2.GetComponent<Combine_block>().ItemInHere.GetComponent<ShowBag_ItemSelect>().ItemNo;
            }
            if (!CombineArea3.GetComponent<Combine_block>().ItemInHere)
            {
                ThingInC = CombineArea3.GetComponent<Combine_block>().ItemInHere.GetComponent<ShowBag_ItemSelect>().ItemNo;
            }
            UserManager.Instance.User.Player.OperationManager.SynthesizeMaterial(
                new Blueprint.ElementInfo[]{
                 new Blueprint.ElementInfo { itemID = ThingInA , positionIndex = 0, itemCount= 1 },
                 new Blueprint.ElementInfo { itemID = ThingInB , positionIndex = 1, itemCount= 1 },
                 new Blueprint.ElementInfo { itemID = ThingInC , positionIndex = 2, itemCount= 1 }
                 }
             );
        }

        if (StopForTest &&
           CombineArea1.GetComponent<Combine_block>().ItemInHere == null
        && CombineArea2.GetComponent<Combine_block>().ItemInHere != null
        && CombineArea3.GetComponent<Combine_block>().ItemInHere != null
        && CombineArea2.GetComponent<Combine_block>().ItemInHere.GetComponent<ShowBag_ItemSelect>().ItemNo == 1
        && CombineArea3.GetComponent<Combine_block>().ItemInHere.GetComponent<ShowBag_ItemSelect>().ItemNo == 3
            )
        {
            CombineAreaOUT.SetActive(true);
            CombineAreaOUT.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Metalmon", typeof(Sprite)) as Sprite;
        }

    }
    void SetGameObject()
    {
        if (!yourButton)
            yourButton = this.gameObject.GetComponent<Button>();
        yourButton.onClick.AddListener(Fuse);
        if (!CombineArea1)
        { CombineArea1 = this.transform.parent.GetChild(0).gameObject; }
        if (!CombineArea2)
        { CombineArea2 = this.transform.parent.GetChild(1).gameObject; }
        if (!CombineArea3)
        { CombineArea3 = this.transform.parent.GetChild(2).gameObject; }
        if (!CombineAreaOUT)
        { CombineAreaOUT = this.transform.parent.GetChild(3).gameObject; }
    }
    

}
