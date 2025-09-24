using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.GraphicsBuffer;


public class OnOffBrock : MonoBehaviour
{
    public float animationSpeed = 0.03f;
    public bool on = false;
    public bool move = false;
    //public Vector3 movevec = Vector3.zero;
    public float moveSpeed=0.4f;//移動速度
    public Vector3 movestop = Vector3.zero;
    /*public*/ Vector3 orizinalpos = Vector3.zero;
    public BoxCollider2D box;
    public SpriteRenderer spr;

    public Sprite onSprite;
    public Sprite offSprite;
    //public string defaultTag = "Selectable";//初期タグをここに保存
    //public Sprite offSprite;//?_??(OFF?p)

    bool moveFlag = false;
    bool fadeFlag = false;
    bool changed = false;
    bool invalid = false;//ブロックの判定を有効化するのを禁止する


    void Start()
    {
        orizinalpos = transform.position;
        if (spr == null) spr = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        Move();
        FadeAnimation();
    }

    //追加分---------------------------------------------------------------------

    public void ON(bool animation = false)
    {
        if (on)
        {
            
            box.enabled = true;
            if(GameManager.instance.GetWaveAnimation())box.isTrigger = false;
            spr.sprite = onSprite;
            Color color = spr.material.color;
            color.a = 1f;
            //波動アニメーションから呼び出されたら、a値を0.4にしとく（アニメーション時の演出用）
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
            //waveAnimation中の場合は当たり判定の取り方を一時的にTriggerで取る。(enabledだとOnOff反転しないため)
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
            //waveAnimation中の場合は当たり判定の取り方を一時的にTriggerで取る。(enabledだとOnOff反転しないため)
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

            //波動アニメーションから呼び出されたら、a値を0.4にしとく（アニメーション時の演出用）
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

    //--------------------------------------------------------------------------
    public bool GetChanged()
    {
        return changed;
    }

    public void SetOnChanged()
    {
        //一度の衝撃波に2回当たっても変わらないようにフラグを立てておく
        changed = true;
    }

    public void SetOffChanged()
    {
        //フラグを降ろす
        changed = false;
    }


    //enabled判定からtrigger判定へ切り替える(これが無いと、OnOff切り替えが上手くいかない)
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

    //trigger判定からenabled判定へ切り替える(これが無いと、OnOff切り替え時めり込みが発生する)
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

        //ONで動くブロックがOFFの時にも動いてしまわないようにmoveFlagを確認する
        if (moveFlag)
        {
            if(move)transform.position = Vector3.MoveTowards(transform.position, movestop, Mathf.Abs(moveSpeed) * Time.deltaTime);
        }

    }

    void FadeAnimation()
    {
        if (fadeFlag == false) return;
        Color color = spr.material.color;
        color.a += animationSpeed;
        spr.material.color = color;

        //透明度が１以上になったらアニメーションを終了させる
        if (color.a >= 1f && animationSpeed > 0) fadeFlag = false;
        //OFF状態で透明度が0.4を切ったら画像を差し替えてアニメーション終了
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