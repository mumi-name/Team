using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public string sceneName = "";
    public float transitionTime = 1.0f;
    public bool locked = false;//�����|�����Ă��邩�ǂ���?

    public static SceneTransition instance;

    //private bool SceneFlag = false;
    private void Start()
    {
        //�ǂݍ��ރV�[�����ݒ肳��ĂȂ��ꍇ�A���݂̃V�[����ݒ�
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;

        //���b�N���|�����Ă��Ȃ��ꍇ�싞�����\��
        if (!locked && transform.childCount > 0) transform.GetChild(0).gameObject.SetActive(false);
        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (locked) return;//�����|�����Ă���ꍇ�������I��

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript.instance.cannotMoveMode();//�v���C���[�������Ȃ��悤��
            AudioManager.instance.PlaySE2("�S�[��������");
            IrisShot.instance.IrisOut();
            TimerManager.instance.StopTimer();
            //�O�b��ɃV�[���J��
            DOVirtual.DelayedCall(transitionTime, () =>
            {
                TimerManager.instance.StartTimer();
                SceneManager.LoadScene(sceneName);
            });

        }
    }

    public void Transition()
    {
        IrisShot.instance.IrisOut();
        TimerManager.instance.StopTimer();

        //�ǂݍ��ރV�[�����ݒ肳��ĂȂ��ꍇ�A���݂̃V�[����ݒ�
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("�V�[�������[�h�����Ⴄ��");

        //n�b��ɃV�[���J��
        DOVirtual.DelayedCall(transitionTime, () =>
        {
            //TimerManager.instance.StartTimer();
            SceneManager.LoadScene(sceneName);
        });
    }

    public void OnOffLocked(bool lockMode)
    {
        locked = false;
    }



}
