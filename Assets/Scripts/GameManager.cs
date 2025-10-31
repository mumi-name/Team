using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float slowSpeed = 1f;
    public float finalTime;//�^�C�}�[�}�l�[�W���[�p�ϐ�
    public bool douziOsi = false;//�o�O�΍�(�����������֎~)
    public List<OnOffBrock> brocks;//�X�e�[�W���ɂ���ONOFF�u���b�N
    public List<TrapController> traps;//�����X�g
    
    public Sprite onSprite;
    public Sprite offSprite;

    private float targetScale = 1;
    private bool waveAnimation = false;//�g���A�j���[�V�����̍Œ����ǂ���?
    private bool resetFlag = true;
    private bool goalFlag = false;

    void Awake()
    {
        instance = this;
    }

    public void ResetFlag(bool flag=false)
    {
        resetFlag = flag;
    }

    public void GoalFlag()
    {
        goalFlag = true;
    }

    void Start()
    {
        TimerManager.instance.StartTimer();
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
        //ON();
        OnOff(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!resetFlag&&!goalFlag)
        {
            if (Input.GetButtonDown("Reset"))
            {
                if (PlayerScript.instance != null && PlayerScript.instance.isDead) return;
                resetFlag = true;
                StartCoroutine(ResetSceneWithSE());
            }
        }
        changeSlow(targetScale);
    }

    public void OnOff(bool OnOff)
    {
        if (PlayerScript.instance.GetIngoreInput()) return;
        foreach (var brock in brocks)
        {
            brock.OnOff(OnOff);
        }
        foreach(var trap in traps)
        {
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

    public void ChangeOnOff(bool on)
    {
        foreach (var brock in brocks)
        {
            //brock.on = !brock.on;
            brock.on = !brock.on;
        }
        //if (on) ON();
        //else OFF();

        OnOff(on);

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

    public void SetStopFloor(bool flag)
    {
        foreach (var brock in brocks)
        {
            if (brock.move) brock.setStopFlag(flag);
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

