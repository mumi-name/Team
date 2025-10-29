using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public GameObject wavePrefab;//�{�^����������ۂɏo��g��
    public Animator animator;//�A�j���[�^�[

    //public TrapController[] targetTraps;
    bool on = false;//�X�C�b�`���N��������?
    void Start()
    {

    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (on) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            //�X���[���[�h�ɂ���
            PlayerScript.instance.ignoreMove(true);//���������d�l�𖳂������߂ɕ�����������(10/29)��ƐR����
            animator.SetTrigger("SwitchTrigger");
            on = true;
            //ToggleTrap();
            GameManager.instance.OnOffSlow(true);
            //AudioManager.instance.PlaySE2("�X�C�b�`�ƏՌ��g");
            AudioManager.instance.PlaySE2("�X�C�b�`(�Ռ��g)");
            //AusioManager.instance.PlaySEPartialOneShot("�X�C�b�`�ƏՌ��g",1.0f,1.5f);
            GameManager.instance.ChangeEnabledToTrigger();
            
            Instantiate(wavePrefab, position: new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            
            foreach (var trap in GameManager.instance.traps)
            {
                trap.ToggleTrap();
                //GameManager.instance.ToggleTrap();
            }
            //GameManager.instance.ToggleTraps();
        }

    }

    public void destroySwitch()
    {
        Destroy(this.gameObject);
    }
}