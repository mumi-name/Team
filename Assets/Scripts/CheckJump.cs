using Unity.VisualScripting;
using UnityEngine;

public class CheckJump : MonoBehaviour
{
    public PlayerScript playerScript;
    public LayerMask elevatorLayer;  //�G���x�[�^�[���C���[
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
            Debug.Log("���肪���Ă��Ȃ�");
            return;
        }
        PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        Debug.Log("���肪��ꂽ��");

        Debug.DrawLine(transform.position, underEnd, Color.blue);

        /*if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //�v���C���[���G���x�[�^�[�̏�ɂ��Ȃ���ΐe�q�֌W�����Ȃ�
            //Vector2 vec = (collision.transform.position - PlayerScript.instance.transform.position);
            //if (vec.y-0.9f < 0) return;
            //Debug.Log("vec.y�̒l��=" + vec.y);  PlayerScript.instance.GetMode()*-2.0f

            Vector2 underEnd = transform.position - new Vector3(0, 2, 0);
            RaycastHit2D underHit = Physics2D.Linecast(transform.position,underEnd, elevatorLayer);

            if (underHit == false)
            {
                Debug.Log("���肪���Ă��Ȃ�");
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
        
        //�v���C���[�����̔��肪�G�ꂽ�̂��A���ł���
        if (collision.gameObject.CompareTag("Floor"))
        {
            
            //�����v���C���[�́h���h�ɏ�������ꍇ�A�W�����v�\�ɂ���
            Vector2 vec = (collision.transform.position - this.transform.position);
            if (vec.y < 0) PlayerScript.instance.OnOffJumpFlag(false);
            
            if (!wasGrounded)
            {
                AudioManager.instance.PlaySE("�W�����v�̒��n");
                wasGrounded = true;
            }
        }

        Vector2 underEnd = transform.position - new Vector3(0, 3, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        if (underHit == false)
        {
            Debug.Log("���肪���Ă��Ȃ�");
            return;
        }
        PlayerScript.instance.transform.SetParent(collision.transform, worldPositionStays: true);
        Debug.Log("���肪��ꂽ��");

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
                //�e�q�֌W��؂�
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