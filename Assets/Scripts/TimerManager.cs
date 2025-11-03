using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [Header("設定")]
    public int stagesPerGroup = 3;
    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deathText;

    public float elapsedTime = 0f; // 経過時間
    private bool isRunning = false;  // タイマーが動いているか

    public int deathCount = 0;     //死亡、リセット回数
    public float[] stageClearTimes = new float[3]; // 1,2,3ステージ分
    public bool[] stageCleared = new bool[3];

    void Awake()
    {
        // シングルトン設定
        if (instance != null&& instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //初回生成時
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 初期化はここで1度だけ
        if (stageClearTimes == null || stageClearTimes.Length == 0)
        {
            int totalStages = 6; // 例: 2グループ×3ステージ
            stageClearTimes = new float[totalStages];
            stageCleared = new bool[totalStages];
        }
    }
    void Start()
    {
        if (instance != null) StartTimer();
        //deathText.gameObject.SetActive(false);
    }
    void Update()
    {
        // タイマーが動いている場合、経過時間を加算
        if (isRunning)
        {
            //elapsedTime += Time.deltaTime;
            elapsedTime += Time.unscaledDeltaTime;
        }

        // UI表示更新
        if (timerText != null)
        {
            timerText.text = "Time: " + FormatTime(elapsedTime);
        }

        if (deathText != null)
        {
            deathText.text = "Deaths:" + FormatTime(deathCount);
        }
        // デバッグ用キー操作
        //if (Input.GetKeyDown(KeyCode.T)) StartTimer();
        //if (Input.GetKeyDown(KeyCode.S)) StopTimer();
        //if (Input.GetKeyDown(KeyCode.R)) ResetTimer();
    }

    // タイマー操作
    public void StartTimer()
    {
        isRunning = true;
    }
    public void StopTimer()
    {
        isRunning = false;
    }

    // タイマーリセット＋死亡回数カウント
    public void AllCountReset()
    {
        deathCount = 0;
        elapsedTime = 0f;
        if (timerText != null)
            timerText.text = "Time: 0:00.00";
        isRunning = true;
    }

    //外部から死亡を通知する関数
    public void AddDeath()
    {
        deathCount++;
    }

    // フォーマット変換 (分:秒.ミリ秒)
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);

        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    // 新しいシーンでUIを再アサインする場合
    public void SetText(TextMeshProUGUI newText, TextMeshProUGUI newDeath, TextMeshProUGUI newRank)
    {
        timerText = newText;
        if (newDeath != null) deathText = newDeath;
    }

    // 他のスクリプトから時間・回数を取得可能
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public int GetDeathCount()
    {
        return deathCount;
    }

    public void SaveStageTime(int stageIndex)
    {
        if (stageClearTimes == null || stageIndex < 0 || stageIndex >= stageClearTimes.Length)
        {
            Debug.LogError($"SaveStageTime: stageIndex {stageIndex} が配列範囲外です");
            return;
        }
        stageClearTimes[stageIndex] = elapsedTime;
        stageCleared[stageIndex] = true;
    }

    public float GetTotalClearTime(int groupIndex)
    {
        float total = 0;
        int startIndex = groupIndex * 3; // 1グループ3ステージ
        for (int i = 0; i < 3; i++)
        {
            if (startIndex + i < stageClearTimes.Length && stageCleared[startIndex + i])
                total += stageClearTimes[startIndex + i];
        }
        return total;
    }

    public bool IsAllStageCleared()
    {
        return stageCleared[0] && stageCleared[1] && stageCleared[2];
    }
}
