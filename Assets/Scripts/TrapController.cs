using UnityEngine;

public class TrapController : MonoBehaviour
{
    public enum TrapDirection { Up, Down, Left, Right }
    public TrapDirection trapDirection;

    private Animator anim;
    private Collider2D trapCollider;
    public bool isActive = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        trapCollider = GetComponent<Collider2D>();

        if (anim != null)
        {
            anim.SetBool("on_toge", isActive);
        }

        if(trapCollider != null)
        {
            trapCollider.enabled = isActive;
        }
    }

    public void ToggleTrap()
    {
        isActive = !isActive;
        UpdateTrapAnimation();

        //当たり判定をON/OFF
        if(trapCollider != null)
        {
            trapCollider.enabled = isActive;
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySE("ONOFF");
        }
    }

    private void UpdateTrapAnimation()
    {
        if (anim == null) return;

        //anim.SetBool("on_toge", isActive);

        switch (trapDirection)
        {
            case TrapDirection.Up:
                anim.SetBool("on_upper", isActive);
                break;
            case TrapDirection.Down:
                anim.SetBool("on_under", isActive);
                break;
            case TrapDirection.Left:
                anim.SetBool("on_left", isActive);
                break;
            case TrapDirection.Right:
                anim.SetBool("on_right", isActive);
                break;
        }

    }
    /*
    public void SetTrap(bool on)
    {
        isActive = on;
        if(anim != null)
        {
            anim.SetBool("on_toge", isActive);
        }
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlaySE2("ONOFF");
        }
    }
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"プレイヤーが {trapDirection} のトラップに当たった！");
        }
    }

}