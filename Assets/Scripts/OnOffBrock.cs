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
    //public string defaultTag = "Selectable";//初期タグをここに保存
    //public Sprite offSprite;//点線(OFF用)

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

        // 初期状態のスプライトを設定（色はいじらない）
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
        //モードがOFFで動く場合
        if (!on)
        {
            moveFlag = false;
            transform.position = orizinalpos;//元の場所に戻す
        }
    }

    public void OffMove()
    {
        moveFlag = false;
        transform.position = orizinalpos;//元の場所に戻す
        if (!on) moveFlag = true;
    }

    void Move()
    {
        if (/*!move ||*/ !moveFlag) return;

        if (moveFlag)
        {
            //Debug.Log("ブロック移動中");
            transform.Translate(movevec * Time.deltaTime);
        }

        if (transform.localPosition.x > movestop.x || transform.localPosition.y > movestop.y)
        {
            //Debug.Log("ブロックの移動を止めました");
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

            //spr.color = new Color(1f, 1f, 1f, 1f); // 不透明
            //box.size = new Vector2(1f, 1f);//オン時の大きさ
        }
        else
        {
            box.enabled = false;
            spr.sprite = offSprite;
            spr.color = Color.white;
            //spr.color = new Color(1f, 1f, 1f, 1f); // 半透明
            //box.size = new Vector2(1f, 1f);//オフ時の大きさ
        }
    }


}
