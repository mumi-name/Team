using UnityEngine;
using TMPro;

public class TotalResultScript : MonoBehaviour
{
    public GameObject resultPanel;      // �����p�l��
    public TextMeshProUGUI resultText;  // ���ԕ\���e�L�X�g

    public float delayTime = 3f;         // �������o��܂ł̑҂�����
    private bool resultShown = false;   // ��x�����\������t���O

    void Start()
    {
        // �ŏ��͍����ƃe�L�X�g��\��
        resultPanel.SetActive(false);
        resultText.gameObject.SetActive(false);

        // delayTime�b��Ƀ��U���g��\��
        Invoke("ShowResult", delayTime);
    }

    void ShowResult()
    {
        if (resultShown) return;
        resultShown = true;
        if(TimerManager.instance != null) TimerManager.instance.StopTimer();


        // �����p�l����\��
        resultPanel.SetActive(true);

        // �X�e�[�W�̑��o�ߎ��Ԃ��擾
        float currentTime = TimerManager.instance != null ? TimerManager.instance.GetElapsedTime() : 0f;
        int deaths = TimerManager.instance != null ? TimerManager.instance.GetDeathCount() : 0;
        // ���U���g�e�L�X�g��\��
        resultText.gameObject.SetActive(true);
        resultText.text = 
            "Time: " + FormatTime(currentTime)+ "\n"+ "Deaths" + deaths;
        
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);
        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
