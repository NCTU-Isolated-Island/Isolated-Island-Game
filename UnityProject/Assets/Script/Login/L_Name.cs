using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L_Name : MonoBehaviour
{
   public GameObject UIControl;
    public Button Name_Button;
    public GameObject input_name, input_speech;
    string player_name, speech;
    public GameObject Camp_screen;
    public GameObject ErrorText;
    void Start()
    {
        UIControl = GameObject.FindWithTag("UImanager");
        //input_name = this.gameObject.transform.GetChild(1).gameObject;
        //input_speech = this.gameObject.transform.GetChild(2).gameObject;
        Name_Button = this.gameObject.transform.GetChild(3).GetComponent<Button>();
        ErrorText = this.gameObject.transform.GetChild(4).gameObject;
        Name_Button.onClick.AddListener(Next);
    }
    
    void Next()
    {
        ErrorText.SetActive(false);
        player_name = input_name.GetComponent<Text>().text;
        speech = input_speech.GetComponent<Text>().text;
        if (player_name.Length>6)
        {
          
            ErrorText.SetActive(true);
            ErrorText.GetComponent<Text>().text = "角色名稱過長";
            return;
        }
        if(speech.Length > 16)
        {
            ErrorText.SetActive(true);
            ErrorText.GetComponent<Text>().text = "船銘過長";
            return;
        }
        if(player_name == " ")
        {
            ErrorText.SetActive(true);
            ErrorText.GetComponent<Text>().text = "角色名稱未輸入";
            return;
        }
        if (speech == null)
        {
            speech = "往夢想之島邁進!!!";
        }
        UIControl.GetComponent<UImanager>().player_name = player_name;
        UIControl.GetComponent<UImanager>().speech = speech;
        Camp_screen.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
