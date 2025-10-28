using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deathText;

    public float elapsedTime = 0f; // 経過時間
    private bool isRunning = false;  // タイマーが動いているか

    public int deathCount = 0;     //死亡、リセット回数

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
    }
    void Start()
    {
        if (instance != null) StartTimer();
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
            deathText.text = "Deaths:" + deathCount.ToString();
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
    public void SetTimerText(TextMeshProUGUI newText)
    {
        timerText = newText;
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
}
