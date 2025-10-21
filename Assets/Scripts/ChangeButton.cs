using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public GameObject wavePrefab;//ボタンを取った際に出る波動
    public Animator animator;//アニメーター

    public TrapController[] targetTraps;
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
            //PlayerScript.instance.ignoreMove(true);
            animator.SetTrigger("SwitchTrigger");
            on = true;
            //ToggleTrap();
            GameManager.instance.OnOffSlow(true);
            AudioManager.instance.PlaySE2("スイッチと衝撃波");
            //AusioManager.instance.PlaySEPartialOneShot("スイッチと衝撃波",1.0f,1.5f);
            GameManager.instance.ChangeEnabledToTrigger();
            Instantiate(wavePrefab, position: new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            
            foreach(TrapController trap in targetTraps)
            {
                if(trap != null)
                {
                    trap.ToggleTrap();
                }
            }
        }

    }

    public void destroySwitch()
    {
        Destroy(this.gameObject);
    }
}