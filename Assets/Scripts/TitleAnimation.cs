using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static System.TimeZoneInfo;

public class TitleAnimation : MonoBehaviour
{
    
    public Animator animator;
    public Animator playerAnimator;
    public GameObject IrisShotObject;

    private bool on = false;
    private bool flag = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayBGM("BGM2");
    }

    // Update is called once per frame
    void Update()
    {

        //AudioManager.instance.PlayBGM("BGM1");
        if (Input.GetButtonDown("Title")||Input.anyKeyDown)
        {
            if (flag) return;
            flag = true;
            IrisShotObject.SetActive(true);
            IrisShot.instance.IrisOut();
            Invoke("Transition", 0.7f);
            if (on)OnPlayer();
            else OffPlayer();
        }

    }

    public void On()
    {
        on = true;
    }

    public void Off()
    {
        on = false;
    }

    public void OnPlayer()
    {
        playerAnimator.SetTrigger("OnTrigger");
    }

    public void OffPlayer()
    {
        playerAnimator.SetTrigger("OffTrigger");
    }

    public void Transition()
    {
        //SceneTransition.instance.Transition();

        //if (IrisShot.instance != null) IrisShot.instance.IrisIN();

        //n•bŒã‚ÉƒV[ƒ“‘JˆÚ
        DOVirtual.DelayedCall(0.5f, () =>
        {
            SceneManager.LoadScene("SSScene");
        });
    }

}
