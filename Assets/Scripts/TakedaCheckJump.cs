using UnityEngine;

public class TakedaCheckJump : MonoBehaviour
{
    
    void Start()
    {
        //if(playerScript == null)playerScript=transform.parent.GetComponent<PlayerScript>();
    }

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("地面");
            Vector2 vec = (collision.transform.position - this.transform.position);



            /*if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                Debug.Log("ジャンプ中且つ側面に接触中"+Time.time);

            }*/

            /*if (vec.x > 0 && Mathf.Abs(vec.y) > 0)
            {
                Debug.Log("左面?");
            }
            else if (vec.x < 0 && Mathf.Abs(vec.y) > 0)
            {
                Debug.Log("右面?");
            }*/

            /*if (vec.y > Mathf.Abs(vec.x))
            {
                Debug.Log("相手は上にいる → 自分が下からぶつかった（＝相手にとっては下面）");
            }
            else if (-vec.y > Mathf.Abs(vec.x))
            {
                Debug.Log("相手は下にいる → 自分が上からぶつかった（＝相手にとっては上面）");
            }
            else if (vec.x > 0)
            {
                Debug.Log("相手は右にいる → 自分が左からぶつかった（＝相手にとっては左側面）");
            }
            else
            {
                Debug.Log("相手は左にいる → 自分が右からぶつかった（＝相手にとっては右側面）"); 
        
            }*/

            TakedaPlayer.instance.OnOffJumpFlag(false); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("空中");
            TakedaPlayer.instance.OnOffJumpFlag(true);
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
