using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public string sceneName = "";
    public bool locked = false;//�����|�����Ă��邩�ǂ���?

    public static SceneTransition instance;

    //private bool SceneFlag = false;
    private void Start()
    {
        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (locked) return;//�����|�����Ă���ꍇ�������I��

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript.instance.cannotMoveMode();//�v���C���[�������Ȃ��悤��
            IrisShot.instance.IrisOut();
            TimerManager.instance.StopTimer();
            //�O�b��ɃV�[���J��
            DOVirtual.DelayedCall(2.0f, () =>
            {
                TimerManager.instance.StartTimer();
                SceneManager.LoadScene(sceneName);
            });
            
        }
    }

    public void OnOffLocked(bool lockMode)
    {
        locked = false;
    }

    
    
}
