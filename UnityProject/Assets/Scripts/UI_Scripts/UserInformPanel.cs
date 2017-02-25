using UnityEngine;
using UnityEngine.UI;

public class UserInformPanel : MonoBehaviour
{
    private Text titleText;
    private Text contentText;

    private void Awake()
    {
        titleText = transform.Find("Title").GetComponent<Text>();
        contentText = transform.Find("Content").GetComponent<Text>();
        transform.Find("Confirm").GetComponent<Button>().onClick.AddListener(() => 
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
