using UnityEngine;

public class FloatingScript : MonoBehaviour
{
    public float floatHeight = 0.5f;   // �㉺�̈ړ���
    public float floatSpeed = 2f;      // �h���X�s�[�h

    private Vector3 initialPosition;

    void Start()
    {
        // �����ʒu���L�^
        initialPosition = transform.position;
    }

    void Update()
    {
        // �㉺�Ɉړ�
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
