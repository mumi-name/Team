using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public float slowSpeed = 0.2f;
    public List<OnOffBrock> brocks;//�X�e�[�W���ɂ���ONOFF�u���b�N
    public static GameManager instance;

    public Sprite onSprite;
    public Sprite offSprite;

    float targetScale = 1;
    bool waveAnimation = false;//�g���A�j���[�V�����̍Œ����ǂ���?
    //bool slow = false;
    public float finalTime;//�^�C�}�[�}�l�[�W���[�p�ϐ�

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        ON();
    }

    // Update is called once per frame
    void Update()
    {
        //���Z�b�g�{�^���������ꂽ��Q�[���V�[�������Z�b�g
        if (Input.GetButtonDown("Reset"))
        {
            //�X���[���[�V�����𑦍��ɏI��
            //slow = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            //�V�[����J�ڂ�����
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        changeSlow(targetScale);

    }

    //�ǉ���---------------------------------------------------------------------

    public void ON()
    {
        foreach (var brock in brocks)
        {

            brock.ON();
        }

    }
    public void OFF()
    {

        foreach (var brock in brocks)
        {

            brock.OFF();
        }
    }

    public void OFFChanged()
    {
        foreach (var brock in brocks)
        {

            brock.SetOffChanged();
        }
    }

    //�X���[���[�V�����̐؂�ւ�
    public void OnOffSlow(bool on)
    {
        if (on)
        {
            //slow = true;
            targetScale = slowSpeed;
            waveAnimation = true;
        }
        else
        {
            targetScale = 1;
            waveAnimation = false;
            ChangeTriggerToEnabled();
        }
    }

    void changeSlow(float targetScale)
    {
        //if (!slow) return;
        //�X���[���[�V�����ɃO���f�[�V��������������
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, 10  * Time.unscaledDeltaTime);
        //�������Z���J�N���Ȃ��悤�Ɏ����̔{�������킹��
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }


    //------------------------------------------------------------------------------

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

    //enabled���肩��trigger����֐؂�ւ���(���ꂪ�����ƁAOnOff�؂�ւ�����肭�����Ȃ�)
    public void ChangeEnabledToTrigger()
    {
        foreach(var brock in brocks)
        {
            brock.ChangeEnabledToTrigger();
        }
    }

    //trigger���肩��enabled����֐؂�ւ���(���ꂪ�����ƁAOnOff�؂�ւ�����肭�����Ȃ�)
    public void ChangeTriggerToEnabled()
    {
        foreach (var brock in brocks)
        {
            brock.ChangeTriggerToEnabled();
        }
    }

    public bool GetWaveAnimation()
    {
        return waveAnimation;
    }
    
}



//<???????????????????????R?????g??>

/*public BoxCollider2D box;
    public SpriteRenderer spr;
    bool jumpFlag = false;*/

//<Update???????????????R?????g??>
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