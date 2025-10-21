using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public float slowSpeed = 1f;
    public List<OnOffBrock> brocks;//ステージ中にあるONOFFブロック
    public List<TrapController> traps;//棘リスト
    public static GameManager instance;

    public Sprite onSprite;
    public Sprite offSprite;

    float targetScale = 1;
    bool waveAnimation = false;//波動アニメーションの最中かどうか?
    //bool slow = false;
    public float finalTime;//タイマーマネージャー用変数

    void Awake()
    {
        instance = this;

        // traps リストが空の場合、自動でシーン内の TrapController を取得
        //if (traps == null || traps.Count == 0)
        //{
        //    traps = new List<TrapController>(FindObjectsOfType<TrapController>());
        //}
    }
    void Start()
    {
        //ステージ01に入ったらカウントを全てリセット
        if (SceneManager.GetActiveScene().name == "01")
        {
            if (TimerManager.instance != null)
            {
                TimerManager.instance.AllCountReset();
            }
        }
        if(IrisShot.instance != null)
        {
            IrisShot.instance.IrisIN();
        }
        AudioManager.instance.PlayBGM("BGM3");
        //DontDestroyOnLoad(gameObject);
        ON();
    }

    // Update is called once per frame
    void Update()
    {

        /*リセットボタンが押されたらゲームシーンをリセット
        if (Input.GetButtonDown("Reset"))
        {
            AudioManager.instance.PlaySE("リセット");
            //スローモーションを即座に終了
            //slow = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            TimerManager.instance.AddDeath();
            IrisShot.instance.ResetIris();
            if(IrisShot.instance != null)
            {
                Destroy(IrisShot.instance.gameObject);//古いものを削除
            }
            //シーンを遷移させる
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        changeSlow(targetScale);
        */
        if (Input.GetButtonDown("Reset"))
        {
            StartCoroutine(ResetSceneWithSE());
        }
        changeSlow(targetScale);
    }

    //追加分---------------------------------------------------------------------

    public void ON()
    {
        //if (waveAnimation && Mathf.Abs(PlayerScript.instance.rb.linearVelocity.y) > 0.5) return;
        foreach (var brock in brocks)
        {
            brock.ON();
            
        }
        foreach(var trap in traps)
        {
            //trap.isActive = true;
            trap.ToggleTrap();
            
        }
        AudioManager.instance.PlaySE2("ONOFF");
        ScreenMove.instance.Toggle();
    }
    public void OFF()
    {
        //if (waveAnimation && Mathf.Abs(PlayerScript.instance.rb.linearVelocity.y) > 0.5) return;
        foreach (var brock in brocks)
        {
            brock.OFF();
        }
        foreach(var trap in traps)
        {
            //trap.isActive = false;
            trap.ToggleTrap();
            
        }
        AudioManager.instance.PlaySE2("ONOFF");
        ScreenMove.instance.Toggle();
    }

    public void OFFChanged()
    {
        foreach (var brock in brocks)
        {

            brock.SetOffChanged();
        }
        AudioManager.instance.PlaySE2("ONOFF");
    }

    //スローモーションの切り替え
    public void OnOffSlow(bool on)
    {
        if (on)
        {
            //slow = true;
            targetScale = slowSpeed;
            waveAnimation = true;
        }
        else
        {
            targetScale = 1;
            waveAnimation = false;
            
        }
    }

    void changeSlow(float targetScale)
    {
        //if (!slow) return;
        //スローモーションにグラデーションを持たせる
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, 10  * Time.unscaledDeltaTime);
        //物理演算がカクつかないように周期の倍率を合わせる
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }


    //------------------------------------------------------------------------------

    public void ChangeOnOff(bool on)
    {
        foreach (var brock in brocks)
        {
            //brock.on = !brock.on;
            brock.on = !brock.on;
        }
        if (on) ON();
        else OFF();
    }

    //enabled判定からtrigger判定へ切り替える(これが無いと、OnOff切り替えが上手くいかない)
    public void ChangeEnabledToTrigger()
    {
        foreach(var brock in brocks)
        {
            brock.ChangeEnabledToTrigger();
        }
    }

    //trigger判定からenabled判定へ切り替える(これが無いと、OnOff切り替えが上手くいかない)
    public void ChangeTriggerToEnabled()
    {
        foreach (var brock in brocks)
        {
            brock.ChangeTriggerToEnabled();
        }
    }

    public bool GetWaveAnimation()
    {
        return waveAnimation;
    }

    //リセット処理のコルーチン
    private IEnumerator ResetSceneWithSE()
    {
        // リセット音を鳴らす
        AudioManager.instance.PlaySE2("リセット");

        // スローモーションを即座に終了
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // 死亡カウントとIrisShotのリセット
        TimerManager.instance?.AddDeath();
        IrisShot.instance?.ResetIris();

        if (IrisShot.instance != null)
        {
            Destroy(IrisShot.instance.gameObject); // 古いものを削除
        }

        // SEが少しだけ鳴る時間を待つ（例: 0.2秒）
        yield return new WaitForSeconds(0.2f);

        // シーンをリロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ToggleTraps()
    {
        if (traps != null)
        {
            foreach (var trap in traps)
            {
                if (!trap.locked)
                {
                    trap.isActive = !trap.isActive;  // ON/OFF 反転
                    trap.ToggleTrap();              // 実際の動作・表示を更新
                }
            }
        }
    }
}



//<???????????????????????R?????g??>

/*public BoxCollider2D box;
    public SpriteRenderer spr;
    bool jumpFlag = false;*/

//<Update???????????????R?????g??>
/*if (Input.GetKey(KeyCode.RightArrow))
{
    if (jumpFlag) return;
    box.enabled = false;
    spr.color = new Color(255, 255, 255, 0);
}
if (Input.GetKey(KeyCode.LeftArrow))
{
    if (jumpFlag) return;
    box.enabled = true;
    spr.color = new Color(255, 255, 255, 255);
}
*/