using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Cinemachine.CinemachineTargetGroup;
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
    //private bool turn = false;
    public bool turn = false;

    private bool stop = false;
    private bool switching = false;//OnOffの切り替え中かどうか？
    private bool invalid = false;//ブロックの判定を有効化するのを禁止する


    float waitTimer = 0f;
    bool waiting = false;

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

    public void OnOff(bool OnOff,bool animation = false)
    {

        //透明度変化アニメーション中に呼び出されたら、アニメを停止
        fadeFlag = false;

        //親子関係を切る
        PlayerScript.instance.transform.SetParent(null);

        //プレイヤーの向き(On・Off)とブロックのOn・Offが一緒の時
        if (on == OnOff)
        {
            //判定を有効化
            box.enabled = true;
            if (GameManager.instance.GetWaveAnimation() == true && !changed)
            {
                box.isTrigger = false;
            }

            //画像を差し替える
            spr.sprite = onSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", true);
                animator.speed = 2;
            }

            //透明度を変更する
            Color color = spr.material.color;
            color.a = 1f;
            if (animation)
            {
                //波動アニメーションから呼び出されたら、a値を0.4にしとく（アニメーション時の演出用）
                color.a = 0.1f;
                if (animationSpeed < 0) animationSpeed *= -1;

            }
            spr.material.color = color;

        }
        //プレイヤーの向き(On・Off)とブロックのOn・Offが別の時
        else
        {
            //判定を無効化
            box.enabled = false;
            //waveAnimation中の場合は当たり判定の取り方を一時的にTriggerで取る。(enabledだとOnOff反転しないため)
            if ((GameManager.instance.GetWaveAnimation() == true || animation) && !changed)
            {
                box.enabled = true;
                box.isTrigger = true;
            }

            //画像を差し替える
            spr.sprite = offSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", false);
                animator.speed = 1;
            }

            //透明度を変更する
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
        OnOffMove(OnOff);

    }

    public void OnfadeAnimation()
    {
        fadeFlag = true;
    }

    public void OnOffMove(bool mode)
    {

        //プレイヤーの向き(On・Off)とブロックのOn・Offが一緒の時
        if (mode == on)
        {
            moveFlag = true;
        }
        //プレイヤーの向き(On・Off)とブロックのOn・Offが別の時
        else
        {
            moveFlag = false;
            transform.position = orizinalpos;
        }
        
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
        //if (!moveFlag) return;
        //if (move)
        //{
        //    if (stop) return;//停止命令が出ていたら停止

        //    if (!turn && Mathf.Approximately(transform.position.x, movestop.x) && Mathf.Approximately(transform.position.y, movestop.y))
        //    {
        //        Invoke("OnTurn", 0.7f);
        //    }
        //    if (turn && Mathf.Approximately(transform.position.x, orizinalpos.x) && Mathf.Approximately(transform.position.y, orizinalpos.y))
        //    {
        //        Invoke("OffTurn", 0.7f);
        //    }

        //    if (!turn)
        //    {
        //        transform.position = Vector3.MoveTowards
        //        (transform.position, movestop, Mathf.Abs(moveSpeed) * Time.deltaTime);
        //    }
        //    else
        //    {
        //        transform.position = Vector3.MoveTowards(transform.position, orizinalpos, Mathf.Abs(moveSpeed) * Time.deltaTime);
        //    }
        //}

        if (!moveFlag || stop) return;
        if (!move) return;

        if (!waiting)
        {
            Vector3 target = turn ? orizinalpos : movestop;
            transform.position = Vector3.MoveTowards(transform.position, target, Mathf.Abs(moveSpeed) * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                waiting = true;
                waitTimer = 0f;
            }
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= 0.7f)
            {
                waiting = false;
                if (loop) turn = !turn;
            }
        }


    }


    void OnTurn()
    {
        if (!loop) return;
        turn = true;

        //エレベーターと折り返し地点の距離を比較
        //var distance1 = orizinalpos - transform.position;
        //var distance2 = movestop - transform.position;

        float distance = Vector3.Distance(transform.position, orizinalpos);
        float distance2 = Vector3.Distance(transform.position, movestop);

        if(distance < distance2)
        {
            turn = false;
        }


        //開始位置に近い場合
        //if (Mathf.Abs(distance1.x) < Mathf.Abs(distance2.x) || Mathf.Abs(distance1.y) < Mathf.Abs(distance2.y))
        //{
        //    turn = false;
        //}

    }

    void OffTurn()
    {

        if (!loop) return;
        turn = false;

        //エレベーターと折り返し地点の距離を比較
        //var distance1 = orizinalpos - transform.position;
        //var distance2 = movestop - transform.position;

        float distance = Vector3.Distance(transform.position, orizinalpos);
        float distance2 = Vector3.Distance(transform.position, movestop);

        if (distance2 < distance)
        {
            turn = true;
        }

        //折り返し地点に近い場合
        //if (Mathf.Abs(distance2.x) < Mathf.Abs(distance1.x) || Mathf.Abs(distance2.y) < Mathf.Abs(distance1.y))
        //{
        //    turn = true;
        //}


        //if (Mathf.Approximately(transform.position.x, movestop.x) && Mathf.Approximately(transform.position.y, movestop.y)) turn = true;
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