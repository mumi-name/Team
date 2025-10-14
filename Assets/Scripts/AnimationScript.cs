using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public float performanceSpeed = 0.1f;//�A�j���[�V�����̉��o���s���X�s�[�h
    public bool doAnimation = true;
    public GameObject brock;//�X�e�[�W���ɂ���ONOFF�u���b�N
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
        //�g������ʊO�ɏo����A�j���[�V�������X�g�b�v����
        if (transform.localScale.x >= 8f)
        {
            doAnimation = false;
        }
        //�g���̑傫���𑝂₵�Ă���
        if (doAnimation)
        {
            Vector3 vec = transform.localScale;
            vec.x += performanceSpeed *Time.unscaledDeltaTime;
            vec.y += performanceSpeed * Time.unscaledDeltaTime;
            transform.localScale = vec;
        }
        //�A�j���[�V�������X�g�b�v���ꂽ��T�C�Y�����ɖ߂��A���������
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
                //�v���C���[�ʒu�Œ�Ɠ��͂͂����̏������Ăяo��
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
            //OnOff�����ɐ؂�ւ���Ă����ꍇ�́A�������I��
            if (brock.GetChanged() == true) return;
            brock.SetOnChanged();

            //OnOff�̑�����؂�ւ�
            brock.on = !brock.on;

            //brock.OnfadeAnimation();
            //brock.SetOnChanged();

            int num = PlayerScript.instance.GetMode();//���݂̌������擾
            if (num > 0) on = true;
            else if (num < 0) on = false;

            //�X�V���s��
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