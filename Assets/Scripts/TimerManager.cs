using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float startTime;
    private float elapsedTime;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = Time.time - startTime;//�����v�Z���Čo�ߎ��Ԃ��o���B

        string formattedTime = FormatTime(elapsedTime);//FormatTime�֐��������ƃG���[�f��
        // �e�L�X�g���X�V����
        if (timerText != null)
        {
            timerText.text = "Time:" + formattedTime;
        }

        // �o�ߎ��Ԃ�:�b.�~���b�̌`���ɕϊ�����֐�
        string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
            int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);

            // �[�����߂��s��
            return string.Format("{0}:{1}.{2}", minutes, seconds, milliseconds);
        }
    }
}
