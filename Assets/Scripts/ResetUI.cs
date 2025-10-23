using UnityEngine;
public class ResetUI : MonoBehaviour
{
    public GameObject uiObject;

    void Start()
    {
        if(uiObject != null)
        {
            uiObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if(uiObject != null)
            {
                uiObject.SetActive(true);
            }
            else
            {
                Debug.Log("uiObject���Ȃ���(^��^)");
            }
        }
        else
        {
            Debug.Log("Player�ɐG��ĂȂ���");
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (uiObject != null)
            {
                uiObject.SetActive(false);
            }
        }
    }

}
