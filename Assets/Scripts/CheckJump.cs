using Unity.VisualScripting;
using UnityEngine;

public class CheckJump : MonoBehaviour
{
    public PlayerScript playerScript;
    public LayerMask elevatorLayer;  //エレベーターレイヤー
    private bool wasGrounded = false;
    void Start()
    {
        //if(playerScript == null)playerScript=transform.parent.GetComponent<PlayerScript>();
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 underEnd = transform.position - new Vector3(0, 3, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        if (underHit == false)
        {
            Debug.Log("判定が取れていない");
            return;
        }
        PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        Debug.Log("判定が取れたよ");

        Debug.DrawLine(transform.position, underEnd, Color.blue);

        /*if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //プレイヤーがエレベーターの上にいなければ親子関係を作らない
            //Vector2 vec = (collision.transform.position - PlayerScript.instance.transform.position);
            //if (vec.y-0.9f < 0) return;
            //Debug.Log("vec.yの値は=" + vec.y);  PlayerScript.instance.GetMode()*-2.0f

            Vector2 underEnd = transform.position - new Vector3(0, 2, 0);
            RaycastHit2D underHit = Physics2D.Linecast(transform.position,underEnd, elevatorLayer);

            if (underHit == false)
            {
                Debug.Log("判定が取れていない");
                return;
            }
            if (brock.move) PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);

        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            
            if (brock.move) PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        //プレイヤー足元の判定が触れたのが、床であり
        if (collision.gameObject.CompareTag("Floor"))
        {
            
            //尚且つプレイヤーの”下”に床がある場合、ジャンプ可能にする
            Vector2 vec = (collision.transform.position - this.transform.position);
            if (vec.y < 0) PlayerScript.instance.OnOffJumpFlag(false);
            
            if (!wasGrounded)
            {
                AudioManager.instance.PlaySE("ジャンプの着地");
                wasGrounded = true;
            }
        }

        Vector2 underEnd = transform.position - new Vector3(0, 3, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        if (underHit == false)
        {
            Debug.Log("判定が取れていない");
            return;
        }
        PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        Debug.Log("判定が取れたよ");

        Debug.DrawLine(transform.position, underEnd, Color.blue);



    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            PlayerScript.instance.OnOffJumpFlag(true);
            wasGrounded = false;

            if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
            {
                //親子関係を切る
                if (brock.move) PlayerScript.instance.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //離れたのが動く床だった場合は親子関係を切る
            if (brock.move) PlayerScript.instance.transform.SetParent(null);
        }
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
      
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("地面");
            Vector2 vec = (collision.transform.position - this.transform.position);
            if(vec.y<0)PlayerScript.instance.OnOffJumpFlag(false);
            else PlayerScript.instance.OnOffJumpFlag(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("空中");
            PlayerScript.instance.OnOffJumpFlag(true);
        }
    }*/


}