using UnityEngine;
using System.Collections.Generic;//���X�g�g�p�̍�
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

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
    public void PlaySE(string name)
    {
        //��񂾂��̏ꍇ
        if (bgmDict.ContainsKey(name))
        {
            seSource.PlayOneShot(seDict[name]);
        }
        else
        {
            Debug.Log($"SE'{name}'��������܂���B");
        }
    }
}