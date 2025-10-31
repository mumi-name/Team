using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class CheckJump : MonoBehaviour
{
    public PlayerScript playerScript;
    public LayerMask elevatorLayer;  //�G���x�[�^�[���C���[
    public LayerMask floorLayer;

    private float underSize = 0.1f;//ray���΂�����
    private bool wasGrounded = false;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���ɐG�ꂽ��
        if (collision.gameObject.CompareTag("Floor"))
        {
            

            

        }
    }


    private void Update()
    {
        //�v���C���[�̑�������Ray���΂��ĉ��̔�����`�F�b�N
        //float weight = (PlayerScript.instance.GetMode() / PlayerScript.instance.GetMode()) - 0.3f;
        //if (PlayerScript.instance.GetMode() < 0) weight = -1;
        //Vector2 besideEnd = transform.position + new Vector3(weight, 0, 0);
        //RaycastHit2D besideHit = Physics2D.Linecast(transform.position, besideEnd, floorLayer);

        ////�G���x�[�^�[�Ƀq�b�g������
        //if (besideHit)
        //{
        //    //�͂����������āA�i����o��⏕������
        //    if (Mathf.Abs(PlayerScript.instance.GetNum()) < 1) return;
        //    //if (Mathf.Abs(PlayerScript.instance.rb.linearVelocityX) < 1.5) return;
        //    PlayerScript.instance.rb.AddForceY(10);
        //    PlayerScript.instance.rb.AddForceX(10);
        //    Debug.Log("�����̒i���Ȃ�o���悤�ɂ��Ă��܂�");
        //}
        //Debug.DrawLine(transform.position, besideEnd, Color.green); 

        //----------------------------------------------------------------------------------------


        



        //<�v���C���[�̑�������>-------------------------------------------------------------------------------------

        //�W�����v���������łȂ����Ŕ�΂�ray�̒�����ς���(�s�����s�����o�O&�؂�ւ��̕s�ւ�)
        if (PlayerScript.instance.GetJumpFlag()) underSize = 0.4f;
        else underSize = 0.1f;

        //�v���C���[��艺�̔�����`�F�b�N
        Vector2 underEnd = transform.position - new Vector3(0, underSize, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, floorLayer);
        RaycastHit2D underHit2 = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

        //���ɂ��G��ĂȂ���
        if (!underHit && !underHit2)
        {
            PlayerScript.instance.OnOffJumpFlag(true);//���̃R�[�h�������ƃJ�`���J�`���ŋ󒆂ɂ����Ƃ���o�O���N����
            wasGrounded = false;
        }
        else PlayerScript.instance.OnOffJumpFlag(false);//���̃R�[�h�������ƁA�؂�ւ��̎�G�肪�����Ȃ��

        //������X�e�[�W2���C���������Č���ꂽ��else if(!underHit)�ɂ��܂��傤�B��������G�芴�͌���

        //�G���x�[�^�[�ɐڐG������
        if (underHit2)
        {
            //�e�q�֌W��؂�
            if (PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
            PlayerScript.instance.transform.SetParent(underHit2.transform, worldPositionStays: true);
        }
        else
        {
            if (PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
        }
        //Debug.Log("���肪��ꂽ��");
        Debug.DrawLine(transform.position, underEnd, Color.red);

        //-------------------------------------------------------------------------------------------------------------
        //�v���C���[�̏�����ɏ������邩�ǂ����𒲂ׂ�
        Vector3 pPos = PlayerScript.instance.gameObject.transform.position+new Vector3(0,1.08f,0);
        Vector3 aboveEnd = pPos + new Vector3(0, 0.1f, 0);
        RaycastHit2D aboveHit = Physics2D.Linecast(pPos, aboveEnd, floorLayer);

        //���ƃG���x�[�^�[�ɋ��܂�Ĉ������Ȃ��悤��
        if (aboveHit && underHit2)
        {
            //�G���x�[�^�[���~
            GameManager.instance.SetStopFloor(true);
        }
        else
        {
            GameManager.instance.SetStopFloor(false);
        }


            Debug.DrawLine(pPos, aboveEnd, Color.blue);


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

        Vector2 underEnd = transform.position - new Vector3(0, 1f, 0);
        RaycastHit2D underHit = Physics2D.Linecast(transform.position, underEnd, elevatorLayer);

    }


    //�����̃R�[�h�������ƁA�e���|�[�g�o�O���o�܂��B���ӂ��ꂽ��--------------------------------------------------
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            //PlayerScript.instance.OnOffJumpFlag(true);
            //wasGrounded = false;

            if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
            {
                //�e�q�֌W��؂�
                if (/*brock.move &&*/ PlayerScript.instance.gameObject.transform.parent != null) PlayerScript.instance.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<OnOffBrock>(out var brock))
        {
            //���ꂽ�̂��������������ꍇ�͐e�q�֌W��؂�
            if (/*brock.move&&*/PlayerScript.instance.gameObject.transform.parent!=null) PlayerScript.instance.transform.SetParent(null);
        }
    }

    //-------------------------------------------------------------------------------------------------------------
   


}