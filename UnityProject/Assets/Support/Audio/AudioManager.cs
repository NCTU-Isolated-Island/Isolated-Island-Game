using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public float volume_speed;

    [SerializeField]
    private AudioSource [] AudioPlayer;

    private int UsingPlayer; // 0 means using player1 , 1 means using player2
    private bool Fading;

    void Awake()
    {
        foreach (AudioSource a in AudioPlayer)
            a.volume = 0;
        UsingPlayer = 0;
        Fading = false;
    }

    void ChangeUsingPlayer()
    {
        UsingPlayer = 1 - UsingPlayer;
    }

    bool SetVolume(float new_volume) // return success or not
    {
        if(Fading == true) return false;

        AudioPlayer[UsingPlayer].volume = new_volume;

        return true;
    }

    bool SetBGM(string SourceName)
    {
        if (Fading == true) return false;

        AudioClip new_clip = AssetDatabase.LoadAssetAtPath("Assets/" + SourceName , typeof(AudioClip)) as AudioClip;
        // can rewrite to (SourceName , FolderName)

        ChangeUsingPlayer();
        AudioPlayer[UsingPlayer].clip = new_clip;
        Fading = true;
        AudioPlayer[UsingPlayer].Play();
        return true;
    }

    void Update()
    {
        if(Fading)
        {
            AudioPlayer[UsingPlayer].volume += Time.deltaTime * volume_speed;
            AudioPlayer[1-UsingPlayer].volume -= Time.deltaTime * volume_speed;

            if (AudioPlayer[UsingPlayer].volume >= 1 && AudioPlayer[1 - UsingPlayer].volume <= 0)
                Fading = false;
        }
    }

    void Start()
    {
        SetBGM("Sound1.wav");

        Invoke("tmp" , 5);
    }

    void tmp()
    {
        Debug.Log("in");
        SetBGM("Sound2.wav");
        Debug.Log("out");
    }
}
