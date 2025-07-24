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
            Debug.Log("’n–Ê");
            PlayerScript.instance.OnOffJumpFlag(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("‹ó’†");
            PlayerScript.instance.OnOffJumpFlag(true);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        Debug.Log("’n–Ê‚ÉˆÚ“®");
    //        PlayerScript.instance.OffJumpFlag();
    //    }
    //    else if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.Log("‚»‚ê•Ç");
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        Debug.Log("‹ó’†‚ÉˆÚ“®");
    //        PlayerScript.instance.OnJumpFlag();
    //    }
    //    else if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.Log("•Ç‚©‚ç—£’E");
    //    }
    //}
}
