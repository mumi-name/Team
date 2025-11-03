using TMPro;
using UnityEngine;

public class TimerUIInitializer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // ÇªÇÃÉVÅ[ÉìÇÃText
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rankText;
    void Start()
    {
        if (TimerManager.instance != null)
        {
            TimerManager.instance.timerText = timerText;
            TimerManager.instance.deathText = deathText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
