using UnityEngine;

public class TrapController : MonoBehaviour
{
    private Animator anim;
    public bool isActive = false;//ON���ǂ���
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    public void ToggleTrap()
    {
       isActive = !isActive;
        //������Ԕ��f

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
        if (!isActive) return;//�I�t�̎��͓�����Ȃ�

        if (collision.CompareTag("Player"))
        {
            if(GameManager.instance != null)
            {
                Debug.Log("���ɓ�������");
                //GameManager�ɒʒm
                //GameManager.instance.
            }

        }
    }*/
}
