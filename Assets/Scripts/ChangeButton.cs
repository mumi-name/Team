using UnityEngine;

public class ChangeButton : MonoBehaviour
{
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
            int num = PlayerScript.instance.GetMode();//���݂̌������擾
            bool flag=true;
            if (num > 0) flag = true;
            if (num < 0) flag = false;
            GameManager.instance.ChangeOnOff(flag);
            Destroy(this.gameObject);

        }

    }
}
