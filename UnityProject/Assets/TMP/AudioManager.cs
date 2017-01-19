using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioSource [] AudioPlayer;
    public float MasterVolume;

    private int UsingPlayer; // 0 means using player1 , 1 means using player2
    private bool Fading;

    void Awake()
    {
        foreach (AudioSource player in AudioPlayer)
        {
            player.volume = 0;
            player.loop = true;
        }
        Instance = this;
        UsingPlayer = 0;
        Fading = false;
        MasterVolume = 1;
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

    public bool SetBGM(string SourceName)
    {
        //
        print(SourceName);
        //
        AudioClip new_clip = AssetDatabase.LoadAssetAtPath("Assets/Resources/Audio/" + SourceName , typeof(AudioClip)) as AudioClip;
        // can rewrite to (SourceName , FolderName)

        ChangeUsingPlayer();
        AudioPlayer[UsingPlayer].clip = new_clip;
        StartCoroutine(FadeMusic(5));

        AudioPlayer[UsingPlayer].Play();
        return true;
    }

    IEnumerator FadeMusic(float OverTime)
    {
        Fading = true;

        float pass_time = 0;
        float IngVolume = AudioPlayer[1 - UsingPlayer].volume;

        while (pass_time < OverTime)
        {
            AudioPlayer[UsingPlayer].volume = Mathf.Lerp(0, MasterVolume, pass_time / OverTime);
            AudioPlayer[1 - UsingPlayer].volume = Mathf.Lerp(IngVolume, 0, pass_time / OverTime);
            pass_time += Time.deltaTime;
            yield return null;
        }
        AudioPlayer[UsingPlayer].volume = MasterVolume;
        AudioPlayer[1 - UsingPlayer].volume = 0;

        Fading = false;
    }

    //

    void Start()
    {
        GameManager.Instance.PlayerGameObject.transform.Find("ShipModel").Find("oil_tanker").gameObject.AddComponent<BGMController>();
        GameManager.Instance.PlayerGameObject.transform.Find("ShipModel").Find("oil_tanker").gameObject.AddComponent<Rigidbody>();
        GameManager.Instance.PlayerGameObject.transform.Find("ShipModel").Find("oil_tanker").gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    //
}
