using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSet : MonoBehaviour
{
    public int AorB;
    GameObject BubbleUp, BubbleMid, BubbleDown, Text;
    bool finish = false;

    //public
    void Start()
    {
        if (!finish)
        SetSize();
    }
    public void SetSize(int Lengh = 24)
    {
        finish = true;
        BubbleUp = this.transform.GetChild(0).gameObject;
        Text = BubbleUp.transform.GetChild(1).gameObject;
        BubbleMid = BubbleUp.transform.GetChild(0).gameObject;
        BubbleDown = BubbleMid.transform.GetChild(0).gameObject;

        BubbleMid.GetComponent<RectTransform>().sizeDelta = new Vector2(206f, 1.2f);
        BubbleDown.GetComponent<RectTransform>().sizeDelta = new Vector2(205f, 24f);
       
            
        if (AorB == 0)
        {
            BubbleMid.GetComponent<RectTransform>().localPosition = new Vector3(0, -15.1f, 0);
            BubbleDown.GetComponent<RectTransform>().localPosition = new Vector3(-0.36f, -12.6f, 0);

        }
        else if (AorB == 1)
        {
            BubbleMid.GetComponent<RectTransform>().localPosition = new Vector3(0.8f, -15.1f, 0);
            BubbleDown.GetComponent<RectTransform>().localPosition = new Vector3(-0.3f, -12.6f, 0);

        }

        int TextLengh = Lengh;
        int TextHeight = TextLengh / 16;
        BubbleMid.GetComponent<RectTransform>().sizeDelta = BubbleMid.GetComponent<RectTransform>().sizeDelta + new Vector2(0f, 15f * TextHeight);
        BubbleMid.GetComponent<RectTransform>().localPosition = BubbleMid.GetComponent<RectTransform>().localPosition + new Vector3(0, -7.5f * TextHeight, 0);
        BubbleDown.GetComponent<RectTransform>().localPosition = BubbleDown.GetComponent<RectTransform>().localPosition + new Vector3(0, -7.5f * TextHeight, 0);
        this.GetComponent<RectTransform>().sizeDelta = new Vector3(350, BubbleUp.GetComponent<RectTransform>().rect.height / 2 + BubbleMid.GetComponent<RectTransform>().rect.height + BubbleDown.GetComponent<RectTransform>().rect.height, 0f);

    }
}
