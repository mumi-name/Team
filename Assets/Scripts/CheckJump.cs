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
            Debug.Log("地面");
            PlayerScript.instance.OnOffJumpFlag(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("空中");
            PlayerScript.instance.OnOffJumpFlag(true);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        Debug.Log("地面に移動");
    //        PlayerScript.instance.OffJumpFlag();
    //    }
    //    else if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.Log("それ壁");
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        Debug.Log("空中に移動");
    //        PlayerScript.instance.OnJumpFlag();
    //    }
    //    else if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        Debug.Log("壁から離脱");
    //    }
    //}
}
