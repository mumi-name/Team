using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public static ScreenMove instance;
    [System.Serializable]
    public class Background
    {
        public Transform bgTransform;//”wŒi‚ÌTransform
        public float speed;          //ˆÚ“®‘¬“x
        public float topLimit;       //ã•ûŒü‚ÌˆÚ“®ŒÀŠE
        public float bottomLimit;       //‰º•ûŒü‚ÌˆÚ“®ŒÀŠE
        [HideInInspector] public int direction = 1;        //1 = ã•ûŒü,-1 = ‰º•ûŒü
        public bool isLeft = false;
    }

    public Background[] backgrounds;
    [HideInInspector] public bool isOn = true; // ON/OFF”½“]ƒtƒ‰ƒO

    void Start()
    {

        instance = this;
        // ‰ŠúÝ’è
        foreach (var bg in backgrounds)
        {
            bg.direction = isOn ? (bg.isLeft ? -1 : 1) : (bg.isLeft ? 1 : -1);
        }
    }

    
    void Update()
    {
        foreach (var bg in backgrounds)
        {
            // ˆÚ“®
            bg.bgTransform.position += new Vector3(0f, bg.speed * bg.direction * Time.deltaTime, 0f);

            // ã‰ºŒÀ‚Å•ûŒü”½“]
            if (bg.bgTransform.position.y >= bg.topLimit)
                bg.direction = -1;
            else if (bg.bgTransform.position.y <= bg.bottomLimit)
                bg.direction = 1;
        }
        /*
        foreach (Background bg in backgrounds)
        {
            int moveDir = isReversed ? -bg.direction : bg.direction;
            bg.bgTransform.position += new Vector3(0f, bg.speed * moveDir * Time.deltaTime, 0f);
        
            // ã‰ºŒÀ‚Å•ûŒü”½“]
            if (bg.bgTransform.position.y >= bg.topLimit)
                bg.direction = -1;
            else if (bg.bgTransform.position.y <= bg.bottomLimit)
                bg.direction = 1;
        }
        */
        /*
        foreach (Background bg in backgrounds)
        {
            int dir = 0;

            if (isOn)
            {
                dir = bg.isLeft ? -1 : 1; // ¶‚Í‰º(-1)A‰E‚Íã(1)
            }
            else
            {
                dir = bg.isLeft ? 1 : -1; // ¶‚Íã(1)A‰E‚Í‰º(-1)
            }

            bg.bgTransform.position += new Vector3(0f, bg.speed * dir * Time.deltaTime, 0f);

            // ã‰ºŒÀƒ`ƒFƒbƒNi•K—v‚É‰ž‚¶‚Äj
            if (bg.bgTransform.position.y >= bg.topLimit)
            {
                bg.bgTransform.position = new Vector3(bg.bgTransform.position.x, bg.topLimit, bg.bgTransform.position.z);
            }
            else if (bg.bgTransform.position.y <= bg.bottomLimit)
            {
                bg.bgTransform.position = new Vector3(bg.bgTransform.position.x, bg.bottomLimit, bg.bgTransform.position.z);
            }
        
        }
        */
    }
    public void Toggle()
    {
        isOn = !isOn;
        foreach (var bg in backgrounds)
        {
            bg.direction = isOn ? (bg.isLeft ? -1 : 1) : (bg.isLeft ? 1 : -1);
        }
        //isOn = !isOn;
    }
}
