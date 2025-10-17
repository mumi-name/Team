using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed=1.0f;
    bool moveFlag = false;

    private GameObject MoveStop;

    void Start()
    {
        MoveStop = GameObject.Find("Goal").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, MoveStop.transform.position, Mathf.Abs(moveSpeed) * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveFlag = true;
            //ÉSÅ[ÉãÇÃÉçÉbÉNÇâèúÇ∑ÇÈ
            MoveStop.GetComponent<SceneTransition>().OnOffLocked(false);
            //SceneTransition.instance.OnOffLocked(false);
        }
    }


}
