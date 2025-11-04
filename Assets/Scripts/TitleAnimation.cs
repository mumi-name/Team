using UnityEngine;
using UnityEngine.UIElements;

public class TitleAnimation : MonoBehaviour
{
    public SpriteRenderer playerSpr;
    public Sprite onSprite;
    public Sprite offSprite;

    public float interval = 1f;

    float timer = 0f;
    bool on = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSpr.sprite =offSprite;
    }

    // Update is called once per frame
    void Update()
    {
        playerSpr.transform.Rotate(0, 1, 0);

        if(playerSpr.transform.rotation.y >= -90) playerSpr.sprite = onSprite;
        if(playerSpr.transform.rotation.y <= 90) playerSpr.sprite = offSprite;


        //timer += Time.deltaTime;
        //if (timer >= interval)
        //{
        //    on=!on;
        //    if(on) playerSpr.sprite = onSprite;
        //    else playerSpr.sprite = offSprite;
        //    timer = 0f;
        //}
    }
}
