using Unity.VisualScripting;
using UnityEngine;

public class CheckJump : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
      
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("�n��");
            PlayerScript.instance.OnOffJumpFlag(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("��");
            PlayerScript.instance.OnOffJumpFlag(true);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        Debug.Log("�n�ʂɈړ�");
    //        PlayerScript.instance.OffJumpFlag();
    //    }
    //    else if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.Log("�����");
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        Debug.Log("�󒆂Ɉړ�");
    //        PlayerScript.instance.OnJumpFlag();
    //    }
    //    else if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.Log("�ǂ��痣�E");
    //    }
    //}
}
