using Unity.VisualScripting;
using UnityEngine;

public class CheckJump : MonoBehaviour
{
    public PlayerScript playerScript;
    void Start()
    {
        //if(playerScript == null)playerScript=transform.parent.GetComponent<PlayerScript>();
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript.instance.OnOffJumpFlag(false);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
      
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("’n–Ê");
            Vector2 vec = (collision.transform.position - this.transform.position);

           

            PlayerScript.instance.OnOffJumpFlag(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("‹ó’†");
            PlayerScript.instance.OnOffJumpFlag(true);
        }
    }

   
}
