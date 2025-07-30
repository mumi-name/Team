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
    bool countFlag = false;
    bool canPushJumpFlag = true;//�W�����v�{�^���������邩�ǂ���
    float interval=0.3f;//�����؂�ւ�����ɃW�����v�������Ȃ�����(���������֎~)
    float timer = 0f;
    float num;
    int pendingMode = 0;//�L�^�p
    //private OnOffBrock onoffBrock;
    //FIX:�o�O

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
        //�X�y�[�X�{�^���������ꂽ��W�����v����
        if (Input.GetButtonDown("Jump")&&canPushJumpFlag)
        {
            if (jumpFlag) return;
            Debug.Log("�W�����v"+Time.time);
            rb.linearVelocityY = 0; 
            rb.AddForce(transform.up * jumpPower);
            OnOffJumpFlag(true);
        }
    }
    void Move()
    {
        //���E�L�[�̓��͂����m
        num = Input.GetAxisRaw("Horizontal");


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
        if (jumpFlag && mode != beforeMode)
        {
            pendingMode = mode;
            return;
        }

        //�v���C���[�̑��x���ő呬�x�𒴂���������𒆎~
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed) return;

        //���͕������ς�����ꍇ�AON��OFF��؂�ւ���
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

        Debug.Log("�ړ�"+Time.time);
        //���E�L�[�������������ɗ͂��|���Ĉړ�������
        rb.AddForce(transform.right * speed * num * Time.deltaTime);
        //�A�j���[�V�����̃X�s�[�h�𑬓x�ɂ���ĕύX����
        float animeSpeed = 1;
        if (Mathf.Abs(rb.linearVelocityX )>= maxSpeed - 1.0f)
        {
            animeSpeed = 0.5f;
            
        }

        animator.speed = Mathf.Abs(rb.linearVelocityX*animeSpeed) ;
        //�v���C���[�̌�����ύX����
        transform.localScale = new Vector3(1 * mode, 1, 1);

    }

    //���݂̌�����Ԃ�
    public int GetMode()
    {
        return mode;
    }
    public bool GetJumpFlag()
    {
        return jumpFlag;
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

        //�n�ʂɒ��n�������Ɍ������X�V����B
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

            pendingMode = 0;//�g�����烊�Z�b�g
            OnOffJumpFlag(false);//���n��W�����v��ԉ���
        }*/
    }

}




