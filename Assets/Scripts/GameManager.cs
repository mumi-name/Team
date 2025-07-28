using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public List<OnOffBrock> brocks;//ステージ中にあるONOFFブロック
    public static GameManager instance;

    public Sprite onSprite;
    public Sprite offSprite;

    void Start()
    {
        instance = this;
        ON();
    }

    // Update is called once per frame
    void Update()
    {
        //リセットボタンが押されたらゲームシーンをリセット
        if (Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public void ON()
    {
        foreach (var brock in brocks)
        {

            if (brock.on)
            {
                brock.box.enabled = true;
                brock.spr.sprite = onSprite;
                Color color = brock.spr.material.color;
                color.a = 1f;
                brock.spr.material.color = color;
                //spr.sprite = onSprite;
                //brock.spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 255);
                //brock.spr.material.color= new Color(spr.color.r, spr.color.g, spr.color.b, 255);
                //Debug.Log("オン起動中");
            }
            else
            {
                brock.box.enabled = false;
                brock.spr.sprite = offSprite;
                Color color = brock.spr.material.color;
                color.a = 0.4f;
                brock.spr.material.color = color;
                //spr.sprite = offSprite;
                //brock.spr.material.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.4f);
                //brock.spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.4f);

            }

            //brock.ApplyVisual();
            brock.OnMove();
        }

    }
    public void OFF()
    {

        foreach (var brock in brocks)
        {

            if (brock.on)
            {

                brock.box.enabled = false;
                brock.spr.sprite = offSprite;
                Color color=brock.spr.material.color;
                color.a = 0.4f;
                brock.spr.material.color= color;
                //spr.sprite = offSprite;
                //brock.spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.4f);
                //brock.spr.material.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.4f);
                //Debug.Log("オフ起動中");

            }
            else
            {
                brock.box.enabled = true;
                brock.spr.sprite = onSprite;
                Color color = brock.spr.material.color;
                color.a = 1f;
                brock.spr.material.color = color;
                //spr.sprite = onSprite;
                //brock.spr.material.color = new Color(spr.color.r, spr.color.g, spr.color.b, 255);
                //brock.spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 255);
            }

            brock.OffMove();
            //brock.ApplyVisual();
        }
    }

    public void ChangeOnOff(bool on)
    {
        foreach (var brock in brocks)
        {
            //brock.on = !brock.on;
            brock.on = !brock.on;
        }
        if (on) ON();
        else OFF();
    }
}



//<変数宣言に書いてあったコメント文>

/*public BoxCollider2D box;
    public SpriteRenderer spr;
    bool jumpFlag = false;*/

//<Updateに書いてあったコメント文>
/*if (Input.GetKey(KeyCode.RightArrow))
{
    if (jumpFlag) return;
    box.enabled = false;
    spr.color = new Color(255, 255, 255, 0);
}
if (Input.GetKey(KeyCode.LeftArrow))
{
    if (jumpFlag) return;
    box.enabled = true;
    spr.color = new Color(255, 255, 255, 255);
}
*/
