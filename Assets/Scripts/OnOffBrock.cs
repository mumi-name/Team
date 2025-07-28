using UnityEngine;

public class OnOffBrock : MonoBehaviour
{

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
    //public Sprite offSprite;//�_��(OFF�p)

    bool moveFlag = false;


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

        // ������Ԃ̃X�v���C�g��ݒ�i�F�͂�����Ȃ��j
        //ApplyVisual();

    }

    void Update()
    {
        Move();
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
            //Debug.Log("�u���b�N�̈ړ����~�߂܂���");
            moveFlag = false;
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
