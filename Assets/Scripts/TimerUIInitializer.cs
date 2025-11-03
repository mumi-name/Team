using TMPro;
using UnityEngine;

public class TimerUIInitializer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // ÇªÇÃÉVÅ[ÉìÇÃText
    public TextMeshProUGUI deathCount;
    public TextMeshProUGUI rankText;
    void Start()
    {
        if (TimerManager.instance != null)
        {
            TimerManager.instance.SetText(timerText, deathCount,rankText);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
