using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
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

    [Header("ステージ情報 (1始まり)")]
    public int groupNumber = 1;
    public int stageNumber = 1;

    [Header("ランクアニメーション")]
    public Animator rankAnimator;

    string rank;
    void Start()
    {
        ShowResult();
    }

    /*void ShowResult()
    {
        if (TimerManager.instance == null) return;

        int groupIndex = Mathf.Max(0, groupNumber - 1);
        int stageInGroup = Mathf.Max(0, stageNumber - 1);

        // ★ 個別ステージのタイムを取得
        //int index = groupIndex * TimerManager.instance.stagesPerGroup + stageInGroup;
        //float stageTime = TimerManager.instance.stageClearTimes[index];
        // ★ ここをあなたのコードに置き換える
        float totalTime = TimerManager.instance.stageClearTimes[groupIndex * TimerManager.instance.stagesPerGroup + stageInGroup];
        if (totalTime <= 0)
        {
            resultText.text = "ClearTime: --:--.--";
            rankText.text = "--";
            deathText.text = "Death: --";
            Debug.Log("まだこのステージはクリアされていません。");
            return;
        }

        resultText.text = "ClearTime: " + FormatTime(totalTime);
        deathText.text = "Death: " + TimerManager.instance.deathCounts[groupIndex * TimerManager.instance.stagesPerGroup + stageInGroup];

        ShowRank(totalTime);
    }*/

    void ShowResult()
    {
        if (TimerManager.instance == null) return;
        int groupIndex = groupNumber - 1;
        int stageInGroup = stageNumber - 1;
        //int groupIndex = Mathf.Max(0, groupNumber - 1);
        //int stageInGroup = Mathf.Max(0, stageNumber - 1);
        float totalTime = TimerManager.instance.GetTotalClearTime(groupIndex);
        totalTime = Mathf.Max(0, totalTime); //if (totalTime < 0) totalTime = 0;
        Debug.Log($"Result画面 totalTime={totalTime}");
        resultText.text = "ClearTime: " + FormatTime(totalTime);
        int deaths = TimerManager.instance.GetDeathCount(groupIndex * TimerManager.instance.stagesPerGroup + stageInGroup);
        deathText.text = "Death: " + deaths; 
        //ランク表示
        ShowRank(totalTime);
        
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
        switch (rank)
        {
            case "S": rankImage.sprite = rankS; break;
            case "A": rankImage.sprite = rankA; break;
            case "B": rankImage.sprite = rankB; break;
            case "C": rankImage.sprite = rankC; break;
        }
        PlayRankAnimation(rank);
    }
    void ShowRank(float totalTime)
    {
        if (totalTime <= 0)
        {
            rankText.text = "--";
            rankImage.sprite = null;
            Debug.Log("[ShowRank] totalTimeが0以下 → ランク判定スキップ");
            return;
        }

        // ランク判定
        rank = GetRank(totalTime);
        rankText.text = rank;
        Debug.Log($"[ShowRank] 判定されたランク: {rank}");

        // ランク画像表示
        ResultRank(rank);
    }
    public void PlayRankAnimation(string rank)
    {
        if (rankAnimator == null) return;
        rankAnimator.ResetTrigger("S");
        rankAnimator.ResetTrigger("A");
        rankAnimator.ResetTrigger("B");
        rankAnimator.ResetTrigger("C");

        rankAnimator.SetTrigger(rank);
        Debug.Log("ランクアニメーション再生: " + rank);
    }
    private string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 100f) % 100f);
        return $"{m}:{s:00}.{ms:00}";
    }
}
