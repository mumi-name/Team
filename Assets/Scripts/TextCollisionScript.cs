using System.Xml;
using TMPro;
using UnityEngine;

public class TextCollisionScript : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーがテキストと被ったらフォントの色を薄くする
        if (collision.gameObject.CompareTag("Player"))
        {

            Color c = timeText.color;
            c.a = 0.2f;
            timeText.color = c;

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーがテキストと被ったらフォントの色を薄くする
        if (collision.gameObject.CompareTag("Player"))
        {

            Color c = timeText.color;
            c.a = 1f;
            timeText.color = c;

        }
    }
}
