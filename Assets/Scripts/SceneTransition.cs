using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public string sceneName = "";
    public float transitionTime = 1.0f;
    public bool locked = false;//�����|�����Ă��邩�ǂ���?
    public Animator animator;
    public Animator padlockAnimator;//�싞���A�j���[�V����
    public static SceneTransition instance;

    //private bool SceneFlag = false;
    private void Start()
    {
        //�ǂݍ��ރV�[�����ݒ肳��ĂȂ��ꍇ�A���݂̃V�[����ݒ�
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;

        //���b�N���|�����Ă��Ȃ��ꍇ�싞�����\��
        if (!locked && transform.childCount > 0) transform.GetChild(0).gameObject.SetActive(false);
        if (instance == null||!instance.locked)
        {
            instance = this;
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (locked) return;//�����|�����Ă���ꍇ�������I��

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript.instance.cannotMoveMode();//�v���C���[�������Ȃ��悤��
            AudioManager.instance.PlaySE2("�S�[��������");

            PlayerScript.instance.gameObject.SetActive(false);
            animator.SetTrigger("GoalTrigger");

        }
    }


    public void Transition()
    {
        IrisShot.instance.IrisOut();
        TimerManager.instance.StopTimer();

        //�ǂݍ��ރV�[�����ݒ肳��ĂȂ��ꍇ�A���݂̃V�[����ݒ�
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;
        
        //n�b��ɃV�[���J��
        DOVirtual.DelayedCall(transitionTime, () =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void OnOffLocked(bool lockMode)
    {
        locked = false;
    }

    public void StartAnimation()
    {
        padlockAnimator.SetTrigger("Cancellation");
    }



}
