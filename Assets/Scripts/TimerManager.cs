using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    public float elapsedTime = 0f; // �o�ߎ���
    private bool isRunning = false;  // �^�C�}�[�������Ă��邩

    public int deathCount = 0;     //���S�A���Z�b�g��

    void Awake()
    {
        // �V���O���g���ݒ�
        if (instance != null&& instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //���񐶐���
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if (instance != null) StartTimer();
    }
    void Update()
    {
        // �^�C�}�[�������Ă���ꍇ�A�o�ߎ��Ԃ����Z
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }

        // UI�\���X�V
        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(elapsedTime);
        }

        // �f�o�b�O�p�L�[����
        //if (Input.GetKeyDown(KeyCode.T)) StartTimer();
        //if (Input.GetKeyDown(KeyCode.S)) StopTimer();
        //if (Input.GetKeyDown(KeyCode.R)) ResetTimer();
    }

    // �^�C�}�[�J�n
    public void StartTimer()
    {
        isRunning = true;
    }

    // �^�C�}�[��~
    public void StopTimer()
    {
        isRunning = false;
    }

    // �^�C�}�[���Z�b�g�{���S�񐔃J�E���g
    public void ResetTimer()
    {
        elapsedTime = 0f;
        deathCount++;
        if (timerText != null)
            timerText.text = "Time: 0:00.00";
    }

    //�O�����玀�S��ʒm����֐�
    public void AddDeath()
    {
        deathCount++;
    }

    // �t�H�[�}�b�g�ϊ� (��:�b.�~���b)
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);

        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    // �V�����V�[����UI���ăA�T�C������ꍇ
    public void SetTimerText(TextMeshProUGUI newText)
    {
        timerText = newText;
    }

    // ���̃X�N���v�g����o�ߎ��Ԃ��擾�\
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public int GetDeathCount()
    {
        return deathCount;
    }
}
