using TMPro;
using UnityEngine;

public class TimerUIInitializer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // ���̃V�[����Text
    public TextMeshProUGUI deathCount;
    void Start()
    {
        if (TimerManager.instance != null)
        {
            TimerManager.instance.SetText(timerText, deathCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
