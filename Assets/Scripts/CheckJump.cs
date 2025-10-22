using Unity.VisualScripting;
using UnityEngine;

public class CheckJump : MonoBehaviour
{
    public PlayerScript playerScript;
    public LayerMask elevatorLayer;  //エレベーターレイヤー
    public LayerMask floorLayer;
    private bool wasGrounded = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 underEnd = transform.position - new Vector3(0, 3, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        
        if(underHit)PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        //Debug.Log("判定が取れたよ");

        Debug.DrawLine(transform.position, underEnd, Color.blue);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            
            if (brock.move) PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);

        }
    }

    private void Update()
    {

        //プレイヤーの横の判定をチェック
        /*float weight= (PlayerScript.instance.GetMode() / PlayerScript.instance.GetMode()) - 0.3f;
        if (PlayerScript.instance.GetMode() < 0) weight *= -1;
        Vector2 besideEnd = transform.position + new Vector3(weight, 0, 0);
        RaycastHit2D besideHit = Physics2D.Linecast(transform.position,besideEnd, elevatorLayer);

        if (besideHit)
        {
            if (Mathf.Abs(PlayerScript.instance.GetNum())< 1) return;
            //if (Mathf.Abs(PlayerScript.instance.rb.linearVelocityX) < 1.5) return;
            PlayerScript.instance.rb.AddForceY(10); 
            PlayerScript.instance.rb.AddForceX(10);
            Debug.Log("少しの段差なら登れるようにしています");
        }
        Debug.DrawLine(transform.position, besideEnd, Color.green);*/

        //プレイヤーより下の判定をチェック
        Vector2 underEnd = transform.position - new Vector3(0, 1, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, floorLayer);
        RaycastHit2D underHit2= Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        if (!underHit&&!underHit2)
        {
            PlayerScript.instance.OnOffJumpFlag(true);
            wasGrounded = false;
        }
        //Debug.Log("判定が取れたよ");

        Debug.DrawLine(transform.position, underEnd, Color.red);
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

        if(underHit)PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        //Debug.Log("判定が取れたよ");

        Debug.DrawLine(transform.position, underEnd, Color.blue);


    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //PlayerScript.instance.OnOffJumpFlag(true);
            //wasGrounded = false;

            if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
            {
                //親子関係を切る
                if (brock.move && PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //離れたのが動く床だった場合は親子関係を切る
            if (brock.move&&PlayerScript.instance.gameObject.transform.parent!=null) PlayerScript.instance.transform.SetParent(null);
        }
    }
   


}