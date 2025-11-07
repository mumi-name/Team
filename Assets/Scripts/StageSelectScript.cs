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
    public TextMeshProUGUI clearTimeText; // 総クリア時間（グループ全体）

    [Header("データ")]
    public Sprite[] previewSprites;      // プレビュー画像（ステージ1〜）
    public string[] stageSceneName;      // シーン名（例："Stage1", "Stage2"...）

    [Header("アニメーター")]
    public Animator[] stageAnimators;

    private int currentIndex = 0;
    private float inputDelay = 0.2f;  // 連続入力防止用
    private float inputTimer = 0f;

    //追加
    //private float previousGroupIndex;
    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        inputTimer += Time.deltaTime;

        float vertical = Input.GetAxisRaw("Vertical");
        // 上移動
        if (inputTimer >= inputDelay)
        {
            if (Input.GetKeyDown(KeyCode.W) || vertical > 0.5f) // 上
            {
                currentIndex = Mathf.Max(0, currentIndex - 1);
                UpdateSelection();
                inputTimer = 0f;
            }
            if (Input.GetKeyDown(KeyCode.S) || vertical < -0.5f) // 下
            {
                currentIndex = Mathf.Min(stageIcons.Length - 1, currentIndex + 1);
                UpdateSelection();
                inputTimer = 0f;
            }
        }
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    currentIndex = Mathf.Max(0, currentIndex - 1);
        //    UpdateSelection();
        //}
        // 下移動
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    currentIndex = Mathf.Min(stageIcons.Length - 1, currentIndex + 1);
        //    UpdateSelection();
        //}
        // スペースでステージへ
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Select"))
        {
            for (int i = 0; i < stageAnimators.Length; i++)
            {
                if (stageAnimators[i] != null)
                {
                    stageAnimators[i].SetBool("Icon", (i == currentIndex));
                }
            }

            if (stageSceneName.Length > currentIndex)
            {
                SceneManager.LoadScene(stageSceneName[currentIndex]);
            }
            else
            {
                Debug.LogWarning($"stageSceneName にシーン名が入っていません index={currentIndex}");
            }
        }
    }

    /// <summary>
    /// 選択ステージが変わるたびに呼び出される
    /// </summary>
    void UpdateSelection()
    {
        if (stageIcons == null || stageIcons.Length == 0) return;

        // 枠をステージアイコン位置に
        if (selectorFrame != null)
            selectorFrame.anchoredPosition = stageIcons[currentIndex].anchoredPosition;

        // プレビュー画像
        if (previewSprites != null && currentIndex < previewSprites.Length && previewSprites[currentIndex] != null)
        {
            previewImage.sprite = previewSprites[currentIndex];
            previewImage.gameObject.SetActive(true);
        }
        else
        {
            previewImage.gameObject.SetActive(false);
        }

        //追加
        //int groupIndex = Mathf.Clamp(currentIndex / TimerManager.instance.stagesPerGroup, 0, TimerManager.instance.totalGroups - 1);
        //if (groupIndex != previousGroupIndex)
        //{
        //    Debug.Log($"グループ {groupIndex + 1} に切り替わったのでタイムを初期化します");
        //    if (TimerManager.instance != null)
        //    {
        //        TimerManager.instance.ResetGroupTime(groupIndex);
        //    }
        //    previousGroupIndex = groupIndex;
        //}
        //追加
        // 総合クリア時間表示
        if (TimerManager.instance != null)
        {
            //int groupIndex = currentIndex / TimerManager.instance.stagesPerGroup;
            int groupIndex = Mathf.Clamp(currentIndex / TimerManager.instance.stagesPerGroup,0, TimerManager.instance.totalGroups);
            float totalTime = TimerManager.instance.GetTotalClearTime(groupIndex);
            //clearTimeText.text = totalTime < 0 ? "Total Time: --:--.--" : "Total Time: " + FormatTime(totalTime);
            clearTimeText.text = "ClearTime:" + FormatTime(totalTime);
        }
        else
        {
            clearTimeText.text = "Total Time: --:--.--";
        }
    }


    string FormatTime(float time)
    {
        int m = Mathf.FloorToInt(time / 60f);
        int s = Mathf.FloorToInt(time % 60f);
        int ms = Mathf.FloorToInt((time * 100f) % 100f);
        return $"{m}:{s:00}.{ms:00}";
    }
}
