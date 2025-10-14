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
        //�v���C���[�����̔��肪�G�ꂽ�̂��A���ł���
        if (collision.gameObject.CompareTag("Floor"))
        {
            //AudioManager.instance.PlaySE("�W�����v�̒��n");
            //�����v���C���[�́h���h�ɏ�������ꍇ�A�W�����v�\�ɂ���
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
                //���ꂽ�̂��������������ꍇ�͐e�q�֌W��؂�
                if (brock.move) PlayerScript.instance.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //���ꂽ�̂��������������ꍇ�͐e�q�֌W��؂�
            if (brock.move) PlayerScript.instance.transform.SetParent(null);
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
      
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("�n��");
            Vector2 vec = (collision.transform.position - this.transform.position);
            if(vec.y<0)PlayerScript.instance.OnOffJumpFlag(false);
            else PlayerScript.instance.OnOffJumpFlag(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //Debug.Log("��");
            PlayerScript.instance.OnOffJumpFlag(true);
        }
    }*/


}