using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 1000f;//�ړ����x
    public float maxSpeed = 5f;//�ő呬�x
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
        //���E�L�[�̓��͂����m
        float num = Input.GetAxisRaw("Horizontal");

        if (num > 0)mode = 1;
        if (num < 0)mode = -1;

        //���E�L�[��������ĂȂ��ꍇ�A�~�߂�
        if (num == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;

        }

        //�W�����v���ɕʕ����ɗ͂��|�����Ă���ꍇ(�I�u�W�F�N�g�̒[���g�����o�O�΍�)
        if (jumpFlag)
        {
            //velocity��0�ɂ���return����
            if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX)&&rb.linearVelocityX!=0) 
            {
                rb.linearVelocityX = 0;
                return;
            }
        }


        //�W�����v���ɈႤ�����������Ȃ��悤�ɂ���
        if (jumpFlag && mode != beforeMode) return;


        //�v���C���[�̑��x���ő呬�x�𒴂���������𒆎~
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed) return;

        //���͕������ς�����ꍇ�AON��OFF��؂�ւ���
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

      
        //���E�L�[�������������ɗ͂��|���Ĉړ�������
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //�A�j���[�V�����̃X�s�[�h�𑬓x�ɂ���ĕύX����
        animator.speed = Mathf.Abs(rb.linearVelocityX) / 2;
        //�v���C���[�̌�����ύX����
        transform.localScale = new Vector3(1 * mode, 1, 1);

    }

    //���݂̌�����Ԃ�
    public int GetMode()
    {
        return mode;
    }
    //�W�����v��Ԃ�؂�ւ���
    public void OnOffJumpFlag(bool flag)
    {
        jumpFlag = flag;
        animator.SetBool("JumpBool", flag);
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




