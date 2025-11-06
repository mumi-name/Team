using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    //private bool resultShown = false;

    [Header("ランクアニメーション")]
    public Animator rankAnimator;

    void Start()
    {
        ShowResult();
    }

    void ShowResult()
    {
        if (TimerManager.instance == null) return;

        int groupIndex = groupNumber - 1;
        int stageInGroup = stageNumber - 1;
        //int groupIndex = Mathf.Max(0, groupNumber - 1);
        //int stageInGroup = Mathf.Max(0, stageNumber - 1);

        float totalTime = TimerManager.instance.GetTotalClearTime(groupIndex);
        //totalTime = Mathf.Max(0, totalTime);        //if (totalTime < 0) totalTime = 0;
        Debug.Log($"Result画面 totalTime={totalTime}");

        if(totalTime <= 0)
        {
            resultText.text = "ClearTime: --:--.--";
            rankText.text = "--";
            Debug.Log("トータルタイムにマイナスが入ってます。");
            return;
        }
        resultText.text = "ClearTime: " + FormatTime(totalTime);
        int deaths = TimerManager.instance.GetDeathCount(groupIndex * TimerManager.instance.stagesPerGroup + stageInGroup);
        deathText.text = "Death: " + deaths;

        // ランク表示
        ShowRank(totalTime);
    }

    // ランク判定
    string GetRank(float time)
    {
        if (time <= 0) return "--";
        if (time < 60f) return "S";
        if (time < 120f) return "A";
        if (time < 180f) return "B";
        else return "C";
    }

    // ランク画像セット
    void ResultRank(string rank)
    {
        switch (rank)
        {
            case "S": rankImage.sprite = rankS; break;
            case "A": rankImage.sprite = rankA; break;
            case "B": rankImage.sprite = rankB; break;
            case "C": rankImage.sprite = rankC; break;
            default: rankImage.sprite = null; break;
        }
        //Debug.Log("ランクイメージに評価が入りました。");
        if (rankImage.sprite == null) Debug.Log("ランクイメージにスプライトが設定されていません。");
    }

    public void ShowRank(float totalTime)
    {
        if (totalTime <= 0)
        {
            rankText.text = "--";
            rankImage.sprite = null;
            Debug.Log("[ShowRank] totalTimeが0以下 → ランク判定スキップ");
            return;
        }
        Debug.Log("TotalTime" + totalTime);
        string rank = GetRank(totalTime);
        rankText.text = rank;
        Debug.Log($"[ShowRank] 判定されたランク: {rank}");
        // 画像を適用
        ResultRank(rank);

        // アニメ再生
        PlayRankAnimation(rank);
    }

    public void PlayRankAnimation(string rank)
    {
        if (rankAnimator == null) return;


       //rankAnimator.ResetTrigger("S");
       //rankAnimator.ResetTrigger("A");
       //rankAnimator.ResetTrigger("B");
       //rankAnimator.ResetTrigger("C");

        rankAnimator.SetTrigger(rank);
        Debug.Log("ランクアニメーションが起動しました。");
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
