using UnityEngine;
using System.Collections.Generic;//リスト使用の際
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float startTime = 0.5f;
    public float duration = 0.2f;

    private bool isPlaying = false;

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
    //--------------------------------
    // BGM
    //--------------------------------
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
    //--------------------------------
    // SE(一回だけ
    //--------------------------------
    public void PlaySE(string name)
    {
        //if (!seDict.ContainsKey(name))
        //{
        //    Debug.Log($"SE'{name}'が見つかりません。");
        //    return;
        //}
        //if(seSource.isPlaying)
        //{
        //    seSource.PlayOneShot(seDict[name]);
        //}
        //else
        //{
        //    Debug.LogWarning($"SE'{name}'が見つかりません。");
        //    return;
        //}
    }
    //--------------------------------
    // SE(部分再生)
    //--------------------------------
    public void PlaySEPartialOneShot(string name,float startTime,float duration)
    {
        //旧バージョン
        /*
        if (seDict.ContainsKey(name))
        {
            seSource.PlayOneShot(seDict[name]);
            //seSource.Play(seDict[name]);
        }
        else
        {
            Debug.Log($"SE'{name}'が見つかりません。");
        }
        */
        //新バージョン
        if (!seDict.ContainsKey(name))
        {
            Debug.Log($"SE'{name}'が見つかりません。");
            return;
        }

        AudioClip clip = seDict[name];
        float clipLength = clip.length;

        //startTimeが範囲外なら修正
        if(startTime > clipLength)
        {
            Debug.LogWarning($"[{name}] の startTime({startTime}) が音声長({clipLength})を超えています。0に修正します。");
            startTime = 0f;
        }

        //再生時間を調整
        if(startTime + duration > clipLength)
        {
            duration = clipLength - startTime;
        }

        StartCoroutine(PlayPartialOneShotCoroutine(clip, startTime, duration));
    }
    private System.Collections.IEnumerator PlayPartialOneShotCoroutine(AudioClip clip, float startTime, float duration)
    {
        // clipを指定時間から再生して、一定時間で止める
        seSource.clip = clip;
        seSource.time = startTime;
        seSource.Play();

        yield return new WaitForSeconds(duration);

        seSource.Stop();
    }
    /*
    private System.Collections.IEnumerator PlayPartialOneShotCoroutine(AudioClip clip, float startTime, float duration)
    {
        
        seSource.clip = clip;
        seSource.time = startTime;
        seSource.Play();
        yield return new WaitForSeconds(duration);
        seSource.Stop();
        
        // 一時的にAudioClipのデータを部分抽出して再生する
        int frequency = clip.frequency;
        int channels = clip.channels;

        int startSample = Mathf.FloorToInt(startTime * frequency);
        int lengthSamples = Mathf.FloorToInt(duration * frequency);

        if (startSample + lengthSamples > clip.samples)
            lengthSamples = clip.samples - startSample;

        // 一時データ領域
        float[] data = new float[lengthSamples * channels];
        clip.GetData(data, startSample);

        // 新しい一時的AudioClipを生成
        AudioClip tempClip = AudioClip.Create("Partial_" + clip.name, lengthSamples, channels, frequency, false);
        tempClip.SetData(data, 0);

        seSource.PlayOneShot(tempClip);

        yield return new WaitForSeconds(duration);
        Destroy(tempClip);
    }
    */
}