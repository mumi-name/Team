using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float slowSpeed = 1f;
    public float finalTime;//タイマーマネージャー用変数
    public bool douziOsi = false;//バグ対策(同時押しを禁止)
    public List<OnOffBrock> brocks;//ステージ中にあるONOFFブロック
    public List<TrapController> traps;//棘リスト
    
    public Sprite onSprite;
    public Sprite offSprite;

    private float targetScale = 1;
    private bool waveAnimation = false;//波動アニメーションの最中かどうか?
    private bool resetFlag = true;
    private bool goalFlag = false;

    void Awake()
    {
        instance = this;
    }

    public void ResetFlag(bool flag=false)
    {
        resetFlag = flag;
    }

    public void GoalFlag()
    {
        goalFlag = true;
    }

    void Start()
    {
        TimerManager.instance.StartTimer();
        Debug.Log("ステージ開始 Timer = " + TimerManager.instance.elapsedTime);
        //ステージ01に入ったらカウントを全てリセット
        if (SceneManager.GetActiveScene().name == "TitleScene")
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
        //ON();
        OnOff(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!resetFlag&&!goalFlag)
        {
            if (Input.GetButtonDown("Reset"))
            {
                if (PlayerScript.instance != null && PlayerScript.instance.isDead) return;
                resetFlag = true;
                StartCoroutine(ResetSceneWithSE());
            }
        }
        changeSlow(targetScale);
    }

    public void OnOff(bool OnOff)
    {
        if (PlayerScript.instance.GetIngoreInput()) return;
       // if (PlayerScript.instance.GetJumpFlag() && PlayerScript.instance.rb.linearVelocityY < 0) return;

        foreach (var brock in brocks)
        {
            brock.OnOff(OnOff);
        }
        foreach(var trap in traps)
        {
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

    public void ChangeOnOff(bool on)
    {
        foreach (var brock in brocks)
        {
            //brock.on = !brock.on;
            brock.on = !brock.on;
        }
        //if (on) ON();
        //else OFF();

        OnOff(on);

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

    public void SetStopFloor(bool flag)
    {
        foreach (var brock in brocks)
        {
            if (brock.move) brock.setStopFlag(flag);
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

        // 死亡カウントとIrisShot
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

