using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    public TextMeshProUGUI timerText;
    
    private float startTime;
    public float elapsedTime { get; private set; }

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            startTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(elapsedTime);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);

        return string.Format("{0}:{1}.{2}", minutes, seconds, milliseconds);
    }

    //新しいシーンでUIを再アサイン用の関数
    public void SetTimerText(TextMeshProUGUI newText)
    {
        timerText = newText;
    }
}
