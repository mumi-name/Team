using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 1000f;//�ړ����x
    public float maxSpeed = 10f;//�ő呬�x
    public float jumpPower = 300f;//�W�����v��
    public Rigidbody2D rb;
    public static PlayerScript instance;

    bool mode = true;//���݂̌���(ON��OFF��)
    bool beforeMode = true;//�ȑO�̌���(ON��OFF��)
    bool jumpFlag = true;//���݃W�����v����


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
        //�X�y�[�X�{�^���������ꂽ��W�����v����
        if (Input.GetAxis("Jump")>0f)
        {
            if (jumpFlag) return;
            rb.AddForce(transform.up * jumpPower);
            jumpFlag = true;
        }
    }
    void Move()
    {
        //���E�L�[�̓��͂����m
        float num = Input.GetAxisRaw("Horizontal");

        //���E�L�[��������ĂȂ��ꍇ�A�~�߂�
        if (num == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            return;
        }

        //�W�����v���ɈႤ�����������Ȃ��悤�ɂ���
        if (jumpFlag)
        {
            if (num > 0 && beforeMode == false) return;
            if (num < 0 && beforeMode == true) return;

        }

        //�v���C���[�̑��x�����ʂ𒴂���������𒆎~
        if (Mathf.Abs(rb.linearVelocity.x) > 5f) return;
        if (num > 0) mode = true;
        else if (num < 0) mode = false;

        //���͕������ς�����ꍇ�AON��OFF��؂�ւ���
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
        //���E�L�[�������������ɗ͂��|���Ĉړ�������
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //�v���C���[�̌�����ύX����
        transform.localScale = new Vector3(1 * num, 1, 1);

    }


    public bool GetMode()
    {
        //���݂̌�����Ԃ�
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
        //㩂ɐG�ꂽ�烊�Z�b�g����
        if (collision.gameObject.CompareTag("Trap"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}



//Move�̉ߋ��̏���

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
           //Debug.Log("�~�߂鏈�����Ăяo����Ă��邼!");
           Vector2 v = rb.linearVelocity;
           v.x = 0;
           rb.linearVelocity = v;
       }
       //�v���C���[�̑��x�����ʂ𒴂���������𒆎~
       if (Mathf.Abs(rb.linearVelocity.x) < 5f) rb.AddForce(transform.right * speed * Time.deltaTime * num);
       //�ړ��L�[�����͂���Ă�����A���]
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
           //Debug.Log("�~�߂鏈�����Ăяo����Ă��邼!");
           Vector2 v = rb.linearVelocity;
           v.x = 0;
           rb.linearVelocity = v;
       }
       //�v���C���[�̑��x�����ʂ𒴂���������𒆎~
       if (Mathf.Abs(rb.linearVelocity.x) < 5f) rb.AddForce(transform.right * speed * Time.deltaTime * num);
       //�ړ��L�[�����͂���Ă�����A���]
       if (num!=0)transform.localScale = new Vector3(1*num, 1, 1);*/



