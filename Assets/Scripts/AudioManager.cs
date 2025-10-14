using UnityEngine;
using System.Collections.Generic;//���X�g�g�p�̍�
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

    [Header("BGMClip���X�g")]
    public List<AudioClip> bgmList; 

    [Header("SEClip���X�g")]
    public List<AudioClip> seList;

    //�ꉞ�����\
    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> seDict;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //�����Ɋ֐�
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
    void InitializeDictionaries()//����(���O����)
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
            //���[�v�Đ��̏ꍇ
            bgmSource.clip = bgmDict[name];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.Log($"BGM'{name}'��������܂���B");
        }
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    //--------------------------------
    // SE(��񂾂�
    //--------------------------------
    public void PlaySE(string name)
    {
        //if (!seDict.ContainsKey(name))
        //{
        //    Debug.Log($"SE'{name}'��������܂���B");
        //    return;
        //}
        //if(seSource.isPlaying)
        //{
        //    seSource.PlayOneShot(seDict[name]);
        //}
        //else
        //{
        //    Debug.LogWarning($"SE'{name}'��������܂���B");
        //    return;
        //}
    }
    //--------------------------------
    // SE(�����Đ�)
    //--------------------------------
    public void PlaySEPartialOneShot(string name,float startTime,float duration)
    {
        //���o�[�W����
        /*
        if (seDict.ContainsKey(name))
        {
            seSource.PlayOneShot(seDict[name]);
            //seSource.Play(seDict[name]);
        }
        else
        {
            Debug.Log($"SE'{name}'��������܂���B");
        }
        */
        //�V�o�[�W����
        if (!seDict.ContainsKey(name))
        {
            Debug.Log($"SE'{name}'��������܂���B");
            return;
        }

        AudioClip clip = seDict[name];
        float clipLength = clip.length;

        //startTime���͈͊O�Ȃ�C��
        if(startTime > clipLength)
        {
            Debug.LogWarning($"[{name}] �� startTime({startTime}) ��������({clipLength})�𒴂��Ă��܂��B0�ɏC�����܂��B");
            startTime = 0f;
        }

        //�Đ����Ԃ𒲐�
        if(startTime + duration > clipLength)
        {
            duration = clipLength - startTime;
        }

        StartCoroutine(PlayPartialOneShotCoroutine(clip, startTime, duration));
    }
    private System.Collections.IEnumerator PlayPartialOneShotCoroutine(AudioClip clip, float startTime, float duration)
    {
        // clip���w�莞�Ԃ���Đ����āA��莞�ԂŎ~�߂�
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
        
        // �ꎞ�I��AudioClip�̃f�[�^�𕔕����o���čĐ�����
        int frequency = clip.frequency;
        int channels = clip.channels;

        int startSample = Mathf.FloorToInt(startTime * frequency);
        int lengthSamples = Mathf.FloorToInt(duration * frequency);

        if (startSample + lengthSamples > clip.samples)
            lengthSamples = clip.samples - startSample;

        // �ꎞ�f�[�^�̈�
        float[] data = new float[lengthSamples * channels];
        clip.GetData(data, startSample);

        // �V�����ꎞ�IAudioClip�𐶐�
        AudioClip tempClip = AudioClip.Create("Partial_" + clip.name, lengthSamples, channels, frequency, false);
        tempClip.SetData(data, 0);

        seSource.PlayOneShot(tempClip);

        yield return new WaitForSeconds(duration);
        Destroy(tempClip);
    }
    */
}