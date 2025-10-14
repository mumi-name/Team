using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public GameObject wavePrefab;//ボタンを取った際に出る波動
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
            //スローモードにする
            GameManager.instance.OnOffSlow(true);
            AudioManager.instance.PlaySE2("スイッチと衝撃波");
            //AusioManager.instance.PlaySEPartialOneShot("スイッチと衝撃波",1.0f,1.5f);
            GameManager.instance.ChangeEnabledToTrigger();
            Instantiate(wavePrefab, position: new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);

        }

    }
}