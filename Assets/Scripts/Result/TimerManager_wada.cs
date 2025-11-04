using UnityEngine;
using TMPro;

public class TimerManager_wada : MonoBehaviour
{
    public static TimerManager_wada instance;

    [Header("UI")]
    public TextMeshProUGUI timerText;        // ステージ内タイマー表示
    public TextMeshProUGUI deathText;        // ステージ内死亡回数表示
    public TextMeshProUGUI resultTimeText;   // リザルト用総タイム
    public TextMeshProUGUI resultDeathText;  // リザルト用死亡回数
    public TextMeshProUGUI resultRankText;   // リザルト用ランク

    [Header("設定値")]
    public int stagesPerGroup = 4;
    public int totalGroups = 3;

    [Header("内部データ")]
    private float elapsedTime = 0f;
    private bool isRunning = false;
    private int deathCount = 0;

    private float[] stageClearTimes;
    private bool[] stageCleared;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        int totalStages = stagesPerGroup * totalGroups;
        stageClearTimes = new float[totalStages];
        stageCleared = new bool[totalStages];
    }

    void Update()
    {
        if (isRunning) elapsedTime += Time.unscaledDeltaTime;

        if (timerText != null) timerText.text = "Time: " + FormatTime(elapsedTime);
        if (deathText != null) deathText.text = "Deaths: " + deathCount;
    }

    // =============================
    // タイマー・死亡管理
    // =============================
    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public void AddDeath()
    {
        deathCount++;
        if (deathText != null)
            deathText.text = "Deaths: " + deathCount;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        deathCount = 0;
        isRunning = true;
    }

    public void SaveStageTime(int groupIndex, int stageIndex)
    {
        int index = groupIndex * stagesPerGroup + stageIndex;
        if (index < 0 || index >= stageClearTimes.Length)
        {
            Debug.LogError($"SaveStageTime: index {index} が範囲外です");
            return;
        }

        stageClearTimes[index] = elapsedTime;
        stageCleared[index] = true;
    }

    public float GetTotalClearTime_Safe(int groupIndex)
    {
        groupIndex = Mathf.Clamp(groupIndex, 0, totalGroups - 1);
        float total = 0f;
        for (int i = 0; i < stagesPerGroup; i++)
        {
            int idx = groupIndex * stagesPerGroup + i;
            if (idx < stageClearTimes.Length && stageCleared[idx])
                total += stageClearTimes[idx];
        }
        return total;
    }

    public int GetDeathCount() => deathCount;

    // =============================
    // リザルト表示
    // =============================
    public void ShowResult(int groupIndex)
    {
        if (resultTimeText == null || resultDeathText == null || resultRankText == null)
        {
            Debug.LogError("リザルトUIがアサインされていません");
            return;
        }

        float totalTime = GetTotalClearTime_Safe(groupIndex);
        bool anyCleared = false;

        for (int i = 0; i < stagesPerGroup; i++)
        {
            int idx = groupIndex * stagesPerGroup + i;
            if (idx < stageCleared.Length && stageCleared[idx])
            {
                anyCleared = true;
                break;
            }
        }

        resultTimeText.text = anyCleared ? "Total Time: " + FormatTime(totalTime) : "Total Time: --:--.--";
        resultDeathText.text = "Deaths: " + deathCount;
        resultRankText.text = CalculateRank(totalTime, deathCount);
    }

    // =============================
    // 補助
    // =============================
    string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 100f) % 100f);
        return $"{m}:{s:00}.{ms:00}";
    }

    string CalculateRank(float totalTime, int deathCount)
    {
        if (totalTime == 0) return "--";
        if (totalTime < 60f && deathCount == 0) return "S";
        if (totalTime < 120f) return "A";
        if (totalTime < 180f) return "B";
        return "C";
    }
}
