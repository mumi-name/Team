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
            IrisShot.instance.IrisIN();
            TimerManager.instance.StopTimer();
            //三秒後にシーン遷移
            DOVirtual.DelayedCall(2.0f, () =>
            {
                SceneManager.LoadScene(sceneName);
            });
            //IrisShot.instance.IrisOut();
            /*IrisShot.instance.IrisIN();
            SceneFlag = true;
            Debug.Log("Flagが真になりました！");
            if (IrisShot.instance != null)
            {
                //IrisShot.instance.IrisIN(() => {
                //    SceneManager.LoadScene(sceneName);
                //});

            }
            else
            {
                Debug.Log("Flagが null　です。");
            }
            */


        }
    }
}
