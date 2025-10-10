using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;


public class OnOffBrock : MonoBehaviour
{
    public bool on = false;//���̃u���b�N��on�Ŕ��肪���̂��ǂ���
    public bool move = false;//���̃u���b�N�͓����̂�
    public bool loop = false;//��������
    public float moveSpeed = 0.4f;//�ړ����x
    
    public Vector3 orizinalpos = Vector3.zero;
    public Vector3 movestop = Vector3.zero;
    //public Vector3 moveSpeedVec = Vector3.zero;
    
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public SpriteRenderer spr;

    public Sprite onSprite;
    public Sprite offSprite;

    public Animator animator;

    float animationSpeed = 0.03f;
    bool moveFlag = false;
    bool fadeFlag = false;
    bool changed = false;
    bool turn = false;
    bool invalid = false;//�u���b�N�̔����L��������̂��֎~����


    void Start()
    {
        if(orizinalpos==Vector3.zero)orizinalpos = transform.position;
        //if (moveSpeedVec == Vector3.zero)
        //{
        //    if (Mathf.Approximately(orizinalpos.x, movestop.x)==false) moveSpeedVec.x = moveSpeed;
        //    if (Mathf.Approximately(orizinalpos.y, movestop.y)==false) moveSpeedVec.y = moveSpeed;
            
        //}
        if (spr == null) spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Move();
        FadeAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    //�ǉ���---------------------------------------------------------------------

    public void ON(bool animation = false)
    {
        //�����x�ω��A�j���[�V�������ɌĂяo���ꂽ��A�A�j�����~
        fadeFlag = false;

        if (on)
        {
            
            box.enabled = true;
            if(GameManager.instance.GetWaveAnimation()==true)box.isTrigger = false;
            spr.sprite = onSprite;
            //animator.SetTrigger("OnTrigger");
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
            
            box.enabled = false;
            //waveAnimation���̏ꍇ�͓����蔻��̎������ꎞ�I��Trigger�Ŏ��B(enabled����OnOff���]���Ȃ�����)
            if (GameManager.instance.GetWaveAnimation()==true||animation)
            {
                Debug.Log("True�ɂȂ��Ă��邾��?");
                box.enabled = true;
                box.isTrigger = true;
            }
            spr.sprite = offSprite;
            //animator.SetTrigger("OffTrigger");
            Color color = spr.material.color;
            color.a = 0.4f;
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

        if (on)
        {
            box.enabled = false;
            //waveAnimation���̏ꍇ�͓����蔻��̎������ꎞ�I��Trigger�Ŏ��B(enabled����OnOff���]���Ȃ�����)
            if (GameManager.instance.GetWaveAnimation()||animation)
            {
                Debug.Log("True�ɂȂ��Ă��邾��?");
                box.enabled = true;
                box.isTrigger = true;
            }
            spr.sprite = offSprite;
            //animator.SetTrigger("OffTrigger");
            Color color = spr.material.color;
            color.a = 0.4f;
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
     
            box.enabled = true;
            if (GameManager.instance.GetWaveAnimation()) box.isTrigger = false;
            spr.sprite = onSprite;
            //animator.SetTrigger("OnTrigger");
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

    //---------------------------------------------------------------------------

    public void OnfadeAnimation()
    {
        fadeFlag = true;
    }

    public void OnMove()
    {
        //if (!on) return;
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

    //--------------------------------------------------------------------------
    public bool GetChanged()
    {
        return changed;
    }

    public void SetOnChanged()
    {
        //��x�̏Ռ��g��2�񓖂����Ă��ς��Ȃ��悤�Ƀt���O�𗧂ĂĂ���
        changed = true;
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
        else
        {
            box.enabled = true;
            box.isTrigger = true;
        }
    }

    //trigger���肩��enabled����֐؂�ւ���(���ꂪ�����ƁAOnOff�؂�ւ����߂荞�݂���������)
    public void ChangeTriggerToEnabled()
    {
        if (box.isTrigger == false)
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
            box.isTrigger = false;
        }
    }

    //--------------------------------------------------------------------------

    void Move()
    {
        //ON�œ����u���b�N��OFF�̎��ɂ������Ă��܂�Ȃ��悤��moveFlag���m�F����
        if (!moveFlag) return;
        if (move)
        {
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

    void MoveCheck()
    {
        //�G���x�[�^����~�ʒu���߂�����~�߂�
        if (transform.position.x >= movestop.x)
        {
            if (orizinalpos.x > movestop.x) return;
            moveFlag = false;

        }
        if (transform.position.x <= movestop.x)
        {
            if (orizinalpos.x < movestop.x) return;
            moveFlag = false;
        }
    }

    void FadeAnimation()
    {
        if (fadeFlag == false) return;
        Color color = spr.material.color;
        color.a += animationSpeed;
        spr.material.color = color;

        //�����x���P�ȏ�ɂȂ�����A�j���[�V�������I��������
        if (color.a >= 1f && animationSpeed > 0) fadeFlag = false;
        //OFF��Ԃœ����x��0.4��؂�����摜�������ւ��ăA�j���[�V�����I��
        if (color.a <= 0.1f && animationSpeed < 0)
        {
            fadeFlag = false;
            spr.sprite = offSprite;
            color.a = 0.4f;
            spr.material.color = color;
        }

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (!move) return;
    //        if (collision.transform.parent != null) return;
    //        collision.gameObject.transform.SetParent(transform, worldPositionStays: true);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (!move) return;
    //        collision.gameObject.transform.SetParent(null, worldPositionStays:true);
    //    }
    //}


}