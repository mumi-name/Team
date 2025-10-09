using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public string sceneName = "";
    //private bool SceneFlag = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {

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
            //IrisShot.instance.IrisOut();
            /*IrisShot.instance.IrisIN();
            SceneFlag = true;
            Debug.Log("Flag���^�ɂȂ�܂����I");
            if (IrisShot.instance != null)
            {
                //IrisShot.instance.IrisIN(() => {
                //    SceneManager.LoadScene(sceneName);
                //});

            }
            else
            {
                Debug.Log("Flag�� null�@�ł��B");
            }
            */


        }
    }
    
}
