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
            //Debug.Log("�n��");
            Vector2 vec = (collision.transform.position - this.transform.position);



            /*if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                Debug.Log("�W�����v�������ʂɐڐG��"+Time.time);

            }*/

            /*if (vec.x > 0 && Mathf.Abs(vec.y) > 0)
            {
                Debug.Log("����?");
            }
            else if (vec.x < 0 && Mathf.Abs(vec.y) > 0)
            {
                Debug.Log("�E��?");
            }*/

            /*if (vec.y > Mathf.Abs(vec.x))
            {
                Debug.Log("����͏�ɂ��� �� ������������Ԃ������i������ɂƂ��Ă͉��ʁj");
            }
            else if (-vec.y > Mathf.Abs(vec.x))
            {
                Debug.Log("����͉��ɂ��� �� �������ォ��Ԃ������i������ɂƂ��Ă͏�ʁj");
            }
            else if (vec.x > 0)
            {
                Debug.Log("����͉E�ɂ��� �� ������������Ԃ������i������ɂƂ��Ă͍����ʁj");
            }
            else
            {
                Debug.Log("����͍��ɂ��� �� �������E����Ԃ������i������ɂƂ��Ă͉E���ʁj"); 
        
            }*/

            TakedaPlayer.instance.OnOffJumpFlag(false); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("��");
            TakedaPlayer.instance.OnOffJumpFlag(true);
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
