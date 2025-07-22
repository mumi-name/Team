using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 1000f;//�ړ����x
    public float maxSpeed = 10f;//�ő呬�x
    public float jumpPower = 300f;//�W�����v��
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerScript instance;

    int beforeMode = 1;//�ȑO�̌���(ON��OFF��)
    int mode = 1;//���݂̌���(ON��OFF��)
    bool jumpFlag = true;//���݃W�����v����


    //FIX:�o�O

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
        if (Input.GetButtonDown("Jump"))//Input.GetKeyDown(KeyCode.Space)   //Input.GetAxis("Jump")>0f
        {
            if (jumpFlag) return;
            rb.linearVelocityY = 0;
            rb.AddForce(transform.up * jumpPower);
            animator.SetBool("JumpBool", true);
            jumpFlag = true;
        }
    }
    void Move()
    {
        //���E�L�[�̓��͂����m
        float num = Input.GetAxisRaw("Horizontal");

        //�W�����v���ɕʕ����ɗ͂��|�����Ă���ꍇ(�I�u�W�F�N�g�̒[���g�����o�O�΍�)
        if (jumpFlag)
        {
            //velocity��0�ɂ���return����
            if (beforeMode > 0 && rb.linearVelocityX < 0)
            {
                rb.linearVelocityX = 0;
                return;
            }
            if (beforeMode < 0 && rb.linearVelocityX > 0)
            {
                rb.linearVelocityX = 0;
                return;
            }
        }
        //���E�L�[��������ĂȂ��ꍇ�A�~�߂�
        if (num == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;
        }
        //�W�����v���ɈႤ�����������Ȃ��悤�ɂ���
        if (jumpFlag && num != beforeMode) return;

        //���͕����ɂ���ăA�j���[�V�����̃o�[�W������؂�ւ���
        if (num > 0)
        {
            animator.SetBool("OnOffBool", true);
            mode = 1;

        }
        else if (num < 0)
        {
            mode = -1;
            animator.SetBool("OnOffBool", false);
        }
        
        //�v���C���[�̑��x�����ʂ𒴂���������𒆎~
        if (Mathf.Abs(rb.linearVelocity.x) > 5f) return;

        //���͕������ς�����ꍇ�AON��OFF��؂�ւ���
        if (beforeMode != mode)
        {
            if (num > 0)
            {
                GameManager.instance.ON();
                beforeMode = 1;

            }
            else if (num < 0)
            {
                GameManager.instance.OFF();
                beforeMode = -1;
            }
        }
        //���E�L�[�������������ɗ͂��|���Ĉړ�������
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //�A�j���[�V�����̃X�s�[�h�𑬓x�ɂ���ĕύX����
        animator.speed = Mathf.Abs(rb.linearVelocityX)/2;
        //�v���C���[�̌�����ύX����
        transform.localScale = new Vector3(1 * mode, 1, 1);

    }


    public int GetMode()
    {
        //���݂̌�����Ԃ�
        return mode;
    }
    public void OnJumpFlag()
    {
        jumpFlag = true;
        animator.SetBool("JumpBool", true);
    }
    public void OffJumpFlag()
    {
        jumpFlag = false;
        animator.SetBool("JumpBool", false);
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
       if (num!=0)transform.localScale = new Vector3(1*num, 1, 1);
}
*/
