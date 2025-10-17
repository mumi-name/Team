using UnityEngine;

public class TrapController : MonoBehaviour
{
    private Animator anim;
    public bool isActive = false;//ON‚©‚Ç‚¤‚©
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void ToggleTrap()
    {
       isActive = !isActive;
        //‰Šúó‘Ô”½‰f

    }
    private void UpDateTrapAnimation()
    {
        if (anim == null) return;

        if (isActive)
        {
            anim.SetBool("on_toge", isActive);
        }
    }
    void Update()
    {
        
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;//ƒIƒt‚Ì‚Í“–‚½‚ç‚È‚¢

        if (collision.CompareTag("Player"))
        {
            if(GameManager.instance != null)
            {
                Debug.Log("™‚É“–‚½‚Á‚½");
                //GameManager‚É’Ê’m
                //GameManager.instance.
            }

        }
    }*/
}
