using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TotalResultScript : MonoBehaviour
{
    [Header("UI関連")]
    public GameObject resultPanel;      // 黒幕パネル
    public TextMeshProUGUI resultText;  // 時間表示テキスト
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rankText;
    public Image rankImage;

    [Header("演出関連")]
    public float delayTime = 3f;         // 黒幕が出るまでの待ち時間
    private bool resultShown = false;   // 一度だけ表示するフラグ
    private string rank;

    [Header("ランク画像")]
    public Sprite rankS;
    public Sprite rankA;
    public Sprite rankB;
    public Sprite rankC;

    [Header("ステージ情報")]
    public int groupNumber = 1;         //ステージグループ
    public int stageNumber = 1;         //グループ内の各ステージ
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

        // ステージの総経過時間を取得
        float currentTime = TimerManager.instance != null ? TimerManager.instance.GetElapsedTime() : 0f;
        int deaths = TimerManager.instance != null ? TimerManager.instance.GetDeathCount() : 0;

        // 今クリアしたステージ番号を保存（例: Stage1なら index = 0）
        int stageIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (TimerManager.instance != null &&stageIndex >= 0 &&stageIndex < TimerManager.instance.stageClearTimes.Length)
        {
            TimerManager.instance.SaveStageTime(stageIndex);
        }
        resultText.text = "Time: " + FormatTime(currentTime);
        deathText.text = "Death:" + TimerManager.instance.GetDeathCount();

        // 全部クリアしたら総合時間も表示
        if (TimerManager.instance.IsAllStageCleared())
        {
            //float total = TimerManager.instance.GetTotalClearTime();
            //resultText.text += "\nTotal Time: " + FormatTime(total);
        }
        // リザルトテキストを表示
        resultText.gameObject.SetActive(true);
        resultText.text = "Time: " + FormatTime(currentTime);
        deathText.text = "Death:" + deaths.ToString();
        //ランク表示
        rank = GetRank(currentTime);
        //rankText.text = "RANK:" + rank;
        ResultRank(rank);

        //クリアタイム保存
        SaveClearTime(currentTime);

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
        /*
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
        */
    }
    //public void HideResult()//リザルトを閉じる
    //{
    //    resultPanel.SetActive(false);
    //    resultText.gameObject.SetActive(false);
    //
    //    //タイマー再開
    //    if (TimerManager.instance != null) TimerManager.instance.StartTimer();
    //}

    void SaveClearTime(float clearTime)
    {
        string key = $"Stage{groupNumber}-{stageNumber}_Time";
        PlayerPrefs.SetFloat(key, clearTime);

        // グループ内の総時間を更新
        UpdateTotalClearTime(groupNumber);

        PlayerPrefs.Save();
    }

    void UpdateTotalClearTime(int group)
    {
        float total = 0f;
        bool allCleared = true;

        // 例：グループ1には3ステージあると仮定
        for (int i = 1; i <= 3; i++)
        {
            string key = $"Stage{group}-{i}_Time";
            if (PlayerPrefs.HasKey(key))
            {
                total += PlayerPrefs.GetFloat(key);
            }
            else
            {
                allCleared = false;
            }
        }

        // すべてクリア済みなら合計を保存
        if (allCleared)
        {
            string totalKey = $"Stage{group}_TotalTime";
            PlayerPrefs.SetFloat(totalKey, total);
            //PlayerPrefs.Save();
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);
        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
