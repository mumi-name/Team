using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 1000f;//移動速度
    public float maxSpeed = 5f;//最大速度
    public float jumpPower = 300f;//ジャンプ力
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerScript instance;

    int beforeMode = 1;//以前の向き(ONかOFFか)
    int mode = 1;//現在の向き(ONかOFFか)
    bool jumpFlag = true;//現在ジャンプ中か
    bool countFlag = false;
    bool canPushJumpFlag = true;//ジャンプボタンを押せるかどうか
    float interval=0.3f;//向き切り替え直後にジャンプを押せない時間(同時押し禁止)
    float timer = 0f;
    float num;
    int pendingMode = 0;//記録用
    //private OnOffBrock onoffBrock;
    //FIX:バグ

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        Jump();
        Move();
        if (countFlag)
        {
            timer += Time.deltaTime;
            if (timer > interval)
            {
                timer = 0;
                canPushJumpFlag = true;
                countFlag = false;
            }
        }

    }

    void Jump()
    {
        //スペースボタンが押されたらジャンプする
        if (Input.GetButtonDown("Jump")&&canPushJumpFlag)
        {
            if (jumpFlag) return;
            Debug.Log("ジャンプ"+Time.time);
            rb.linearVelocityY = 0; 
            rb.AddForce(transform.up * jumpPower);
            OnOffJumpFlag(true);
        }
    }
    void Move()
    {
        //左右キーの入力を検知
        num = Input.GetAxisRaw("Horizontal");


        if (num > 0)mode = 1;
        if (num < 0)mode = -1;

        //左右キーが押されてない場合、止める
        if (num == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;

        }

        //ジャンプ中に別方向に力が掛かっている場合(オブジェクトの端を使ったバグ対策)
        if (jumpFlag)
        {
            //velocityを0にしてreturnする
            if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX)&&rb.linearVelocityX!=0) 
            {
                rb.linearVelocityX = 0;
                return;
            }
        }


        //ジャンプ中に違う方向を向けないようにする
        if (jumpFlag && mode != beforeMode)
        {
            pendingMode = mode;
            return;
        }

        //プレイヤーの速度が最大速度を超えたら加速を中止
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed) return;

        //入力方向が変わった場合、ONとOFFを切り替える
        if (beforeMode != mode)
        {
            canPushJumpFlag = false;
            countFlag = true;
            if (num > 0)
            {
                animator.SetBool("OnOffBool", true);
                GameManager.instance.ON();
            }
            else if (num < 0)
            {
                animator.SetBool("OnOffBool", false);
                GameManager.instance.OFF();
            }
            beforeMode = mode;

        }

        Debug.Log("移動"+Time.time);
        //左右キーを押した方向に力を掛けて移動させる
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //アニメーションのスピードを速度によって変更する
        float animeSpeed = 1;
        if (Mathf.Abs(rb.linearVelocityX )>= maxSpeed - 1.0f)
        {
            animeSpeed = 0.5f;
            
        }

        animator.speed = Mathf.Abs(rb.linearVelocityX*animeSpeed) ;
        //プレイヤーの向きを変更する
        transform.localScale = new Vector3(1 * mode, 1, 1);

    }

    //現在の向きを返す
    public int GetMode()
    {
        return mode;
    }
    public bool GetJumpFlag()
    {
        return jumpFlag;
    }
    //ジャンプ状態を切り替える
    public void OnOffJumpFlag(bool flag)
    {
        jumpFlag = flag;
        animator.SetBool("JumpBool", flag);
        
    }
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //罠に触れたらリセットする
        if (collision.gameObject.CompareTag("Trap"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //地面に着地した時に向きを更新する。
        /*if (collision.gameObject.CompareTag("Floor"))
        {
            if (pendingMode != 0 && pendingMode != beforeMode)
            {
                canPushJumpFlag = false;
                countFlag = true;

                if (pendingMode > 0)
                {
                    animator.SetBool("OnOffBool", true);
                    GameManager.instance.ON();
                }
                else
                {
                    animator.SetBool("OnOffBool", false);
                    GameManager.instance.OFF();
                }

                beforeMode = pendingMode;
            }

            pendingMode = 0;//使ったらリセット
            OnOffJumpFlag(false);//着地後ジャンプ状態解除
        }*/
    }

}




