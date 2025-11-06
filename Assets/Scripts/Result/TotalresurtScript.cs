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

    private bool resultShown = false;

    [Header("ランクアニメーション")]
    public Animator rankAnimator;

    void Start()
    {
        if (TimerManager.instance != null)
            TimerManager.instance.StopTimer();

        ShowResult();
    }

    void ShowResult()
    {
        if (resultShown || TimerManager.instance == null) return;
        resultShown = true;

        int groupIndex = groupNumber - 1;
        int stageInGroup = stageNumber - 1;

        float totalTime = TimerManager.instance.GetTotalClearTime(groupIndex);
        if (totalTime < 0) totalTime = 0;

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
        return "C";
    }

    // ランク画像セット
    void ResultRank(string rank)
    {
        //if (rankAnimator != null)
        //    rankAnimator.enabled = false;  // 一旦止める

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
        if (totalTime <= 0)
        {
            rankText.text = "--";
            rankImage.sprite = null;
            return;
        }

        string rank = GetRank(totalTime);
        rankText.text = rank;

        // 画像を適用
        ResultRank(rank);

        // アニメ再生
        PlayRankAnimation(rank);
    }

    public void PlayRankAnimation(string rank)
    {
        if (rankAnimator == null) return;

        //rankAnimator.enabled = true; // 復活させて…
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
