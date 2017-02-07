using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LExpand : MonoBehaviour
{
    private bool ExpandOrWithdraw;

    public GameObject viewportHorizontal;
    public GameObject viewportVertical;

    private float hori_ori;
    private float ver_ori;

    private IEnumerator coroutine;

    // Use this for initialization
    void Start()
    {
        Vector3 horiTmp = viewportHorizontal.GetComponent<RectTransform>().offsetMin;
        Vector3 verTmp = viewportVertical.GetComponent<RectTransform>().offsetMax;
        hori_ori = horiTmp.x;
        ver_ori = verTmp.y;

        ExpandOrWithdraw = false;
        // TESTING
        //Invoke("ExpandBtn", 1f);
        //Invoke("WithDrawBtn", 2f);
    }

    public void OnClick()
    {
        if (ExpandOrWithdraw) ExpandBtn();
        else WithDrawBtn();

        ExpandOrWithdraw = !ExpandOrWithdraw;
    }

    public void ExpandBtn()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(false);
        StartCoroutine(coroutine);
    }

    public void WithDrawBtn()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(true);
        StartCoroutine(coroutine);
    }

    IEnumerator ExpandBtnCoroutine(bool OnOff)
    {
        print("OnOff = " + OnOff);

        float passTime = 0f;
        Vector3 horiTmp = viewportHorizontal.GetComponent<RectTransform>().offsetMin;
        Vector3 verTmp = viewportVertical.GetComponent<RectTransform>().offsetMax;

        float hori_st = horiTmp.x;
        float ver_st = verTmp.y;

        while (passTime < 1.5f)
        {
            if (OnOff == true)
            {
                horiTmp.x = Mathf.Lerp(hori_st, 0, passTime / 1.5f);
                viewportHorizontal.GetComponent<RectTransform>().offsetMin = horiTmp;
                verTmp.y = Mathf.Lerp(ver_st, 0, passTime / 1.5f);
                viewportVertical.GetComponent<RectTransform>().offsetMax = verTmp;
            }
            else
            {
                horiTmp.x = Mathf.Lerp(hori_st, hori_ori, passTime / 1.5f);
                viewportHorizontal.GetComponent<RectTransform>().offsetMin = horiTmp;
                verTmp.y = Mathf.Lerp(ver_st, ver_ori, passTime / 1.5f);
                viewportVertical.GetComponent<RectTransform>().offsetMax = verTmp;
            }

            passTime += Time.deltaTime;
            yield return null;
        }
    }

}
