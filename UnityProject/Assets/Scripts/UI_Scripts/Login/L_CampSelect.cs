using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IsolatedIslandGame.Library;

public class L_CampSelect : MonoBehaviour
{

    public GameObject UIControl;

    public GameObject[] Camp;
    public int CampNum; //0=農,1=商,2=動物
    public GameObject CampTitle;

    public Button RightButton, LeftButton, CampSelect;
    public GameObject Confirm;
    // Use this for initialization
    void Start()
    {

        UIControl = GameObject.FindWithTag("UImanager");

        int t = this.gameObject.transform.childCount;
        RightButton = this.gameObject.transform.GetChild(t - 3).GetComponent<Button>();
        RightButton.onClick.AddListener(Right);
        LeftButton = this.gameObject.transform.GetChild(t - 2).GetComponent<Button>();
        LeftButton.onClick.AddListener(Left);
        CampSelect = this.gameObject.transform.GetChild(t - 4).GetComponent<Button>();
        CampSelect.onClick.AddListener(Select);

        if (!CampTitle)
        { CampTitle = this.gameObject.transform.GetChild(t - 5).gameObject; }

        CampNum = 0;

        if (!Confirm)
            Confirm = this.gameObject.transform.GetChild(8).gameObject;
        Confirm.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Yes);
        Confirm.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(No);
    }
    void Update()
    {
        switch (CampNum)
        {
            case 0: CampTitle.GetComponent<Text>().text = ("Farmer");
                CampTitle.transform.GetChild(0).GetComponent<Text>().text = (
                  "與世無爭，靠天吃飯的一群農人，相信大自然的力量不可違背。\n為了拯救地球的糧食危機，具有高科技背景的農夫研發出具高度環境耐受力的基因改造作物，\n並進行一連串的動物實驗。\n然而這些動物卻在吃下基改作物後發生基因突變，造成一連串不可收拾的後果。\n在末日來臨、土地和家園被洪水沖毀之際，農人帶著希望划向最後一片淨土，\n渴望重新開闢與自然和平共處的生活，美麗的阡陌小路與盎綠農田。");

                   CampTitle.transform.GetChild(0).GetComponent<Text>().text.Replace("\\n", "\n");
                break;
            case 1: CampTitle.GetComponent<Text>().text = ("Businessman");
                CampTitle.transform.GetChild(0).GetComponent<Text>().text = (
     "金錢至上，以利益為導向的一群商人，相信這世界沒有錢買不到的東西在動物突變後，為了阻止動物們反撲的危機，引發了核彈\n導致末日將近，在世界僅剩最後一片淨土之時，他們想再次佔領區域\n在此蓋上金融大廈、豪華住宅，恢復原本有錢人控制的世界秩序");
                CampTitle.transform.GetChild(0).GetComponent<Text>().text.Replace("\\n", "\n");

                break;
            case 2: CampTitle.GetComponent<Text>().text = ("Animal");
                CampTitle.transform.GetChild(0).GetComponent<Text>().text = (
                   "由於吃下身為動物無法負荷的人類食品，造成基因突變，動物們開始學會思考\n有了自己思緒後的動物們，決定奪回屬於他們的世界，\n而和世界上原本霸主－－商人起了衝突，卻成了無法阻止的局面\n而這群幸運存活下來的生物，牠們深信災難和人類的破壞是密不可分，\n正在海上漂流的他們只希望能夠重新到達肥沃的淨土，\n開闢一個沒有人類、萬物欣欣向榮的家園。");
                   CampTitle.transform.GetChild(0).GetComponent<Text>().text.Replace("\\n", "\n");
                break;
        }
    }
    void Right()
    {
        if (!Confirm.activeInHierarchy)
        {
            Camp[CampNum].SetActive(false);
            if (CampNum == 2) { CampNum = 0; } else { CampNum++; }
            Camp[CampNum].SetActive(true);
        }
      
    }
    void Left()
    {
        if (!Confirm.activeInHierarchy)
        {
            Camp[CampNum].SetActive(false);
        if (CampNum == 0) { CampNum = 2; } else { CampNum--; }
        Camp[CampNum].SetActive(true);
        }
    }
    void Select()
    {
        if (!Confirm.activeInHierarchy)
        {
            Confirm.SetActive(true);
        }
    }
    void Yes()
    {
        UIControl.GetComponent<UImanager>().CampNum = CampNum;
        UIControl.GetComponent<UImanager>().Create_Charter();
        this.gameObject.transform.parent.GetComponent<L_Loading>().LoadingCase = 0;
        //UIControl.GetComponent<UImanager>().GameUI = UImanager.UI.Main_Boat;
        UIControl.GetComponent<UImanager>().ChangeUI((int)UImanager.UI.Main_Boat);
        this.gameObject.SetActive(false);
    }
    void No()
    {
        Confirm.SetActive(false);
    }

}
