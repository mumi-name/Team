using UnityEngine;
using TMPro;

public class TotalResultScript : MonoBehaviour
{
    public GameObject resultPanel;      // �����p�l��
    public TextMeshProUGUI resultText;  // ���ԕ\���e�L�X�g
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rankText;

    public float delayTime = 3f;         // �������o��܂ł̑҂�����
    private bool resultShown = false;   // ��x�����\������t���O
    private string rank;

    void Start()
    {
        if (TimerManager.instance != null) TimerManager.instance.StopTimer();
        // �ŏ��Ƀe�L�X�g�\��
        deathText.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
        AudioManager.instance.PlayBGM("BGM3");
        AudioManager.instance.PlaySE2("BGM_last");
        ShowResult();
        //Invoke("ShowResult", delayTime);
    }

    void ShowResult()
    {
        if (resultShown) return;
        resultShown = true;

        // �����p�l����\��
        //resultPanel.SetActive(true);

        // �X�e�[�W�̑��o�ߎ��Ԃ��擾
        float currentTime = TimerManager.instance != null ? TimerManager.instance.GetElapsedTime() : 0f;
        int deaths = TimerManager.instance != null ? TimerManager.instance.GetDeathCount() : 0;
        // ���U���g�e�L�X�g��\��
        resultText.gameObject.SetActive(true);
        resultText.text = "Time: " + FormatTime(currentTime);
        deathText.text = "Death:" + deaths.ToString();
        //���Ȃ烉���N�\��
        rank = GetRank(currentTime);
        //rankText.text = "RANK:" + rank;
        ResultRank(rank);
    }
    string GetRank(float time)
    {
        if (time < 60f) return "S";
        if (time < 120f) return "A";
        if (time < 180f) return "B";
        else return "C";
    }
    void ResultRank(string rank)
    {
        //�摜��������
        switch (rank)
        {
            case "S":
                rankImage.sprite = rankS;
                break;
            case "A":
                rankImage.sprite = rankA;
                break;
            case "B":
                rankImage.sprite = rankB;
                break;
            case "C":
                rankImage.sprite = rankC;
                break;
            default:
                rankImage.sprite = null;
                break;
        }
    }
    //public void HideResult()//���U���g�����
    //{
    //    resultPanel.SetActive(false);
    //    resultText.gameObject.SetActive(false);
    //
    //    //�^�C�}�[�ĊJ
    //    if (TimerManager.instance != null) TimerManager.instance.StartTimer();
    //}

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);
        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
