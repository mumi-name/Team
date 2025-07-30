using UnityEngine;

public class FloatingScript : MonoBehaviour
{
    public float floatHeight = 0.5f;   // 上下の移動幅
    public float floatSpeed = 2f;      // 揺れるスピード

    private Vector3 initialPosition;

    void Start()
    {
        // 初期位置を記録
        initialPosition = transform.position;
    }

    void Update()
    {
        // 上下に移動
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
