using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 1.0f;
    bool moveFlag = false;

    private GameObject MoveStop;

    void Start()
    {
        //MoveStop = GameObject.Find("Goal").gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, SceneTransition.instance.transform.position + new Vector3(0, 1.2f, 0), Mathf.Abs(moveSpeed) * Time.deltaTime);
        }

        //鍵が南京錠に到達したら削除   MoveStop.transform.position + new Vector3(0, 1.2f, 0)
        if (transform.position == SceneTransition.instance.transform.position + new Vector3(0, 1.2f, 0))
        {
            //ゴールのロックを解除する
            SceneTransition.instance.OnOffLocked(false);
            //ここに南京錠アニメーションをさせる処理をかく
            SceneTransition.instance.StartAnimation();
       
            Destroy(gameObject, 0.1f);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moveFlag = true;
            transform.GetChild(0).gameObject.SetActive(true);
            //ゴールのロックを解除する
            //MoveStop.GetComponent<SceneTransition>().OnOffLocked(false);
            //SceneTransition.instance.OnOffLocked(false);
        }
    }


}
