using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class CheckJump : MonoBehaviour
{
    public PlayerScript playerScript;
    public LayerMask elevatorLayer;  //エレベーターレイヤー
    public LayerMask floorLayer;
    private bool wasGrounded = false;

    private float underSize = 0.1f;//rayを飛ばす長さ
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 underEnd = transform.position - new Vector3(0, 3, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        if (underHit)
        {
            //親子関係を切る
            //if (PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
            //PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        }
        //Debug.Log("判定が取れたよ");

        //Debug.DrawLine(transform.position, underEnd, Color.blue);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {

            if (brock.move)
            {
                //親子関係を切る
                //if (PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
                //PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
            }

        }
    }

    private void Update()
    {

        //<段差に突っかからないプログラム>---------------------------------------------------------

        //プレイヤーの足元からRayを飛ばして横の判定をチェック
        /*float weight= (PlayerScript.instance.GetMode() / PlayerScript.instance.GetMode()) - 0.3f;
        if (PlayerScript.instance.GetMode() < 0) weight *= -1;
        Vector2 besideEnd = transform.position + new Vector3(weight, 0, 0);
        RaycastHit2D besideHit = Physics2D.Linecast(transform.position,besideEnd, elevatorLayer);

        //エレベーターにヒットしたら
        if (besideHit)
        {
            //力を少し加えて、段差を登る補助をする
            if (Mathf.Abs(PlayerScript.instance.GetNum())< 1) return;
            //if (Mathf.Abs(PlayerScript.instance.rb.linearVelocityX) < 1.5) return;
            PlayerScript.instance.rb.AddForceY(10); 
            PlayerScript.instance.rb.AddForceX(10);
            Debug.Log("少しの段差なら登れるようにしています");
        }
        Debug.DrawLine(transform.position, besideEnd, Color.green);*/

        //----------------------------------------------------------------------------------------

        //ジャンプ中かそうでないかで飛ばすrayの長さを変える(ピョンピョンバグ&切り替えの不便さ)
        if (PlayerScript.instance.GetJumpFlag()) underSize = 0.4f;
        else underSize = 0.1f;

        //プレイヤーより下の判定をチェック
        Vector2 underEnd = transform.position - new Vector3(0, underSize, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, floorLayer);
        RaycastHit2D underHit2 = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        //何にも触れてない時
        if (!underHit && !underHit2)
        {
            //PlayerScript.instance.OnOffJumpFlag(true);
            wasGrounded = false;
        }
        
        //エレベーターに接触したら
        if (underHit2)
        {
            //親子関係を切る
            if (PlayerScript.instance.gameObject.transform.parent != null)PlayerScript.instance.transform.SetParent(null);
            PlayerScript.instance.transform.SetParent(underHit2.transform, worldPositionStays: true);
        }
        else
        {
            if (PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
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

        Vector2 underEnd = transform.position - new Vector3(0, 1f, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        //if (underHit) PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        //Debug.Log("判定が取れたよ");

        //Debug.DrawLine(transform.position, underEnd, Color.blue);


    }


    //↓下のコードを消すと、テレポートバグが出ます。注意されたし--------------------------------------------------
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //PlayerScript.instance.OnOffJumpFlag(true);
            //wasGrounded = false;

            if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
            {
                //親子関係を切る
                if (/*brock.move &&*/ PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //離れたのが動く床だった場合は親子関係を切る
            if (/*brock.move&&*/PlayerScript.instance.gameObject.transform.parent!=null) PlayerScript.instance.transform.SetParent(null);
        }
    }

    //-------------------------------------------------------------------------------------------------------------
   


}