using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Items;
public class ShowBag_DetailPanel : MonoBehaviour, IPointerClickHandler
{
    public Button Back_Button;
    public Button Combin_Button;
    public Button PutOut_Button;
    public Button Throw_Button;
    public Button Favorite_Button;

    public GameObject ItemCallMe;

    public GameObject Pic;
    public GameObject Text;

    public int inventoryItemInfoID;
    public bool FavoriteBool;
    // Use this for initialization
    void Start()
    {
        SetGameObject();
    }
    void SetGameObject()
    {
        if (!Back_Button)
            Back_Button = this.gameObject.transform.GetChild(7).GetComponent<Button>();
        Back_Button.onClick.AddListener(back);
             
            if (!Combin_Button)
            Combin_Button = this.gameObject.transform.GetChild(3).GetComponent<Button>();
      
        if (!PutOut_Button)
            PutOut_Button = this.gameObject.transform.GetChild(4).GetComponent<Button>();
        
        if (!Throw_Button)
            Throw_Button = this.gameObject.transform.GetChild(5).GetComponent<Button>();
    
        if (!Favorite_Button)
            Favorite_Button = this.gameObject.transform.GetChild(6).GetComponent<Button>();
        Favorite_Button.onClick.AddListener(SwitchFavorite);

        if (!Pic)
            Pic = this.gameObject.transform.GetChild(1).gameObject;
        if (!Text)
            Text = this.gameObject.transform.GetChild(2).gameObject;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       
    }
    void SwitchFavorite()
    {
        int inventoryID = UserManager.Instance.User.Player.Inventory.InventoryID;
        UserManager.Instance.User.Player.OperationManager.SetFavoriteItem(inventoryID, inventoryItemInfoID);
        FavoriteBool = !FavoriteBool;
        ItemCallMe.GetComponent<ShowBag_ItemSelect>().FavoriteBool = FavoriteBool;
    }
    void back()
    {
        this.gameObject.SetActive(false);
    }
    void Update()
    {
        if (FavoriteBool)
        {
           Favorite_Button.transform.GetChild(0).GetComponent<Text>().text = (" Favorite ");
        }
        else
        {
            Favorite_Button.transform.GetChild(0).GetComponent<Text>().text = (" X ");
        }
    }
    // Update is called once per frame

}
