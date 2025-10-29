using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public GameObject wavePrefab;//ボタンを取った際に出る波動
    public Animator animator;//アニメーター

    //public TrapController[] targetTraps;
    bool on = false;//スイッチが起動したか?
    void Start()
    {

    }
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (on) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            //スローモードにする
            PlayerScript.instance.ignoreMove(true);//同時押し仕様を無くすために復活させたよ(10/29)企業審査後
            animator.SetTrigger("SwitchTrigger");
            on = true;
            //ToggleTrap();
            GameManager.instance.OnOffSlow(true);
            //AudioManager.instance.PlaySE2("スイッチと衝撃波");
            AudioManager.instance.PlaySE2("スイッチ(衝撃波)");
            //AusioManager.instance.PlaySEPartialOneShot("スイッチと衝撃波",1.0f,1.5f);
            GameManager.instance.ChangeEnabledToTrigger();
            
            Instantiate(wavePrefab, position: new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            
            foreach (var trap in GameManager.instance.traps)
            {
                trap.ToggleTrap();
                //GameManager.instance.ToggleTrap();
            }
            //GameManager.instance.ToggleTraps();
        }

    }

    public void destroySwitch()
    {
        Destroy(this.gameObject);
    }
}