using TMPro;
using UnityEngine;

public class TimerUIInitializer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // ÇªÇÃÉVÅ[ÉìÇÃText
    void Start()
    {
        if (TimerManager.instance != null)
        {
            TimerManager.instance.SetTimerText(timerText);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
