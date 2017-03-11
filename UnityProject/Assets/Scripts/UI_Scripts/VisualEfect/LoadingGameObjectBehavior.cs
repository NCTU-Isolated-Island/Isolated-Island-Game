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

    void Update()
    {
        upperCircle.fillAmount = Mathf.Cos(theta);
        lowerCircle.fillAmount = Mathf.Cos(2 * theta);

        theta += Time.deltaTime;
    }
	
}
