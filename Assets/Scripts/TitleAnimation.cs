using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleAnimation : MonoBehaviour
{
    
    public Animator animator;
    public Animator playerAnimator;

    private bool on = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayBGM("BGM2");
    }

    // Update is called once per frame
    void Update()
    {

        //AudioManager.instance.PlayBGM("BGM1");
        if (Input.GetButtonDown("Title"))
        {
            if (on)
            {
                animator.SetTrigger("ButtonTrigger");
                OnPlayer();
            }
            else
            {
                animator.SetTrigger("OffButtonTrigger");
                OffPlayer();
            }
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

}
