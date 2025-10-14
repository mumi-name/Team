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
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            if (brock.move) PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);

        }
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
            //AudioManager.instance.PlaySE("ジャンプの着地");
            //尚且つプレイヤーの”下”に床がある場合、ジャンプ可能にする
            Vector2 vec = (collision.transform.position - this.transform.position);
            if (vec.y < 0) PlayerScript.instance.OnOffJumpFlag(false);

        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            PlayerScript.instance.OnOffJumpFlag(true);

            if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
            {
                //離れたのが動く床だった場合は親子関係を切る
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