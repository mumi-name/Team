using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TotalResultScript : MonoBehaviour
{
    [Header("UI関連")]
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

        //resultText.gameObject.SetActive(true);
        //deathText.gameObject.SetActive(true);

        ShowResult();
    }

    void ShowResult()
    {
        if (resultShown || TimerManager.instance == null) return;
        resultShown = true;

        int groupIndex = groupNumber - 1;
        int stageInGroup = stageNumber - 1;

        //ステージタイム保存（0始まりで渡す）
        TimerManager.instance.SaveStageTime(groupIndex, stageInGroup);

        //グループ合計タイム取得
        float totalTime = TimerManager.instance.GetTotalClearTime(groupIndex);

        //-1（まだデータなし）は0扱い
        if (totalTime < 0) totalTime = 0;

        //UI 表示
        resultText.text = "ClearTime: " + FormatTime(totalTime);

        int deaths = TimerManager.instance.GetDeathCount(groupIndex * TimerManager.instance.stagesPerGroup + stageInGroup);
        deathText.text = "Death: " + deaths;

        //ランク表示
        ShowRank(totalTime);
    }


    string GetRank(float time)
    {
        if (time <= 0) return "--";
        if (time < 60f) return "S";
        if (time < 120f) return "A";
        if (time < 180f) return "B";
        return "C";
    }

    void ResultRank(string rank)
    {
        if (rank == null)
        {
            Debug.Log("変数　rankに何も入ってません");
            return;
        }
            

        switch (rank)
        {
            case "S": rankImage.sprite = rankS; break;
            case "A": rankImage.sprite = rankA; break;
            case "B": rankImage.sprite = rankB; break;
            case "C": rankImage.sprite = rankC; break;
            default: rankImage.sprite = null; break;
        }
    }
    public void ShowRank(float totalTime)
    {
        Debug.Log($"[ShowRank] totalTime = {totalTime}");

        if (rankText == null) Debug.Log("rankText がセットされていません。");
        if (rankImage == null) Debug.Log("rankImage がセットされていません。");

        if (totalTime <= 0)
        {
            Debug.Log("クリア時間がありません。（totalTime <= 0）");
            rankText.text = "--";
            rankImage.sprite = null;
            return;
        }

        string rank = GetRank(totalTime);
        Debug.Log($"[ShowRank] rank = {rank}");
        //テキストと画像セット
        rankText.text = rank;
        ResultRank(rank);

        Debug.Log($"rankText.text = {rankText.text}");
        Debug.Log($"rankImage.sprite = {rankImage?.sprite}");
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
