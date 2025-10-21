using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public float slowSpeed = 1f;
    public List<OnOffBrock> brocks;//�X�e�[�W���ɂ���ONOFF�u���b�N
    public List<TrapController> traps;//�����X�g
    public static GameManager instance;

    public Sprite onSprite;
    public Sprite offSprite;

    float targetScale = 1;
    bool waveAnimation = false;//�g���A�j���[�V�����̍Œ����ǂ���?
    //bool slow = false;
    public float finalTime;//�^�C�}�[�}�l�[�W���[�p�ϐ�

    void Awake()
    {
        instance = this;

        // traps ���X�g����̏ꍇ�A�����ŃV�[������ TrapController ���擾
        //if (traps == null || traps.Count == 0)
        //{
        //    traps = new List<TrapController>(FindObjectsOfType<TrapController>());
        //}
    }
    void Start()
    {
        //�X�e�[�W01�ɓ�������J�E���g��S�ă��Z�b�g
        if (SceneManager.GetActiveScene().name == "01")
        {
            if (TimerManager.instance != null)
            {
                TimerManager.instance.AllCountReset();
            }
        }
        if(IrisShot.instance != null)
        {
            IrisShot.instance.IrisIN();
        }
        AudioManager.instance.PlayBGM("BGM3");
        //DontDestroyOnLoad(gameObject);
        ON();
    }

    // Update is called once per frame
    void Update()
    {

        /*���Z�b�g�{�^���������ꂽ��Q�[���V�[�������Z�b�g
        if (Input.GetButtonDown("Reset"))
        {
            AudioManager.instance.PlaySE("���Z�b�g");
            //�X���[���[�V�����𑦍��ɏI��
            //slow = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            TimerManager.instance.AddDeath();
            IrisShot.instance.ResetIris();
            if(IrisShot.instance != null)
            {
                Destroy(IrisShot.instance.gameObject);//�Â����̂��폜
            }
            //�V�[����J�ڂ�����
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        changeSlow(targetScale);
        */
        if (Input.GetButtonDown("Reset"))
        {
            StartCoroutine(ResetSceneWithSE());
        }
        changeSlow(targetScale);
    }

    //�ǉ���---------------------------------------------------------------------

    public void ON()
    {
        //if (waveAnimation && Mathf.Abs(PlayerScript.instance.rb.linearVelocity.y) > 0.5) return;
        foreach (var brock in brocks)
        {
            brock.ON();
            
        }
        foreach(var trap in traps)
        {
            //trap.isActive = true;
            trap.ToggleTrap();
            
        }
        AudioManager.instance.PlaySE2("ONOFF");
        ScreenMove.instance.Toggle();
    }
    public void OFF()
    {
        //if (waveAnimation && Mathf.Abs(PlayerScript.instance.rb.linearVelocity.y) > 0.5) return;
        foreach (var brock in brocks)
        {
            brock.OFF();
        }
        foreach(var trap in traps)
        {
            //trap.isActive = false;
            trap.ToggleTrap();
            
        }
        AudioManager.instance.PlaySE2("ONOFF");
        ScreenMove.instance.Toggle();
    }

    public void OFFChanged()
    {
        foreach (var brock in brocks)
        {

            brock.SetOffChanged();
        }
        AudioManager.instance.PlaySE2("ONOFF");
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

    //���Z�b�g�����̃R���[�`��
    private IEnumerator ResetSceneWithSE()
    {
        // ���Z�b�g����炷
        AudioManager.instance.PlaySE2("���Z�b�g");

        // �X���[���[�V�����𑦍��ɏI��
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // ���S�J�E���g��IrisShot�̃��Z�b�g
        TimerManager.instance?.AddDeath();
        IrisShot.instance?.ResetIris();

        if (IrisShot.instance != null)
        {
            Destroy(IrisShot.instance.gameObject); // �Â����̂��폜
        }

        // SE�����������鎞�Ԃ�҂i��: 0.2�b�j
        yield return new WaitForSeconds(0.2f);

        // �V�[���������[�h
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ToggleTraps()
    {
        if (traps != null)
        {
            foreach (var trap in traps)
            {
                if (!trap.locked)
                {
                    trap.isActive = !trap.isActive;  // ON/OFF ���]
                    trap.ToggleTrap();              // ���ۂ̓���E�\�����X�V
                }
            }
        }
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