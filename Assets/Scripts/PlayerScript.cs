using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 1000f;//�ړ����x
    public float maxSpeed = 5f;//�ő呬�x
    public float jumpPower = 300f;//�W�����v��
    public float slowMaxSpeed = 0.1f;
    public float slowGravity = 0.1f;
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerScript instance;

    int beforeMode = 1;//�ȑO�̌����iON��OFF���j
    int mode = 1;//���݂̌����iON��OFF���j
    int pendingMode = 0;//�L�^�p
    float num;
    bool jumpFlag = true;//���݃W�����v����
    bool canPushJumpFlag = true;//�W�����v�{�^���������邩�ǂ���
    private bool isDead = false;
    //-------------------------------------------------------------------------------------------
    bool ignoreInput = false;//���͂�S�Ė�������
    //-------------------------------------------------------------------------------------------

    
    
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
        //-------------------------------------------------------------------------------------------
        if (ignoreInput) return;//���͂𖳎����A�������s��Ȃ�
        //-------------------------------------------------------------------------------------------

        //Debug.Log("velocity:" + rb.linearVelocityY);

        if(ignoreInput) return;
        Jump();
        Move();

    }

    void Jump()
    {
        //�X�y�[�X�{�^���������ꂽ��W�����v����
        if (Input.GetButtonDown("Jump") && canPushJumpFlag)
        {
            if (jumpFlag) return;
            Debug.Log("�W�����v" + Time.time);
            rb.linearVelocityY = 0;
            rb.AddForce(transform.up * jumpPower);
            OnOffJumpFlag(true);
        }
    }
    void Move()
    {
        //���E�L�[�̓��͂����m
        num = Input.GetAxisRaw("Horizontal");


        if (num > 0) mode = 1;
        if (num < 0) mode = -1;

        //���E�L�[��������ĂȂ��ꍇ�A�~�߂�
        if (Mathf.Abs(num) < 0.5)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;

        }

        //FIX�F�o�O�i�R���g���[���[�ő��삷��ƁA�����Ȓl�������Ē�~���������Ă��܂��j
        /*if (num == 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;

        }*/

        //�W�����v���ɕʕ����ɗ͂��|�����Ă���ꍇ�i�I�u�W�F�N�g�̒[���g�����o�O�΍�j
        if (jumpFlag)
        {
            //velocity��0�ɂ���return����
            if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX) && rb.linearVelocityX != 0)
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

        //Debug.Log("�ړ�"+Time.time);
        //���E�L�[�������������ɗ͂��|���Ĉړ�������
        rb.AddForce(transform.right * speed * mode * Time.deltaTime);
        //�A�j���[�V�����̃X�s�[�h�𑬓x�ɂ���ĕύX����
        float animeSpeed = 1;
        if (Mathf.Abs(rb.linearVelocityX) >= maxSpeed - 1.0f)
        {
            animeSpeed = 0.3f;

        }

        animator.speed = Mathf.Abs(rb.linearVelocityX * animeSpeed);
        //�v���C���[�̌�����ύX����
        transform.localScale = new Vector3(1 * mode, 1, 1);

    }

    //�����Ȃ��悤�ɋ󒆂ɌŒ�
    public void cannotMoveMode()
    {
        ignoreInput = true;
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        rb.gravityScale = 0;
    }

    public void canMoveMode()
    {
        ignoreInput = false;
        rb.gravityScale = 1.7f;
    }

    //���݂̌�����Ԃ�
    public int GetMode()
    {
        //return mode;
        return (int)transform.localScale.x;
    }
    public bool GetJumpFlag()
    {
        return jumpFlag;
    }
    //--------------------------------------------------------------------------



    //�W�����v��Ԃ�؂�ւ���
    public void OnOffJumpFlag(bool flag)
    {
        //�W�����v���Ƀt���O��false�ɂ��悤�Ƃ����ꍇ���^�[������
        if (flag == false && Mathf.Abs(rb.linearVelocityY) > 0) return;
        jumpFlag = flag;
        animator.SetBool("JumpBool", flag);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //㩂ɐG�ꂽ�烊�Z�b�g����
        if (collision.gameObject.CompareTag("Trap") && !isDead)
        {
            isDead = true;
            TimerManager.instance.AddDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //�n�ʂɒ��n�������Ɍ������X�V����
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

            pendingMode = 0;//?g?????????Z?b?g
            OnOffJumpFlag(false);//???n???W?????v????????
        }*/
    }

}