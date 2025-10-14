using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public float performanceSpeed = 0.1f;//アニメーションの演出を行うスピード
    public bool doAnimation = true;
    public GameObject brock;//ステージ中にあるONOFFブロック
    Vector3 orizinSize;
    CircleCollider2D circleCollider;
    public static AnimationScript instance;

    bool on = false;
    bool tutorial = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        orizinSize = transform.localScale;
        circleCollider = GetComponent<CircleCollider2D>();
        if (CameraZoomTransition.instance!=null&&CameraZoomTransition.instance.zoomCamera != null) tutorial = true;

    }

    // Update is called once per frame
    void Update()
    {
        //波動が画面外に出たらアニメーションをストップする
        if (transform.localScale.x >= 8f)
        {
            doAnimation = false;
        }
        //波動の大きさを増やしていく
        if (doAnimation)
        {
            Vector3 vec = transform.localScale;
            vec.x += performanceSpeed *Time.unscaledDeltaTime;
            vec.y += performanceSpeed * Time.unscaledDeltaTime;
            transform.localScale = vec;
        }
        //アニメーションがストップされたらサイズを元に戻し、判定を消す
        if (!doAnimation)
        {
            GameManager.instance.OFFChanged();
            transform.localScale = orizinSize;
            circleCollider.enabled = false;
            GameManager.instance.OnOffSlow(false);
            if (tutorial) GameManager.instance.ChangeTriggerToEnabled();
            if (CameraZoomTransition.instance!=null&&CameraZoomTransition.instance.zoomCamera != null)
            {
                CameraZoomTransition.instance.StartZoom();
                //プレイヤー位置固定と入力はじきの処理を呼び出す
                PlayerScript.instance.cannotMoveMode();
            }
            Destroy(gameObject);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<OnOffBrock>() != null)
        {
            if (tutorial) return;
            OnOffBrock brock = collision.gameObject.GetComponent<OnOffBrock>();
            //OnOffが既に切り替わっていた場合は、処理を終了
            if (brock.GetChanged() == true) return;
            brock.SetOnChanged();

            //OnOffの属性を切り替え
            brock.on = !brock.on;

            //brock.OnfadeAnimation();
            //brock.SetOnChanged();

            int num = PlayerScript.instance.GetMode();//現在の向きを取得
            if (num > 0) on = true;
            else if (num < 0) on = false;

            //更新を行う
            if (on) brock.ON(true);
            else brock.OFF(true);


            //brock.ChangeTriggerToEnabled();

            brock.OnfadeAnimation();
            //brock.SetOnChanged();



            Debug.Log("On??Off?????]");
        }



    }

    public bool GetTutorialMode()
    {
        return tutorial;
    }


}