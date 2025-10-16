using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    [System.Serializable]
    public class Background
    {
        public Transform bgTransform;//”wŒi‚ÌTransform
        public float speed;          //ˆÚ“®‘¬“x
        public float topLimit;       //ã•ûŒü‚ÌˆÚ“®ŒÀŠE
        public float bottomLimit;       //‰º•ûŒü‚ÌˆÚ“®ŒÀŠE
        [HideInInspector] public int direction = 1;        //1 = ã•ûŒü,-1 = ‰º•ûŒü
    }

    public Background[] backgrounds;
    void Start()
    {
        
    }

    
    void Update()
    {
        foreach (Background bg in backgrounds)
        {
            // c•ûŒü‚ÉˆÚ“®
            bg.bgTransform.position += new Vector3(0f, bg.speed * bg.direction * Time.deltaTime, 0f);
            
            //ã‰ºŒÀƒ`ƒFƒbƒN‚µ‚Ä•ûŒü”½“]
            if(bg.bgTransform.position.y >= bg.topLimit)
            {
                bg.direction = -1;
            }
            else if(bg.bgTransform.position.y <= bg.bottomLimit)
            {
                bg.direction = 1;
            }
        }
    }
}
