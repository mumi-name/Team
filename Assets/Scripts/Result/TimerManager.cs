using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deathText;

    //[Header("設定値")]
    public int stagesPerGroup = 4; // ←★1グループ内のステージ数
    public int totalGroups = 3;    // ←★グループ数（ステージ数 = stagesPerGroup × totalGroups）

    [Header("タイム・死亡管理")]
    public float elapsedTime = 0f;
    private bool isRunning = false;
    public int deathCount = 0;

    public float[] stageClearTimes = new float[3]; // ステージごとのクリア時間
    public bool[] stageCleared;

    [SerializeField] public float[] stageTimes = new float[3]; // 3ステージの時間
    [SerializeField] public int[] deathCounts = new int[3];    // 3ステージの死亡数

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
        if (isRunning)
            elapsedTime += Time.unscaledDeltaTime;

        if (timerText != null)
            timerText.text = "Time: " + FormatTime(elapsedTime);

        if (deathText != null)
            deathText.text = "Deaths: " + deathCount;
    }

    public void StartTimer()
    {
        isRunning = true;
    }
    public void StopTimer()
    {
        isRunning = false;
    }
    public void AllCountReset()
    {
        elapsedTime = 0f;
        deathCount = 0;
        isRunning = true;
    }
    public void AddDeath() => deathCount++;

    // ■ クリア時に呼ぶ（ステージ保存）
    //public void SaveStageTime(int groupIndex, int stageIndex)
    //{
    //    int index = groupIndex * stagesPerGroup + stageIndex;
    //    Debug.Log($"SaveStageTime 呼ばれた: group={groupIndex}, stage={stageIndex}, index={index}, time={elapsedTime}");
    //    if (index < 0 || index >= stageClearTimes.Length)
    //    {
    //        Debug.LogError($"SaveStageTime: index {index} が範囲外です");
    //        return;
    //    }
    //    if(stageTimes == null || stageTimes.Length == 0)
    //    {
    //        int totalStages = totalGroups * stagesPerGroup;
    //        stageTimes = new float[totalStages];
    //        deathCounts = new int[totalStages];
    //    }
    //
    //    stageTimes[index] = elapsedTime;
    //    deathCounts[index] = deathCount;
    //    stageCleared[index] = true;
    //    GetTotalClearTime(groupIndex);
    //}
    public void SaveStageTime(int groupIndex, int stageIndex)
    {
        int index = groupIndex * stagesPerGroup + stageIndex;
        if (index < 0 || index >= stageClearTimes.Length) return;

        // ★ステージ単体のタイムではなく、グループ合計にするなら↓
        //float total = GetTotalClearTime(groupIndex);
        stageClearTimes[index] = elapsedTime;  // ←ここを変更(total)
        deathCounts[index] = deathCount;
        stageCleared[index] = true;

        GetTotalClearTime(groupIndex);
        Debug.Log($"【SaveStageTime】Group={groupIndex} Stage={stageIndex} Total={elapsedTime}");//total
    }
    public void SaveStageTimeAndUpdateTotal(int groupIndex, int stageIndex)
    {
        // まず今のステージ時間を保存
        //SaveStageTime(groupIndex, stageIndex);
        GetTotalClearTime(groupIndex);
        // ここで「このグループの合計タイム」を再計算する
        float total = GetTotalClearTime(groupIndex);

        Debug.Log($"【合計タイム更新】Group {groupIndex + 1} の合計 = {total}");
    }

    // ■グループの総合クリアタイム
    //public float GetTotalClearTime(int groupindex)
    //{
    //    float total = elapsedTime;
    //    for (int i = 0; i < stagesPerGroup; i++)
    //    {
    //        int index = (group - 1) * stagesPerGroup + i;
    //
    //        if (!stageCleared[index])return -1f; // まだ埋まってない → 表示しない
    //        total += stageClearTimes[index];
    //    }
    //    return total;
    //}

    
    public float GetTotalClearTime(int groupIndex)
    {
        float total = elapsedTime;
        bool anyCleared = false;

        groupIndex = Mathf.Clamp(groupIndex, 0, totalGroups - 1);
        // groupIndex が 0～(totalGroups-1) かチェック
        if (groupIndex < 0 || groupIndex >= totalGroups)
        {
            Debug.LogError($"グループ番号 {groupIndex} は存在しません（0～{totalGroups - 1}）");
            return 0f;
        }

        for (int i = 0; i < stagesPerGroup; i++)
        {
            int idx = groupIndex * stagesPerGroup + i;  // ←1次元配列に変換

            if (idx >= stageClearTimes.Length) continue;

            if (!stageCleared[idx]) return elapsedTime;

            if (stageCleared[idx])
            {
                total += stageClearTimes[idx];
                anyCleared = true;
            }
        }
        Debug.Log($"Total time={total}");
        return total; // クリアしてないなら -1を返す
    }

    private string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 100f) % 100f);
        return $"{m}:{s:00}.{ms:00}";
    }

    // すべてのステージ（全グループ含む）をクリア済みかどうか判定
    public bool IsAllStageCleared()
    {
        foreach (bool cleared in stageCleared)
        {
            if (!cleared) return false;
        }
        return true;
    }

    public float GetElapsedTime(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < stageTimes.Length)
        {
            return stageTimes[stageIndex];
        }
        return 0f;
    }

    public int GetDeathCount(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < deathCounts.Length)
        {
            return deathCounts[stageIndex];
        }
        return 0;
    }
}
