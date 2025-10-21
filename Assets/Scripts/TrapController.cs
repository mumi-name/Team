using UnityEngine;

public class TrapController : MonoBehaviour
{
    public enum TrapDirection { Up, Down, Left, Right }
    public TrapDirection trapDirection;

    private Animator anim;
    private Collider2D trapCollider;
    public bool isActive = false;
    public bool locked = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        trapCollider = GetComponent<Collider2D>();

        if (anim != null)
        {
            anim.SetBool("on_toge", isActive);
        }
    }

    public void ToggleTrap()
    {
        if (locked) return;
        isActive = !isActive;
        UpdateCollider();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySE("ONOFF");
        }
    }
    // スイッチで反転したときに呼ぶ
    public void ToggleTrapAndLock()
    {
        isActive = !isActive;
        locked = true; // この状態で固定
        UpdateCollider();
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySE("ONOFF");
    }

    private void UpdateCollider()
    {
        if(trapCollider != null)trapCollider.enabled = isActive;
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

    //public void UpdateVisual()
    //{
    //    if (spr != null) spr.enabled = isActive;
    //    if (col != null) col.enabled = isActive;
    //}

    public void SetActive(bool on)
    {
        isActive = on;
        UpdateCollider();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"プレイヤーが {trapDirection} のトラップに当たった！");
        }
    }

}