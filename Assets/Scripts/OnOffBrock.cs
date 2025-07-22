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
    public Sprite newSprite;
    //public string defaultTag = "Selectable";//�����^�O�������ɕۑ�
    //public Sprite offSprite;//�_��(OFF�p)

    bool moveFlag = false;
 

    void Start()
    {
        //if(on&& new) spr = spr.sprite = newSprite;
        //else spr.color = new Color(255, 255, 255, spr.color.a);
        //orizinalpos=transform.position;
        
    }

    void Update()
    {
        Move();
    }

    public void OnMove()
    {
        moveFlag = true;
        if (!on) moveFlag = false;
    }

    public void OffMove()
    {
        moveFlag = false;
        transform.position = orizinalpos;//���̏ꏊ�ɖ߂�
        if (!on) moveFlag = true;
    }
    
    void Move()
    {
        if (!move||!moveFlag) return;

        if (moveFlag)
        {
            transform.Translate(movevec * Time.deltaTime);
        }
        
        if (transform.position.x >= movestop.x || transform.position.y >= movestop.y)
        {
            moveFlag = false;
        }
        
    }

  
}



