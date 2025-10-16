using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed=1.0f;

    bool moveFlag = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, SceneTransition.instance.transform.position, Mathf.Abs(moveSpeed) * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveFlag = true;
            //ƒS[ƒ‹‚ÌƒƒbƒN‚ğ‰ğœ‚·‚é
            SceneTransition.instance.OnOffLocked(false);
        }
    }

   

}
