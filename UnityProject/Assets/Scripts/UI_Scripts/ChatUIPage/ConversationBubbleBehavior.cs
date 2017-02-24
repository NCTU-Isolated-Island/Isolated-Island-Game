using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationBubbleBehavior : MonoBehaviour
{
    //void Start()
    //{
    //    Invoke("AdjustSize", 0.01f);
    //}

    void Update()
    {
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(217f, gameObject.GetComponentInChildren<Text>().gameObject.GetComponent<RectTransform>().rect.height + 20);
    }
}
