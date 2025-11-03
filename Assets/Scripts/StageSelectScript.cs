using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageSelectScript : MonoBehaviour
{
    [Header("左側UI（ステージアイコン関連）")]
    public RectTransform[] stageIcons;   // 1,2,3 のステージアイコン(位置だけ使う)
    public RectTransform selectorFrame;  // 枠(カーソル)
    public TextMeshProUGUI stageNameText;

    [Header("右側UI（情報表示）")]
    public Image previewImage;           // ステージのプレビュー
    public TextMeshProUGUI clearTimeText; // 総クリア時間（全ステージ合計）

    [Header("データ")]
    public Sprite[] previewSprites;      // プレビュー画像（ステージ1〜）
    public string[] stageSceneName;      // シーン名（例："Stage1", "Stage2"...）

    [Header("アニメーター")]
    public Animator[] stageAnimators;
    private int currentIndex = 0;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        // 上移動
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateSelection();
        }
        // 下移動
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = Mathf.Min(stageIcons.Length - 1, currentIndex + 1);
            UpdateSelection();
        }
        // スペースでステージへ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // カーソルがあるアイコンだけ光らせる
            for (int i = 0; i < stageAnimators.Length; i++)
            {
                if (stageAnimators[i] != null)
                {
                    stageAnimators[i].SetBool("Icon", (i == currentIndex));
                }
            }
            //if (stageAnimators.Length > currentIndex && stageAnimators[currentIndex] != null)
            //{
            //    stageAnimators[0].SetBool("Icon",true);
            //}
            if (stageSceneName.Length > currentIndex)
            {
                SceneManager.LoadScene(stageSceneName[currentIndex]);
            }
            else
            {
                Debug.Log($"stageSceneName にシーン名が入っていません index={currentIndex}");
            }
        }
    }

    /// <summary>
    /// 選択ステージが変わるたびに呼び出される
    /// </summary>
    void UpdateSelection()
    {
        //if (currentIndex < previewSprites.Length && previewSprites[currentIndex] != null) return;

        // 親が違っても確実に同じ場所に移動できる
        //selectorFrame.position = stageIcons[currentIndex].position;
        //selectorFrame.localScale = Vector3.one; // 念のためスケールを戻す

        if (stageIcons == null || stageIcons.Length == 0)
        {
            Debug.LogError("stageIcons が空または未設定です");
            return;
        }
        if (previewSprites == null || previewSprites.Length == 0)
        {
            Debug.LogError("previewSprites が空または未設定です");
            return;
        }

        if (currentIndex < 0) currentIndex = stageIcons.Length - 1;
        if (currentIndex >= stageIcons.Length) currentIndex = 0;
        // 枠をステージアイコン位置へ
        //selectorFrame.position = stageIcons[currentIndex].position;
        selectorFrame.anchoredPosition = stageIcons[currentIndex].anchoredPosition;

        // 右側にプレビュー画像表示
        if (currentIndex < previewSprites.Length && previewSprites[currentIndex] != null)
        {
            previewImage.sprite = previewSprites[currentIndex];
            previewImage.gameObject.SetActive(true);
        }
        else
        {
            previewImage.gameObject.SetActive(false);
        }
        //総クリア時間だけ表示（ステージごとのタイムではない）
        float totalTime = 0f;
        int groupIndex = currentIndex / TimerManager.instance.stagesPerGroup;
        if (TimerManager.instance != null)
        {
            totalTime = TimerManager.instance.GetTotalClearTime(groupIndex); // ← 全ステージの合計時間
        }
        clearTimeText.text = "Total Time: " + FormatTime(totalTime);
    }

    string FormatTime(float time)
    {
        int m = Mathf.FloorToInt(time / 60f);
        int s = Mathf.FloorToInt(time % 60f);
        int ms = Mathf.FloorToInt((time * 100f) % 100f);
        return $"{m}:{s:00}.{ms:00}";
    }
}