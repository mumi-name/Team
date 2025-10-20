using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    public static ScreenMove instance;
    [System.Serializable]
    public class Background
    {
        public Transform bgTransform;//�w�i��Transform
        public float speed;          //�ړ����x
        public float topLimit;       //������̈ړ����E
        public float bottomLimit;       //�������̈ړ����E
        [HideInInspector] public int direction = 1;        //1 = �����,-1 = ������
        public bool isLeft = false;
    }

    public Background[] backgrounds;
    [HideInInspector] public bool isOn = true; // ON/OFF���]�t���O

    void Start()
    {

        instance = this;
        // �����ݒ�
        foreach (var bg in backgrounds)
        {
            bg.direction = isOn ? (bg.isLeft ? -1 : 1) : (bg.isLeft ? 1 : -1);
        }
    }

    
    void Update()
    {
        foreach (var bg in backgrounds)
        {
            // �ړ�
            bg.bgTransform.position += new Vector3(0f, bg.speed * bg.direction * Time.deltaTime, 0f);

            // �㉺���ŕ������]
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
        
            // �㉺���ŕ������]
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
                dir = bg.isLeft ? -1 : 1; // ���͉�(-1)�A�E�͏�(1)
            }
            else
            {
                dir = bg.isLeft ? 1 : -1; // ���͏�(1)�A�E�͉�(-1)
            }

            bg.bgTransform.position += new Vector3(0f, bg.speed * dir * Time.deltaTime, 0f);

            // �㉺���`�F�b�N�i�K�v�ɉ����āj
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
