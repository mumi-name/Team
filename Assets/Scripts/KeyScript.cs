using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed=1.0f;
    bool moveFlag = false;

    private Vector3 MoveStop;

    void Start()
    {
        MoveStop = GameObject.Find("Goal").gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, MoveStop, Mathf.Abs(moveSpeed) * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveFlag = true;
            //�S�[���̃��b�N����������
            SceneTransition.instance.OnOffLocked(false);
        }
    }

   

}
