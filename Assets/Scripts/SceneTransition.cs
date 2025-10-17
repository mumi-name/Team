using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public string sceneName = "";
    public float transitionTime = 1.0f;
    public bool locked = false;//鍵が掛かっているかどうか?

    public static SceneTransition instance;

    //private bool SceneFlag = false;
    private void Start()
    {
        //読み込むシーンが設定されてない場合、現在のシーンを設定
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;

        //ロックが掛かっていない場合南京錠を非表示
        if (!locked && transform.childCount > 0) transform.GetChild(0).gameObject.SetActive(false);
        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (locked) return;//鍵が掛かっている場合処理を終了

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript.instance.cannotMoveMode();//プレイヤーが動かないように
            AudioManager.instance.PlaySE2("ゴール時音声");
            IrisShot.instance.IrisOut();
            TimerManager.instance.StopTimer();
            //三秒後にシーン遷移
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

        //読み込むシーンが設定されてない場合、現在のシーンを設定
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("シーンをロードしちゃうぜ");

        //n秒後にシーン遷移
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
