using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    public string sceneName = "";
    public float transitionTime = 1.0f;
    public bool locked = false;//鍵が掛かっているかどうか?

    public Animator animator;
    public Animator padlockAnimator;//南京錠アニメーション

    public int groupNumber = 1;//1グループ目なら1
    public int stageNumber = 1;//グループ内の1ステージ目なら1

    //private bool SceneFlag = false;
    private void Start()
    {
        //読み込むシーンが設定されてない場合、現在のシーンを設定
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;

        //ロックが掛かっていない場合南京錠を非表示
        if (!locked && transform.childCount > 0) transform.GetChild(0).gameObject.SetActive(false);
        if (instance == null||!instance.locked)
        {
            instance = this;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (locked) return;//鍵が掛かっている場合処理を終了

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ゴール時点の時間 = " + TimerManager.instance.elapsedTime);
            GameManager.instance.GoalFlag();
            TimerManager.instance.SaveStageTime(groupNumber -1,stageNumber - 1);
            //TimerManager.instance.SaveStageTimeAndUpdateTotal(groupNumber - 1, stageNumber - 1);
            TimerManager.instance.StopTimer();
            PlayerScript.instance.cannotMoveMode();//プレイヤーが動かないように
            AudioManager.instance.PlaySE2("ゴール時音声");

            PlayerScript.instance.gameObject.SetActive(false);
            animator.speed = 1;
            animator.SetTrigger("GoalTrigger");

        }
    }


    public void Transition()
    {
        if(IrisShot.instance!=null)IrisShot.instance.IrisOut();
        if(IrisShot.instance!=null)TimerManager.instance.StopTimer();

        //読み込むシーンが設定されてない場合、現在のシーンを設定
        if (sceneName == "") sceneName = SceneManager.GetActiveScene().name;

        //TimerManager.instance.SaveStageTime(groupNumber - 1, stageNumber - 1);

        //n秒後にシーン遷移
        DOVirtual.DelayedCall(transitionTime, () =>
        {
            SceneManager.LoadScene(sceneName);
        });

        Destroy(PlayerScript.instance.gameObject,transitionTime);
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
