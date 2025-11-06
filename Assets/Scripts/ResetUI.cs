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
                Debug.Log("uiObjectÇ™Ç†ÇËÇ‹ÇπÇÒ");
            }
        }
        else
        {
            Debug.Log("PlayerÇ…êGÇÍÇƒÇ‹ÇπÇÒ");
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
