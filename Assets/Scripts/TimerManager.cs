using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float startTime;
    private float elapsedTime;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = Time.time - startTime;//差を計算して経過時間を出す。

        string formattedTime = FormatTime(elapsedTime);//FormatTime関数が無いとエラー吐く
        // テキストを更新する
        if (timerText != null)
        {
            timerText.text = "Time:" + formattedTime;
        }

        // 経過時間を分:秒.ミリ秒の形式に変換する関数
        string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
            int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);

            // ゼロ埋めを行う
            return string.Format("{0}:{1}.{2}", minutes, seconds, milliseconds);
        }
    }
}
