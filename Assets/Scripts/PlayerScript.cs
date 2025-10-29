using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Player")]
    public float speed = 1000f;//�ړ����x
    public float maxSpeed = 5f;//�ő呬�x
    public float jumpPower = 300f;//�W�����v��
    public float slowMaxSpeed = 0.1f;
    public float slowGravity = 0.1f;
    //public float footstepCooldown = 0.25f;//�����̊Ԋu
    //private float lastFootstepTime = 0f;   //�Ō�ɖ炵������
    public bool jumpMode = true;//�W�����v�̃A���E�i�V
    public bool backJump = true;
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerScript instance;


    public bool bagTaisyo5 = false;

    int beforeMode = 1;//�ȑO�̌����iON��OFF���j
    int mode = 1;//���݂̌����iON��OFF���j
    int pendingMode = 0;//�L�^�p
    float num;
    bool jumpFlag = true;//���݃W�����v����
    bool canPushJumpFlag = true;//�W�����v�{�^���������邩�ǂ���
    public bool isDead = false;
    //-------------------------------------------------------------------------------------------
    bool ignoreInput = false;//���͂�S�Ė�������
    //-------------------------------------------------------------------------------------------

    
    
    //private OnOffBrock onoffBrock;
    //FIX:�o�O
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
        //Debug.Log("VeloX�F" + rb.linearVelocityX.ToString());
        Debug.Log("VeloX�F" + rb.linearVelocityX.ToString() + "  num�F" + num.ToString() + "  mode:" + mode.ToString());
        //���͂𖳎����A�������s��Ȃ�
        if (ignoreInput) return;
        if(isDead ) return;
        

        if (GameManager.instance.GetWaveAnimation() && Mathf.Abs(rb.linearVelocity.y)!=0) jumpFlag = true;
        if (jumpMode)Jump();
        Move();

    }

    private void FixedUpdate()
    {
        //���͂𖳎����A�������s��Ȃ�
        if (ignoreInput) return;
        if (isDead) return;

        //�v���C���[�̑��x���ő呬�x�𒴂���������𒆎~
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed) return;
        //���E�L�[�������������ɗ͂��|���Ĉړ�������
        rb.linearVelocityX = speed * mode * Mathf.Abs(num) * Time.deltaTime;

        //�v���C���[�̌����Ƌt�����ɗ͂��|�����Ă���ꍇ��return
        if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX) && rb.linearVelocityX != 0)
        {
            if (!backJump)
            {
                rb.linearVelocityX = 0;
                return;
            }
            //rb.linearVelocityX *= -1.0f;

        }


        //�W�����v���ɈႤ�����������Ȃ��悤�ɂ���
        //if (jumpFlag && mode != beforeMode)
        //{
        //    pendingMode = mode;
        //    return;
        //}

    }

    void Jump()
    {
        //�X�y�[�X�{�^���������ꂽ��W�����v����
        if (Input.GetButtonDown("Jump") && canPushJumpFlag)
        {   
            if (jumpFlag) return;
            AudioManager.instance.PlaySE("�W�����v");
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

        ////�W�����v���ɈႤ�����ɓ��͂��ꂽ�琂������������
        //if (jumpFlag && mode != beforeMode)
        //{
        //    num = 0;
        //    rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        //    return;
        //}

        //���E�L�[��������ĂȂ��ꍇ�A�~�߂�
        if (Mathf.Abs(num) < 0.2f)//0.2f�ɖ߂��Ƃ� 10/24
        {
            num = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            animator.speed = 0;
            return;
        }

        //�W�����v���ɕʕ����ɗ͂��|�����Ă���ꍇ�i�I�u�W�F�N�g�̒[���g�����o�O�΍�j
        //velocity��0�ɂ���return����
        if (Mathf.Sign(beforeMode) != Mathf.Sign(rb.linearVelocityX) && rb.linearVelocityX != 0)
        {
            if (!backJump)
            {
                rb.linearVelocityX = 0;
                return;
            }

           rb.linearVelocityX *= -1.0f;
        }

        //�W�����v���ɈႤ�����������Ȃ��悤�ɂ���
        if (jumpFlag && mode != beforeMode)
        {
            pendingMode = mode;
            //�����������������ꍇ�̓R�����g�A�E�g
            //num = 0;

            //���͕����Ɣ��Ε����ɗ͂��|�����Ă����甽�]
            if (Mathf.Sign(mode) == Mathf.Sign(num))
            {
                if (Mathf.Sign(rb.linearVelocityX) != Mathf.Sign(num)) rb.linearVelocityX *= -1.0f;
            }
            return;
        }


        //���͕������ς�����ꍇ�AON��OFF��؂�ւ���
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
            AudioManager.instance.PlaySE("����");
        }

        //�A�j���[�V�����̃X�s�[�h�𑬓x�ɂ���ĕύX����
        float animeSpeed = 1;
        if (Mathf.Abs(rb.linearVelocityX) >= maxSpeed - 1.0f)
        {
            animeSpeed = 0.3f;
        }

        animator.speed = Mathf.Abs(rb.linearVelocityX * animeSpeed);
        //animator.speed = 1;
        //�v���C���[�̌�����ύX����
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)* mode*-1.0f, transform.localScale.y, 1);

    }

    
    public void ignoreMove(bool mode)
    {
        if (mode == false && AnimationScript.instance.GetTutorialMode())
        {
            Debug.Log("�͂����������o���Ă��邺�[�[�[2");
            return;
        }
        ignoreInput = mode;
        Debug.Log("�͂����������ē����Ă���̂��E�E�ȁH");
        //if(mode)rb.linearVelocityX = 0;
    }

    //�����Ȃ��悤�ɋ󒆂ɌŒ�
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

    //���݂̌�����Ԃ�
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



    //�W�����v��Ԃ�؂�ւ���
    public void OnOffJumpFlag(bool flag)
    {
        //�W�����v���Ƀt���O��false�ɂ��悤�Ƃ����ꍇ���^�[������
        //if (flag == false && Mathf.Abs(rb.linearVelocityY) > 0) return;
        jumpFlag = flag;
        animator.SetBool("JumpBool", flag);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //㩂ɐG�ꂽ�烊�Z�b�g����
        if (collision.gameObject.CompareTag("Trap") && !isDead)
        {
            animator.speed = 1;
            isDead = true;
            cannotMoveMode(true);
            animator.SetTrigger("DeathTrigger");
            Debug.Log("���S��̏����͌Ăяo����Ă���?");
            AudioManager.instance.PlaySE("�����");
            TimerManager.instance.AddDeath();
            TrapController trap = collision.gameObject.GetComponent<TrapController>();
            
            //�V�[���J�ڂ͎��S�A�j���[�V�������I����ɍĐ����邽�ߏ�����ʃX�N���v�g�Ɉڍs(10/17)
            //�A�j���[�V�����C�x���g��SceneTransition��Transition()���Ă�
            
        }

        //if()

        
    }

}