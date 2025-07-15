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
            GameManager.instance.ChangeOnOff(PlayerScript.instance.GetMode());
            Destroy(this.gameObject);

        }

    }
}
