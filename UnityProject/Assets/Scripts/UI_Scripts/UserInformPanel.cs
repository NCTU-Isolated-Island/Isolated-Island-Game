using UnityEngine;
using UnityEngine.UI;

public class UserInformPanel : MonoBehaviour
{
    private Text titleText;
    private Text contentText;

    private void Awake()
    {
        titleText = transform.Find("TitlePanel/TitleText").GetComponent<Text>();
        contentText = transform.Find("ContentPanel/ContentText").GetComponent<Text>();
        transform.Find("ContentPanel").GetComponent<Button>().onClick.AddListener(() => 
        {
            Destroy(gameObject);
        });
    }

    public void RenderUserInform(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
    }
}
