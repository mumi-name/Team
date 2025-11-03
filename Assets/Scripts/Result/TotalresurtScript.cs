using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TotalResultScript : MonoBehaviour
{
    [Header("UI関連")]
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rankText;
    public Image rankImage;

    [Header("ランク画像")]
    public Sprite rankS;
    public Sprite rankA;
    public Sprite rankB;
    public Sprite rankC;

    [Header("ステージ情報")]
    public int groupNumber = 1;
    public int stageNumber = 1;

    private bool resultShown = false;

    void Start()
    {
        if (TimerManager.instance != null)
            TimerManager.instance.StopTimer();

        resultText.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);

        ShowResult();
    }

    void ShowResult()
    {
        if (resultShown) return;
        resultShown = true;

        if (TimerManager.instance == null) return;

        // 現在のステージインデックス
        int stageIndex = (groupNumber - 1) * TimerManager.instance.stagesPerGroup + (stageNumber - 1);

        // ステージ内インデックス
        int stageInGroup = stageNumber - 1;

        // 現在のステージタイムを取得
        float currentTime = TimerManager.instance.GetElapsedTime(stageIndex);
        int deaths = TimerManager.instance.GetDeathCount(stageIndex);

        // ステージタイムを保存
        TimerManager.instance.SaveStageTime(stageIndex, stageInGroup);

        // ステージタイム・死亡回数表示
        resultText.text = "Time: " + FormatTime(currentTime);
        deathText.text = "Death: " + deaths;

        // ランク表示
        string rank = GetRank(currentTime);
        rankText.text = "RANK: " + rank;
        ResultRank(rank);

        // グループ全ステージクリア済みなら総合タイム表示
        float totalTime = TimerManager.instance.GetTotalClearTime(groupNumber - 1);
        resultText.text += "\nTotal Time: " + FormatTime(totalTime);
    }

    string GetRank(float time)
    {
        if (time < 60f) return "S";
        if (time < 120f) return "A";
        if (time < 180f) return "B";
        return "C";
    }

    void ResultRank(string rank)
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

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
