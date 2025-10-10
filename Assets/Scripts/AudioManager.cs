using UnityEngine;
using System.Collections.Generic;//リスト使用の際
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGMAudioSource")]
    public AudioSource bgmSource;

    [Header("SEAudioSource")]
    public AudioSource seSource;

    [Header("BGMClipリスト")]
    public List<AudioClip> bgmList; 

    [Header("SEClipリスト")]
    public List<AudioClip> seList;

    //一応検索可能
    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> seDict;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //ここに関数
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeDictionaries()//辞書(名前検索)
    {
        bgmDict = new Dictionary<string, AudioClip>();
        seDict = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in bgmList)
        {
            if (!bgmDict.ContainsKey(clip.name))
                bgmDict.Add(clip.name, clip);
        }

        foreach (AudioClip clip in seList)
        {
            if (!seDict.ContainsKey(clip.name))
                seDict.Add(clip.name, clip);
        }
    }
    public void PlayBGM(string name)
    {
        if (bgmDict.ContainsKey(name))
        {
            //ループ再生の場合
            bgmSource.clip = bgmDict[name];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.Log($"BGM'{name}'が見つかりません。");
        }
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void PlaySE(string name)
    {
        //一回だけの場合
        if (bgmDict.ContainsKey(name))
        {
            seSource.PlayOneShot(seDict[name]);
        }
        else
        {
            Debug.Log($"SE'{name}'が見つかりません。");
        }
    }
}