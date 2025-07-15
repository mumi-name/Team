using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 1000f;//移動速度
    public float maxSpeed = 10f;//最大速度
    public float jumpPower = 300f;//ジャンプ力
    public Rigidbody2D rb;
    public static PlayerScript instance;

    bool mode = true;//現在の向き(ONかOFFか)
    bool beforeMode = true;//以前の向き(ONかOFFか)
    bool jumpFlag = true;//現在ジャンプ中か


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
        if (Input.GetAxis("Jump")>0f)
        {
            if (jumpFlag) return;
            rb.AddForce(transform.up * jumpPower);
            jumpFlag = true;
        }
    }
    void Move()
    {
        //左右キーの入力を検知
        float num = Input.GetAxisRaw("Horizontal");

        //左右キーが押されてない場合、止める
        if (num == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            return;
        }

        //ジャンプ中に違う方向を向かないようにする
        if (jumpFlag)
        {
            if (num > 0 && beforeMode == false) return;
            if (num < 0 && beforeMode == true) return;

        }

        //プレイヤーの速度が一定量を超えたら加速を中止
        if (Mathf.Abs(rb.linearVelocity.x) > 5f) return;
        if (num > 0) mode = true;
        else if (num < 0) mode = false;

        //入力方向が変わった場合、ONとOFFを切り替える
        if (beforeMode != mode)
        {
            if (num > 0)
            {
                GameManager.instance.ON();
                beforeMode = true;
            }
            else if (num < 0)
            {
                GameManager.instance.OFF();
                beforeMode = false;
            }
        }
        //左右キーを押した方向に力を掛けて移動させる
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //プレイヤーの向きを変更する
        transform.localScale = new Vector3(1 * num, 1, 1);

    }


    public bool GetMode()
    {
        //現在の向きを返す
        return mode;
    }
    public void OnJumpFlag()
    {
        jumpFlag = true;
    }
    public void OffJumpFlag()
    {
        jumpFlag = false;
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



//Moveの過去の処理

/*int num = 0;
       if (Input.GetKey(KeyCode.RightArrow))
       {
           if (beforeMode==false) return;
           num = 1;
           mode = true;
           if (beforeMode != mode) GameManager.instance.ON();
           beforeMode = true;
       }
       if (Input.GetKey(KeyCode.LeftArrow))
       {
           if (jumpFlag && beforeMode==true) return;
           num = -1;
           mode = false;
           if (beforeMode != mode) GameManager.instance.OFF();
           beforeMode = false;
       }
       if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
       {
           //Debug.Log("止める処理が呼び出されているぞ!");
           Vector2 v = rb.linearVelocity;
           v.x = 0;
           rb.linearVelocity = v;
       }
       //プレイヤーの速度が一定量を超えたら加速を中止
       if (Mathf.Abs(rb.linearVelocity.x) < 5f) rb.AddForce(transform.right * speed * Time.deltaTime * num);
       //移動キーが入力されていたら、反転
       if (num!=0)transform.localScale = new Vector3(1*num, 1, 1);*/



/*int num = 0;
       if (Input.GetKey(KeyCode.RightArrow))
       {
           if (beforeMode==false) return;
           num = 1;
           mode = true;
           if (beforeMode != mode) GameManager.instance.ON();
           beforeMode = true;
       }
       if (Input.GetKey(KeyCode.LeftArrow))
       {
           if (jumpFlag && beforeMode==true) return;
           num = -1;
           mode = false;
           if (beforeMode != mode) GameManager.instance.OFF();
           beforeMode = false;
       }
       if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
       {
           //Debug.Log("止める処理が呼び出されているぞ!");
           Vector2 v = rb.linearVelocity;
           v.x = 0;
           rb.linearVelocity = v;
       }
       //プレイヤーの速度が一定量を超えたら加速を中止
       if (Mathf.Abs(rb.linearVelocity.x) < 5f) rb.AddForce(transform.right * speed * Time.deltaTime * num);
       //移動キーが入力されていたら、反転
       if (num!=0)transform.localScale = new Vector3(1*num, 1, 1);*/



