using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;


public class OnOffBrock : MonoBehaviour
{
    public float moveSpeed = 0.4f;//移動速度
    public bool on = false;//このブロックはonで判定がつくのかどうか
    public bool move = false;//このブロックは動くのか
    public bool loop = false;//往復する
    
    public Vector3 orizinalpos = Vector3.zero;
    public Vector3 movestop = Vector3.zero;
    //コンポーネント
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public SpriteRenderer spr;
    public Animator animator;
    //スプライト
    public Sprite onSprite;
    public Sprite offSprite;
    //内部参照
    private float animationSpeed = 0.03f;
    private bool moveFlag = false;
    private bool fadeFlag = false;
    private bool changed = false;
    private bool turn = false;
    private bool stop = false;
    private bool invalid = false;//ブロックの判定を有効化するのを禁止する

    void Start()
    {
        if(orizinalpos==Vector3.zero)orizinalpos = transform.position;
        if (spr == null) spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FadeAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }


    public void ON(bool animation = false)
    {
        //透明度変化アニメーション中に呼び出されたら、アニメを停止
        fadeFlag = false;

        PlayerScript.instance.transform.SetParent(null);

        if (on)
        {
            //当たり判定を有効化
            box.enabled = true;
            if (GameManager.instance.GetWaveAnimation() == true && !changed)
            {
                box.isTrigger = false;
            }

            //画像・アニメーションを変更
            spr.sprite = onSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", true);
                animator.speed = 2;
            }
            Color color = spr.material.color;
            color.a = 1f;
            //波動アニメーションから呼び出されたら、a値を0.4にしとく（アニメーション時の演出用）
            if (animation)
            {
                color.a = 0.1f;
                if (animationSpeed < 0) animationSpeed *= -1;
                
            }
            spr.material.color = color;

        }
        else
        {
            //当たり判定を無効化
            box.enabled = false;
            //waveAnimation中の場合は当たり判定の取り方を一時的にTriggerで取る。(enabledだとOnOff反転しないため)
            if ((GameManager.instance.GetWaveAnimation()==true||animation)&&!changed)
            {
                box.enabled = true;
                box.isTrigger = true;
            }

            //画像・アニメーションを変更
            spr.sprite = offSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", false);
                animator.speed = 1;
            }
            Color color = spr.material.color;
            color.a = 1f;
            if (animation)
            {
                spr.sprite = onSprite;
                color.a = 1f;
                if (animationSpeed > 0) animationSpeed *= -1;
            }
            spr.material.color = color;

        }
        OnMove();
    }

    public void OFF(bool animation = false)
    {
        //透明度変化アニメーション中に呼び出されたら、アニメを停止
        fadeFlag = false;

        PlayerScript.instance.transform.SetParent(null);

        if (on)
        {
            //当たり判定を無効化
            box.enabled = false;
            //waveAnimation中の場合は当たり判定の取り方を一時的にTriggerで取る。(enabledだとOnOff反転しないため)
            if ((GameManager.instance.GetWaveAnimation()||animation)&&!changed)
            {
                Debug.Log("Trueになっているだと?");
                box.enabled = true;
                box.isTrigger = true;
               
            }

            //画像・アニメーションを変更
            spr.sprite = offSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", false);
                animator.speed = 1;
            }
            Color color = spr.material.color;
            color.a = 1f;
            if (animation)
            {
                spr.sprite = onSprite;
                color.a = 1f;
                if (animationSpeed > 0) animationSpeed *= -1;
                
            }
            spr.material.color = color;

        }
        else
        {
            //当たり判定を有効化
            box.enabled = true;
            if (GameManager.instance.GetWaveAnimation() && !changed)
            {
                box.isTrigger = false;
            }

            //画像・アニメーションを変更
            spr.sprite = onSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", true);
                animator.speed = 2;
            }
            Color color = spr.material.color;
            color.a = 1f;
            //波動アニメーションから呼び出されたら、a値を0.4にしとく（アニメーション時の演出用）
            if (animation)
            {
                color.a = 0.1f; 
                if (animationSpeed < 0) animationSpeed *= -1;
                
            }
            spr.material.color = color;

        }
        OffMove();

    }

    public void OnfadeAnimation()
    {
        fadeFlag = true;
    }

    public void OnMove()
    {
        moveFlag = true;
        //モードがOFFで動く場合
        if (!on)
        {
            moveFlag = false;
            transform.position = orizinalpos;//元の場所に戻す
        }
    }

    public void OffMove()
    {
        moveFlag = false;
        transform.position = orizinalpos;//元の場所に戻す
        if (!on) moveFlag = true;
    }

    public void SetOnChanged()
    {
        //一度の衝撃波に2回当たっても変わらないようにフラグを立てておく
        changed = true;
        ChangeTriggerToEnabled();
    }

    public void SetOffChanged()
    {
        //フラグを降ろす
        changed = false;
    }


    //enabled判定からtrigger判定へ切り替える(これが無いと、OnOff切り替えが上手くいかない)
    public void ChangeEnabledToTrigger()
    {
        if (box.enabled == true)
        {
            box.isTrigger = false;
        }
        else if(box.enabled == false)
        {
            box.enabled = true;
            box.isTrigger = true;
        }
    }

    //trigger判定からenabled判定へ切り替える(これが無いと、OnOff切り替え時めり込みが発生する)
    public void ChangeTriggerToEnabled()
    {
        if (box.isTrigger)
        {
            box.enabled = false;
            box.isTrigger = false;
        }
        else if(box.isTrigger == false)
        {
            box.enabled = true;
        }
    }
    public void setStopFlag(bool flag)
    {
        stop = flag;
    }
    public bool GetChanged()
    {
        return changed;
    }


    void Move()
    {
        //ONで動くブロックがOFFの時にも動いてしまわないようにmoveFlagを確認する
        if (!moveFlag) return;
        if (move)
        {
            if (stop) return;//停止命令が出ていたら停止
            if (loop && Mathf.Approximately(transform.position.x, movestop.x) && Mathf.Approximately(transform.position.y, movestop.y)) Invoke("OnTurn", 0.7f);
            if (Mathf.Approximately(transform.position.x, orizinalpos.x) && Mathf.Approximately(transform.position.y, orizinalpos.y)) Invoke("OffTurn", 0.7f);

            if (!turn)
            {
                transform.position = Vector3.MoveTowards
                (transform.position, movestop, Mathf.Abs(moveSpeed) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,orizinalpos, Mathf.Abs(moveSpeed) * Time.deltaTime);
            }
        }
        
    }

    void OnTurn()
    {
        turn = true;
    }

    void OffTurn()
    {
        turn = false;
    }

    void FadeAnimation()
    {
        if (fadeFlag == false) return;
        Color color = spr.material.color;
        color.a += animationSpeed;
        spr.material.color = color;

        //透明度が１以上になったらアニメーションを終了させる
        if (color.a >= 1f && animationSpeed > 0)
        {
            fadeFlag = false;
            PlayerScript.instance.ignoreMove(false);
        }
        //OFF状態で透明度が0.4を切ったら画像を差し替えてアニメーション終了
        if (color.a <= 0.1f && animationSpeed < 0)
        {
            fadeFlag = false;
            spr.sprite = offSprite;
            color.a = 0.4f;
            spr.material.color = color;
            PlayerScript.instance.ignoreMove(false);
        }

    }


}