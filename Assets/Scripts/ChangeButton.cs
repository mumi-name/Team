using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public GameObject wavePrefab;//�{�^����������ۂɏo��g��
    void Start()
    {

    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�X���[���[�h�ɂ���
            GameManager.instance.OnOffSlow(true);
            GameManager.instance.ChangeEnabledToTrigger();
            Instantiate(wavePrefab, position: new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);

        }

    }
}