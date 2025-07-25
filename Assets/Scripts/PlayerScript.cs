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


    //FIX:バグ

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();

    }

    void Jump()
    {
        //スペースボタンが押されたらジャンプする
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpFlag) return;
            rb.linearVelocityY = 0; 
            rb.AddForce(transform.up * jumpPower);
            OnOffJumpFlag(true);
        }
    }
    void Move()
    {
        //左右キーの入力を検知
        float num = Input.GetAxisRaw("Horizontal");

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
        if (jumpFlag && mode != beforeMode) return;


        //プレイヤーの速度が最大速度を超えたら加速を中止
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed) return;

        //入力方向が変わった場合、ONとOFFを切り替える
        if (beforeMode != mode)
        {
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

      
        //左右キーを押した方向に力を掛けて移動させる
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //アニメーションのスピードを速度によって変更する
        animator.speed = Mathf.Abs(rb.linearVelocityX) / 2;
        //プレイヤーの向きを変更する
        transform.localScale = new Vector3(1 * mode, 1, 1);

    }

    //現在の向きを返す
    public int GetMode()
    {
        return mode;
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
    }

}




