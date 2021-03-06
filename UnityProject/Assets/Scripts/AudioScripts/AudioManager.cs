﻿using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioSource[] AudioPlayer;
    public float MasterVolume;

    private int UsingPlayer; // 0 means using player1 , 1 means using player2
    private bool isFading;

    private IEnumerator FadeCoroutine;

    void Awake()
    {
        foreach (AudioSource player in AudioPlayer)
        {
            player.volume = 0;
            player.loop = true;
        }
        Instance = this;
        UsingPlayer = 0;
        isFading = false;
        MasterVolume = 0.3f;
    }

    void ChangeUsingPlayer()
    {
        UsingPlayer = 1 - UsingPlayer;
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = value;
        AudioPlayer[UsingPlayer].volume = value;
    }

    public void SetBGM(string SourceName)
    {
        AudioClip new_clip = Resources.Load("Audio/" + SourceName, typeof(AudioClip)) as AudioClip;

        ChangeUsingPlayer();
        AudioPlayer[UsingPlayer].clip = new_clip;
        AudioPlayer[UsingPlayer].Play();

        if (FadeCoroutine != null)
        {
            StopCoroutine(FadeCoroutine);
            print("stop");
        }

        FadeCoroutine = FadeMusic(5);
        StartCoroutine(FadeCoroutine);
    }

    IEnumerator FadeMusic(float OverTime)
    {
        isFading = true;

        float pass_time = 0;
        float OriVol0 = AudioPlayer[UsingPlayer].volume;
        float OriVol1 = AudioPlayer[1 - UsingPlayer].volume;

        while (pass_time < OverTime)
        {
            AudioPlayer[UsingPlayer].volume = Mathf.Lerp(OriVol0, MasterVolume, pass_time / OverTime);
            AudioPlayer[1 - UsingPlayer].volume = Mathf.Lerp(OriVol1, 0, pass_time / OverTime);
            pass_time += Time.deltaTime;
            yield return null;
        }
        AudioPlayer[UsingPlayer].volume = MasterVolume;
        AudioPlayer[1 - UsingPlayer].volume = 0;

        isFading = false;
    }

    void Start()
    {
        GameObject OceanDetector = new GameObject("OceanDetector");
        OceanDetector.AddComponent<BGMController>();
        OceanDetector.AddComponent<BoxCollider>();
        OceanDetector.AddComponent<Rigidbody>();
        OceanDetector.GetComponent<Collider>().isTrigger = true;
        OceanDetector.GetComponent<Rigidbody>().isKinematic = true;
		OceanDetector.GetComponent<Rigidbody>().useGravity = false;
        OceanDetector.transform.SetParent(GameManager.Instance.PlayerGameObject.transform);
        OceanDetector.transform.localPosition = Vector3.zero;
		OceanDetector.layer = LayerMask.NameToLayer("Ignore Raycast");

        //GameManager.Instance.PlayerGameObject.AddComponent<BGMController>();
        //GameManager.Instance.PlayerGameObject.AddComponent<Rigidbody>();
        //GameManager.Instance.PlayerGameObject.GetComponent<Rigidbody>().isKinematic = true;
        //GameManager.Instance.PlayerGameObject.GetComponent<Collider>().isTrigger = true;
    }
}