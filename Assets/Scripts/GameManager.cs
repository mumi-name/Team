using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public float slowSpeed = 0.2f;
    public List<OnOffBrock> brocks;//ステージ中にあるONOFFブロック
    public static GameManager instance;

    public Sprite onSprite;
    public Sprite offSprite;

    float targetScale = 1;
    bool waveAnimation = false;//波動アニメーションの最中かどうか?
    //bool slow = false;
    public float finalTime;//タイマーマネージャー用変数

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        ON();
    }

    // Update is called once per frame
    void Update()
    {
        //リセットボタンが押されたらゲームシーンをリセット
        if (Input.GetButtonDown("Reset"))
        {
            //スローモーションを即座に終了
            //slow = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            //シーンを遷移させる
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        changeSlow(targetScale);

    }

    //追加分---------------------------------------------------------------------

    public void ON()
    {
        foreach (var brock in brocks)
        {

            brock.ON();
        }

    }
    public void OFF()
    {

        foreach (var brock in brocks)
        {

            brock.OFF();
        }
    }

    public void OFFChanged()
    {
        foreach (var brock in brocks)
        {

            brock.SetOffChanged();
        }
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
            ChangeTriggerToEnabled();
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