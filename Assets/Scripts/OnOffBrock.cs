using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;


public class OnOffBrock : MonoBehaviour
{
    public float animationSpeed = 0.03f;
    public bool on = false;
    public bool move = false;
    public Vector3 movevec = Vector3.zero;
    public Vector3 movestop = Vector3.zero;
    public Vector3 orizinalpos = Vector3.zero;
    public BoxCollider2D box;
    public SpriteRenderer spr;

    public Sprite onSprite;
    public Sprite offSprite;
    //public string defaultTag = "Selectable";//�����^�O�������ɕۑ�
    //public Sprite offSprite;//?_??(OFF?p)

    bool moveFlag = false;
    bool fadeFlag = false;
    bool changed = false;
    bool invalid = false;//�u���b�N�̔����L��������̂��֎~����


    void Start()
    {
        /*
        if(spr != null){
            if (on && onSprite != null) spr.sprite = onSprite;
            else if (!on && offSprite != null)
            {
                spr.sprite = offSprite;
                //spr.color = new Color(255, 255, 255, spr.color.a);
            }
            spr.color = new Color(1f, 1f, 1f, spr.color.a);
        }
        */
        if (spr == null) spr = GetComponent<SpriteRenderer>();

        //orizinalpos = transform.position;

        //������Ԃ̃X�v���C�g��ݒ�i�F�͂�����Ȃ��j
        //ApplyVisual();

    }

    void Update()
    {
        Move();
        FadeAnimation();
    }

    //�ǉ���---------------------------------------------------------------------

    public void ON(bool animation = false)
    {
        if (on)
        {
            
            box.enabled = true;
            if(GameManager.instance.GetWaveAnimation())box.isTrigger = false;
            spr.sprite = onSprite;
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
            if (GameManager.instance.GetWaveAnimation())
            {
                box.enabled = true;
                box.isTrigger = true;
            }
            spr.sprite = offSprite;
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
        if (on)
        {

            box.enabled = false;
            //waveAnimation���̏ꍇ�͓����蔻��̎������ꎞ�I��Trigger�Ŏ��B(enabled����OnOff���]���Ȃ�����)
            if (GameManager.instance.GetWaveAnimation())
            {
                box.enabled = true;
                box.isTrigger = true;
            }
            spr.sprite = offSprite;
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
        if (/*!move ||*/ !moveFlag) return;

        if (moveFlag)
        {
            //Debug.Log("�u���b�N�ړ���");
            transform.Translate(movevec * Time.deltaTime);
        }

        if (transform.localPosition.x > movestop.x || transform.localPosition.y > movestop.y)
        {
            //Debug.Log("�u���b�N�̈ړ����~");
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


    public void ApplyVisual()
    {
        if (on)
        {
            box.enabled = true;
            spr.sprite = onSprite;
            spr.color = Color.white;

            //spr.color = new Color(1f, 1f, 1f, 1f); // �s����
            //box.size = new Vector2(1f, 1f);//�I�����̑傫��
        }
        else
        {
            box.enabled = false;
            spr.sprite = offSprite;
            spr.color = Color.white;
            //spr.color = new Color(1f, 1f, 1f, 1f); // ������
            //box.size = new Vector2(1f, 1f);//�I�t���̑傫��
        }
    }

  


}