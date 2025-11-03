using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    [Header("UI関連")]
    public GameObject resultPanel;      // 黒幕パネル
    public TextMeshProUGUI resultText;  // ステージタイム表示
    public TextMeshProUGUI deathText;   // 死亡回数
    public TextMeshProUGUI rankText;    // ランク文字列
    public Image rankImage;             // ランク画像

    [Header("ランク画像")]
    public Sprite rankS;
    public Sprite rankA;
    public Sprite rankB;
    public Sprite rankC;

    [Header("ステージ情報")]
    public int groupNumber = 1;         // グループ番号
    public int stageNumber = 1;         // グループ内ステージ番号
    public int stagesPerGroup = 3;      // グループ内ステージ数（可変）

    private bool resultShown = false;

    void Start()
    {
        if (TimerManager.instance != null)
            TimerManager.instance.StopTimer();

        resultText.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);
        if (rankText != null) rankText.gameObject.SetActive(true);
        if (rankImage != null) rankImage.gameObject.SetActive(true);

        ShowResult();
    }

    void ShowResult()
    {
        if (resultShown) return;
        resultShown = true;

        // 現在のステージタイムを取得
        float currentTime = TimerManager.instance != null
            ? TimerManager.instance.GetElapsedTime()
            : 0f;
        int deaths = TimerManager.instance != null
            ? TimerManager.instance.GetDeathCount()
            : 0;

        // ステージタイムを保存
        SaveClearTime(currentTime);

        // ステージタイムと死亡回数表示
        resultText.text = "Time: " + FormatTime(currentTime);
        deathText.text = "Death: " + deaths;

        // ランク表示
        string rank = GetRank(currentTime);
        rankText.text = "RANK: " + rank;
        SetRankImage(rank);

        // グループ全ステージクリア済みなら総合タイム表示
        string totalKey = $"Stage{groupNumber}_TotalTime";
        if (PlayerPrefs.HasKey(totalKey))
        {
            float totalTime = PlayerPrefs.GetFloat(totalKey);
            resultText.text += "\nTotal Time: " + FormatTime(totalTime);
        }
    }

    string GetRank(float time)
    {
        if (time < 60f) return "S";
        if (time < 120f) return "A";
        if (time < 180f) return "B";
        return "C";
    }

    void SetRankImage(string rank)
    {
        if (rankImage == null) return;
        switch (rank)
        {
            case "S": rankImage.sprite = rankS; break;
            case "A": rankImage.sprite = rankA; break;
            case "B": rankImage.sprite = rankB; break;
            case "C": rankImage.sprite = rankC; break;
            default: rankImage.sprite = null; break;
        }
    }

    void SaveClearTime(float clearTime)
    {
        string key = $"Stage{groupNumber}-{stageNumber}_Time";
        PlayerPrefs.SetFloat(key, clearTime);

        // グループ内の総合タイムを更新
        UpdateTotalClearTime();

        PlayerPrefs.Save();
    }

    void UpdateTotalClearTime()
    {
        float total = 0f;
        bool allCleared = true;

        for (int i = 1; i <= stagesPerGroup; i++)
        {
            string key = $"Stage{groupNumber}-{i}_Time";
            if (PlayerPrefs.HasKey(key))
                total += PlayerPrefs.GetFloat(key);
            else
                allCleared = false;
        }

        // 全てクリア済みなら総合タイムも保存
        if (allCleared)
        {
            string totalKey = $"Stage{groupNumber}_TotalTime";
            PlayerPrefs.SetFloat(totalKey, total);
            PlayerPrefs.Save();
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
        return $"{minutes}:{seconds:00}.{milliseconds:00}";
    }
}
