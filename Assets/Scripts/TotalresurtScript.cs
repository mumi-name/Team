using UnityEngine;
using TMPro;

public class TotalResultScript : MonoBehaviour
{
    public GameObject resultPanel;      // 黒幕パネル
    public TextMeshProUGUI resultText;  // 時間表示テキスト

    public float delayTime = 3f;         // 黒幕が出るまでの待ち時間
    private bool resultShown = false;   // 一度だけ表示するフラグ

    void Start()
    {
        // 最初は黒幕とテキスト非表示
        resultPanel.SetActive(false);
        resultText.gameObject.SetActive(false);

        // delayTime秒後にリザルトを表示
        Invoke("ShowResult", delayTime);
    }

    void ShowResult()
    {
        if (resultShown) return;
        resultShown = true;
        if(TimerManager.instance != null) TimerManager.instance.StopTimer();


        // 黒幕パネルを表示
        resultPanel.SetActive(true);

        // ステージの総経過時間を取得
        float currentTime = TimerManager.instance != null ? TimerManager.instance.GetElapsedTime() : 0f;
        int deaths = TimerManager.instance != null ? TimerManager.instance.GetDeathCount() : 0;
        // リザルトテキストを表示
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
