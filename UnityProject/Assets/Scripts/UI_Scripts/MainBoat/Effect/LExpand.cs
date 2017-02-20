using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LExpand : MonoBehaviour
{
    public static LExpand Instance { get; private set; }
    // false for Buttons are in , true for Buttons are expanded
    [SerializeField]
    private bool LButtonStatus;

    //[SerializeField]
    //private GameObject settingButton;
    //[SerializeField]
    //private GameObject aboutButton;

    [SerializeField]
    private GameObject viewportHorizontal;
    [SerializeField]
    private GameObject viewportVertical;

    private float hori_ori;
    private float ver_ori;

    private float intervalTime;

    private IEnumerator coroutine;
    //
    private void SetNotLExpandButtonStatus(bool OnOff)
    {
        //settingButton.SetActive(OnOff);
        //aboutButton.SetActive(OnOff);
    }
    //
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        Vector3 horiTmp = viewportHorizontal.GetComponent<RectTransform>().offsetMin;
        Vector3 verTmp = viewportVertical.GetComponent<RectTransform>().offsetMax;
        hori_ori = horiTmp.x;
        ver_ori =  verTmp.y;
        // Initial Setting
        SetNotLExpandButtonStatus(false);

        intervalTime = 0.5f;
        // TESTING
    }

    public void OnClick()
    {
        if (!LButtonStatus) ExpandBtn();
        else WithDrawBtn();
    }

    public void ExpandBtn()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(true);
        StartCoroutine(coroutine);

        SetNotLExpandButtonStatus(true);
        LButtonStatus = true;
    }

    public void WithDrawBtn()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = ExpandBtnCoroutine(false);
        StartCoroutine(coroutine);

        SetNotLExpandButtonStatus(false);
        LButtonStatus = false;
    }

    IEnumerator ExpandBtnCoroutine(bool isOn)
    {
        float passTime = 0f;
        Vector3 horiTmp = viewportHorizontal.GetComponent<RectTransform>().offsetMin;
        Vector3 verTmp = viewportVertical.GetComponent<RectTransform>().offsetMax;

        float hori_st = horiTmp.x;
        float ver_st = verTmp.y;

        while (passTime < intervalTime)
        {
            passTime += Time.deltaTime;
            float lerpAlpha = 0.1f * passTime / intervalTime + 0.9f * Mathf.Sqrt(1 - Mathf.Pow(passTime / intervalTime - 1, 2));
            if (isOn)
            {
                horiTmp.x = Mathf.Lerp(hori_st, 0, lerpAlpha);
                viewportHorizontal.GetComponent<RectTransform>().offsetMin = horiTmp;
                verTmp.y = Mathf.Lerp(ver_st, 0, lerpAlpha);
                viewportVertical.GetComponent<RectTransform>().offsetMax = verTmp;
            }
            else
            {
                horiTmp.x = Mathf.Lerp(hori_st, hori_ori, lerpAlpha);
                viewportHorizontal.GetComponent<RectTransform>().offsetMin = horiTmp;
                verTmp.y = Mathf.Lerp(ver_st, ver_ori, lerpAlpha);
                viewportVertical.GetComponent<RectTransform>().offsetMax = verTmp;
            }

            yield return null;
        }
    }
}