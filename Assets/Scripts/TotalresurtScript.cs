using UnityEngine;
using TMPro;
public class TotalresurtScript : MonoBehaviour
{
    public GameObject resultPanel;　　//黒幕パネル
    public TextMeshProUGUI resultText;//時間表示テキスト

    public float Keikajikan = 3f;
    private float timer = 0f;
    private bool resultShow = false;
    void Start()
    {
        resultPanel.SetActive(false);
        float finalTime = GameManager.instance.finalTime;
        resultText.text = "Time:" + FormatTime(finalTime);
    }

    public void ShowResult(float finalTime)
    {
        resultPanel.SetActive(true); // 黒幕を表示

        // 経過時間を表示
        resultText.text = "Time: " + FormatTime(finalTime);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100F) % 100F);

        return string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= Keikajikan)
        {
            ShowResult(timer);
            resultShow = true;
        }
    }
}
