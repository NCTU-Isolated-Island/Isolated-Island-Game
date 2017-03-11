using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingGameObjectBehavior : MonoBehaviour {

    [SerializeField]
    private Image upperCircle;
    [SerializeField]
    private Image lowerCircle;

    private float theta;

    void Start()
    {
        theta = 0;
    }

    void Update()
    {
        lowerCircle = GetComponentsInChildren<Image>()[0];
        upperCircle = GetComponentsInChildren<Image>()[1];

        upperCircle.fillAmount = Mathf.Abs(Mathf.Sin(theta));

        if(theta >= Mathf.PI / 2f)
        {
            theta = 0f;

            upperCircle.transform.SetParent(transform);
            lowerCircle.transform.SetParent(upperCircle.transform);
        }

        theta += Time.deltaTime / 2f;
    }
	
}
