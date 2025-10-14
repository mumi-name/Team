using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class SceneTransition : MonoBehaviour
{
    public string sceneName = "";
    public bool locked = false;//Œ®‚ªŠ|‚©‚Á‚Ä‚¢‚é‚©‚Ç‚¤‚©?

    public static SceneTransition instance;

    //private bool SceneFlag = false;
    private void Start()
    {
        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (locked) return;//Œ®‚ªŠ|‚©‚Á‚Ä‚¢‚éê‡ˆ—‚ðI—¹

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript.instance.cannotMoveMode();//ƒvƒŒƒCƒ„[‚ª“®‚©‚È‚¢‚æ‚¤‚É
            IrisShot.instance.IrisOut();
            TimerManager.instance.StopTimer();
            //ŽO•bŒã‚ÉƒV[ƒ“‘JˆÚ
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
