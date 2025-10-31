using UnityEngine;
using TMPro;

public class TotalResultScript : MonoBehaviour
{
    public GameObject resultPanel;      // 黒幕パネル
    public TextMeshProUGUI resultText;  // 時間表示テキスト
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rankText;

    public float delayTime = 3f;         // 黒幕が出るまでの待ち時間
    private bool resultShown = false;   // 一度だけ表示するフラグ
    private string rank;

    void Start()
    {
        if (TimerManager.instance != null) TimerManager.instance.StopTimer();
        // 最初にテキスト表示
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

        // 黒幕パネルを表示
        //resultPanel.SetActive(true);

        // ステージの総経過時間を取得
        float currentTime = TimerManager.instance != null ? TimerManager.instance.GetElapsedTime() : 0f;
        int deaths = TimerManager.instance != null ? TimerManager.instance.GetDeathCount() : 0;
        // リザルトテキストを表示
        resultText.gameObject.SetActive(true);
        resultText.text = "Time: " + FormatTime(currentTime);
        deathText.text = "Death:" + deaths.ToString();
        //やるならランク表示
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
        //画像を入れるやつ
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
    //public void HideResult()//リザルトを閉じる
    //{
    //    resultPanel.SetActive(false);
    //    resultText.gameObject.SetActive(false);
    //
    //    //タイマー再開
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
