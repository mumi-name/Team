using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;


public class OnOffBrock : MonoBehaviour
{
    public float moveSpeed = 0.4f;//�ړ����x
    public bool on = false;//���̃u���b�N��on�Ŕ��肪���̂��ǂ���
    public bool move = false;//���̃u���b�N�͓����̂�
    public bool loop = false;//��������
    
    public Vector3 orizinalpos = Vector3.zero;
    public Vector3 movestop = Vector3.zero;
    //�R���|�[�l���g
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public SpriteRenderer spr;
    public Animator animator;
    //�X�v���C�g
    public Sprite onSprite;
    public Sprite offSprite;
    //�����Q��
    private float animationSpeed = 0.03f;
    private bool moveFlag = false;
    private bool fadeFlag = false;
    private bool changed = false;
    private bool turn = false;
    private bool stop = false;
    private bool invalid = false;//�u���b�N�̔����L��������̂��֎~����

    void Start()
    {
        if(orizinalpos==Vector3.zero)orizinalpos = transform.position;
        if (spr == null) spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FadeAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }


    public void ON(bool animation = false)
    {
        //�����x�ω��A�j���[�V�������ɌĂяo���ꂽ��A�A�j�����~
        fadeFlag = false;

        PlayerScript.instance.transform.SetParent(null);

        if (on)
        {
            //�����蔻���L����
            box.enabled = true;
            if (GameManager.instance.GetWaveAnimation() == true && !changed)
            {
                box.isTrigger = false;
            }

            //�摜�E�A�j���[�V������ύX
            spr.sprite = onSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", true);
                animator.speed = 2;
            }
            Color color = spr.material.color;
            color.a = 1f;
            //�g���A�j���[�V��������Ăяo���ꂽ��Aa�l��0.4�ɂ��Ƃ��i�A�j���[�V�������̉��o�p�j
            if (animation)
            {
                color.a = 0.1f;
                if (animationSpeed < 0) animationSpeed *= -1;
                
            }
            spr.material.color = color;

        }
        else
        {
            //�����蔻��𖳌���
            box.enabled = false;
            //waveAnimation���̏ꍇ�͓����蔻��̎������ꎞ�I��Trigger�Ŏ��B(enabled����OnOff���]���Ȃ�����)
            if ((GameManager.instance.GetWaveAnimation()==true||animation)&&!changed)
            {
                box.enabled = true;
                box.isTrigger = true;
            }

            //�摜�E�A�j���[�V������ύX
            spr.sprite = offSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", false);
                animator.speed = 1;
            }
            Color color = spr.material.color;
            color.a = 1f;
            if (animation)
            {
                spr.sprite = onSprite;
                color.a = 1f;
                if (animationSpeed > 0) animationSpeed *= -1;
            }
            spr.material.color = color;

        }
        OnMove();
    }

    public void OFF(bool animation = false)
    {
        //�����x�ω��A�j���[�V�������ɌĂяo���ꂽ��A�A�j�����~
        fadeFlag = false;

        PlayerScript.instance.transform.SetParent(null);

        if (on)
        {
            //�����蔻��𖳌���
            box.enabled = false;
            //waveAnimation���̏ꍇ�͓����蔻��̎������ꎞ�I��Trigger�Ŏ��B(enabled����OnOff���]���Ȃ�����)
            if ((GameManager.instance.GetWaveAnimation()||animation)&&!changed)
            {
                Debug.Log("True�ɂȂ��Ă��邾��?");
                box.enabled = true;
                box.isTrigger = true;
               
            }

            //�摜�E�A�j���[�V������ύX
            spr.sprite = offSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", false);
                animator.speed = 1;
            }
            Color color = spr.material.color;
            color.a = 1f;
            if (animation)
            {
                spr.sprite = onSprite;
                color.a = 1f;
                if (animationSpeed > 0) animationSpeed *= -1;
                
            }
            spr.material.color = color;

        }
        else
        {
            //�����蔻���L����
            box.enabled = true;
            if (GameManager.instance.GetWaveAnimation() && !changed)
            {
                box.isTrigger = false;
            }

            //�摜�E�A�j���[�V������ύX
            spr.sprite = onSprite;
            if (animator != null)
            {
                animator.SetBool("OnOffBool", true);
                animator.speed = 2;
            }
            Color color = spr.material.color;
            color.a = 1f;
            //�g���A�j���[�V��������Ăяo���ꂽ��Aa�l��0.4�ɂ��Ƃ��i�A�j���[�V�������̉��o�p�j
            if (animation)
            {
                color.a = 0.1f; 
                if (animationSpeed < 0) animationSpeed *= -1;
                
            }
            spr.material.color = color;

        }
        OffMove();

    }

    public void OnfadeAnimation()
    {
        fadeFlag = true;
    }

    public void OnMove()
    {
        moveFlag = true;
        //���[�h��OFF�œ����ꍇ
        if (!on)
        {
            moveFlag = false;
            transform.position = orizinalpos;//���̏ꏊ�ɖ߂�
        }
    }

    public void OffMove()
    {
        moveFlag = false;
        transform.position = orizinalpos;//���̏ꏊ�ɖ߂�
        if (!on) moveFlag = true;
    }

    public void SetOnChanged()
    {
        //��x�̏Ռ��g��2�񓖂����Ă��ς��Ȃ��悤�Ƀt���O�𗧂ĂĂ���
        changed = true;
        ChangeTriggerToEnabled();
    }

    public void SetOffChanged()
    {
        //�t���O���~�낷
        changed = false;
    }


    //enabled���肩��trigger����֐؂�ւ���(���ꂪ�����ƁAOnOff�؂�ւ�����肭�����Ȃ�)
    public void ChangeEnabledToTrigger()
    {
        if (box.enabled == true)
        {
            box.isTrigger = false;
        }
        else if(box.enabled == false)
        {
            box.enabled = true;
            box.isTrigger = true;
        }
    }

    //trigger���肩��enabled����֐؂�ւ���(���ꂪ�����ƁAOnOff�؂�ւ����߂荞�݂���������)
    public void ChangeTriggerToEnabled()
    {
        if (box.isTrigger)
        {
            box.enabled = false;
            box.isTrigger = false;
        }
        else if(box.isTrigger == false)
        {
            box.enabled = true;
        }
    }
    public void setStopFlag(bool flag)
    {
        stop = flag;
    }
    public bool GetChanged()
    {
        return changed;
    }


    void Move()
    {
        //ON�œ����u���b�N��OFF�̎��ɂ������Ă��܂�Ȃ��悤��moveFlag���m�F����
        if (!moveFlag) return;
        if (move)
        {
            if (stop) return;//��~���߂��o�Ă������~
            if (loop && Mathf.Approximately(transform.position.x, movestop.x) && Mathf.Approximately(transform.position.y, movestop.y)) Invoke("OnTurn", 0.7f);
            if (Mathf.Approximately(transform.position.x, orizinalpos.x) && Mathf.Approximately(transform.position.y, orizinalpos.y)) Invoke("OffTurn", 0.7f);

            if (!turn)
            {
                transform.position = Vector3.MoveTowards
                (transform.position, movestop, Mathf.Abs(moveSpeed) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,orizinalpos, Mathf.Abs(moveSpeed) * Time.deltaTime);
            }
        }
        
    }

    void OnTurn()
    {
        turn = true;
    }

    void OffTurn()
    {
        turn = false;
    }

    void FadeAnimation()
    {
        if (fadeFlag == false) return;
        Color color = spr.material.color;
        color.a += animationSpeed;
        spr.material.color = color;

        //�����x���P�ȏ�ɂȂ�����A�j���[�V�������I��������
        if (color.a >= 1f && animationSpeed > 0)
        {
            fadeFlag = false;
            PlayerScript.instance.ignoreMove(false);
        }
        //OFF��Ԃœ����x��0.4��؂�����摜�������ւ��ăA�j���[�V�����I��
        if (color.a <= 0.1f && animationSpeed < 0)
        {
            fadeFlag = false;
            spr.sprite = offSprite;
            color.a = 0.4f;
            spr.material.color = color;
            PlayerScript.instance.ignoreMove(false);
        }

    }


}