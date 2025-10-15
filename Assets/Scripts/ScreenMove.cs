using UnityEngine;

public class ScreenMove : MonoBehaviour
{
    [System.Serializable]
    public class Background
    {
        public Transform bgTransform;//�w�i��Transform
        public float speed;          //�ړ����x
        public float topLimit;       //������̈ړ����E
        public float bottomLimit;       //�������̈ړ����E
        [HideInInspector] public int direction = 1;        //1 = �����,-1 = ������
    }

    public Background[] backgrounds;
    void Start()
    {
        
    }

    
    void Update()
    {
        foreach (Background bg in backgrounds)
        {
            // �c�����Ɉړ�
            bg.bgTransform.position += new Vector3(0f, bg.speed * bg.direction * Time.deltaTime, 0f);
            
            //�㉺���`�F�b�N���ĕ������]
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
