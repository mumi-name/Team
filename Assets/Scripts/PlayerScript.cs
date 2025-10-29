using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Player")]
    public float speed = 1000f;//移動速度
    public float maxSpeed = 5f;//最大速度
    public float jumpPower = 300f;//ジャンプ力
    public float slowMaxSpeed = 0.1f;
    public float slowGravity = 0.1f;
    //public float footstepCooldown = 0.25f;//足音の間隔
    //private float lastFootstepTime = 0f;   //最後に鳴らした時間
    public bool jumpMode = true;//ジャンプのアリ・ナシ
    public bool backJump = true;
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerScript instance;


    public bool bagTaisyo5 = false;

    int beforeMode = 1;//以前の向き（ONかOFFか）
    int mode = 1;//現在の向き（ONかOFFか）
    int pendingMode = 0;//記録用
    float num;
    bool jumpFlag = true;//現在ジャンプ中か
    bool canPushJumpFlag = true;//ジャンプボタンを押せるかどうか
    public bool isDead = false;
    //-------------------------------------------------------------------------------------------
    bool ignoreInput = false;//入力を全て無視する
    //-------------------------------------------------------------------------------------------

    
    
    //private OnOffBrock onoffBrock;
    //FIX:バグ
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("VeloX：" + rb.linearVelocityX.ToString());
        Debug.Log("VeloX：" + rb.linearVelocityX.ToString() + "  num：" + num.ToString() + "  mode:" + mode.ToString());
        //入力を無視し、処理を行わない
        if (ignoreInput) return;
        if(isDead ) return;
        

        if (GameManager.instance.GetWaveAnimation() && Mathf.Abs(rb.linearVelocity.y)!=0) jumpFlag = true;
        if (jumpMode)Jump();
        Move();

    }

    private void FixedUpdate()
    {
        //入力を無視し、処理を行わない
        if (ignoreInput) return;
        if (isDead) return;

        //プレイヤーの速度が最大速度を超えたら加速を中止
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed) return;
        //左右キーを押した方向に力を掛けて移動させる
        rb.linearVelocityX = speed * mode * Mathf.Abs(num) * Time.deltaTime;

        //プレイヤーの向きと逆方向に力が掛かっている場合はreturn
        if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX) && rb.linearVelocityX != 0)
        {
            if (!backJump)
            {
                rb.linearVelocityX = 0;
                return;
            }
            //rb.linearVelocityX *= -1.0f;

        }


        //ジャンプ中に違う方向を向けないようにする
        //if (jumpFlag && mode != beforeMode)
        //{
        //    pendingMode = mode;
        //    return;
        //}

    }

    void Jump()
    {
        //スペースボタンが押されたらジャンプする
        if (Input.GetButtonDown("Jump") && canPushJumpFlag)
        {   
            if (jumpFlag) return;
            AudioManager.instance.PlaySE("ジャンプ");
            Debug.Log("ジャンプ" + Time.time);
            rb.linearVelocityY = 0;
            rb.AddForce(transform.up * jumpPower);
            OnOffJumpFlag(true);
        }
    }
    void Move()
    {
        //左右キーの入力を検知
        num = Input.GetAxisRaw("Horizontal");


        if (num > 0) mode = 1;
        if (num < 0) mode = -1;

        ////ジャンプ中に違う方向に入力されたら垂直落下させる
        //if (jumpFlag && mode != beforeMode)
        //{
        //    num = 0;
        //    rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        //    return;
        //}

        //左右キーが押されてない場合、止める
        if (Mathf.Abs(num) < 0.2f)//0.2fに戻しとく 10/24
        {
            num = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;
        }

        //ジャンプ中に別方向に力が掛かっている場合（オブジェクトの端を使ったバグ対策）
        //velocityを0にしてreturnする
        if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX) && rb.linearVelocityX != 0)
        {
            if (!backJump)
            {
                rb.linearVelocityX = 0;
                return;
            }

           rb.linearVelocityX *= -1.0f;
        }

        //ジャンプ中に違う方向を向けないようにする
        if (jumpFlag && mode != beforeMode)
        {
            pendingMode = mode;
            //垂直落下させたい場合はコメントアウト
            //num = 0;

            //入力方向と反対方向に力が掛かっていたら反転
            if (Mathf.Sign(mode) == Mathf.Sign(num))
            {
                if (Mathf.Sign(rb.linearVelocityX) != Mathf.Sign(num)) rb.linearVelocityX *= -1.0f;
            }
            return;
        }


        //入力方向が変わった場合、ONとOFFを切り替える
        if (beforeMode != mode)
        {
            if (mode > 0)
            {
                animator.SetBool("OnOffBool", true);
                GameManager.instance.ON();
            }
            else if (mode < 0)
            {
                animator.SetBool("OnOffBool", false);
                GameManager.instance.OFF();
            }
            beforeMode = mode;
        }


        if(!jumpFlag)
        {
            AudioManager.instance.PlaySE("足音");
        }

        //アニメーションのスピードを速度によって変更する
        float animeSpeed = 1;
        if (Mathf.Abs(rb.linearVelocityX) >= maxSpeed - 1.0f)
        {
            animeSpeed = 0.3f;
        }

        animator.speed = Mathf.Abs(rb.linearVelocityX * animeSpeed);
        //animator.speed = 1;
        //プレイヤーの向きを変更する
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)* mode*-1.0f, transform.localScale.y, 1);

    }

    
    public void ignoreMove(bool mode)
    {
        if (mode == false && AnimationScript.instance.GetTutorialMode())
        {
            Debug.Log("はじく処理が出来ているぜーーー2");
            return;
        }
        ignoreInput = mode;
        Debug.Log("はじく処理って動いているのか・・な？");
        //if(mode)rb.linearVelocityX = 0;
    }

    //動けないように空中に固定
    public void cannotMoveMode(bool gravity = false)
    {
        ignoreInput = true;
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        rb.gravityScale = 0;
        if (gravity) rb.gravityScale = 1;
    }

    public void canMoveMode()
    {
        ignoreInput = false;
        rb.gravityScale = 1.7f;
    }

    //現在の向きを返す
    public int GetMode()
    {
        //return mode;
        int muki = 1;
        if (transform.localScale.x*-1.0f < 0) muki = -1;
        return muki;
    }

    public float GetNum()
    {
        return num;
    }
    public bool GetJumpFlag()
    {
        return jumpFlag;
    }
    //--------------------------------------------------------------------------



    //ジャンプ状態を切り替える
    public void OnOffJumpFlag(bool flag)
    {
        //ジャンプ中にフラグをfalseにしようとした場合リターンする
        //if (flag == false && Mathf.Abs(rb.linearVelocityY) > 0) return;
        jumpFlag = flag;
        animator.SetBool("JumpBool", flag);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //罠に触れたらリセットする
        if (collision.gameObject.CompareTag("Trap") && !isDead)
        {
            animator.speed = 1;
            isDead = true;
            cannotMoveMode(true);
            animator.SetTrigger("DeathTrigger");
            Debug.Log("死亡後の処理は呼び出されている?");
            AudioManager.instance.PlaySE("割れる");
            TimerManager.instance.AddDeath();
            TrapController trap = collision.gameObject.GetComponent<TrapController>();
            
            //シーン遷移は死亡アニメーションが終了後に再生するため処理を別スクリプトに移行(10/17)
            //アニメーションイベントでSceneTransitionのTransition()を呼ぶ
            
        }

        //if()

        
    }

}