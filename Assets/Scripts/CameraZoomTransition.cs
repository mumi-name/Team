using UnityEngine;

public class CameraZoomTransition : MonoBehaviour
{
    public Camera mainCamera;         // メインカメラ
    public Camera zoomCamera;         // ズームカメラ（最終的な位置・サイズだけ使う）
    public float transitionTime = 1f; // ズームにかける秒数
    public float projectionTime = 1f;//カメラ遷移後映す時間
    public static CameraZoomTransition instance;

    float timer = 0f;
    float startSize;
    float endSize;
    Vector3 startPos;
    Vector3 endPos;
    bool isZooming = false;


    void Start()
    {
        instance = this;
    }
    void Update()
    {
        if (isZooming)
        {
            timer += Time.unscaledDeltaTime; // スローでも一定速度
            float t = Mathf.Clamp01(timer / transitionTime);

            // 位置補間
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            // サイズ補間（2Dの場合はorthographicSizeでズーム）
            mainCamera.orthographicSize = Mathf.Lerp(startSize, endSize, t);


            if (t >= 1f)
            {
                isZooming = false;

            }
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
            ReturnZoom();
    }

    public void StartZoom()
    {
        startPos = mainCamera.transform.position;
        startSize = mainCamera.orthographicSize;
        endPos = zoomCamera.transform.position;
        endSize = zoomCamera.orthographicSize;
        timer = 0f;
        isZooming = true;
        Invoke("ChangeAnimation", transitionTime);
        Invoke("ReturnZoom", projectionTime + transitionTime);
    }

    //チュートリアル時は、カメラ遷移後にアニメーションが起きる
    void ChangeAnimation()
    {
        int num = PlayerScript.instance.GetMode();//????????????????
        bool on = false;

        if (num > 0) on = true;
        else if (num < 0) on = false;

        foreach (var brock in GameManager.instance.brocks)
        {
            if (brock.GetChanged() == true) return;
            
            brock.on = !brock.on;
            if (on) brock.ON(true);
            else brock.OFF(true);
            brock.OnfadeAnimation();
            brock.SetOnChanged();

        }
    }

    public void ReturnZoom()
    {
        //判定を元に戻す
        foreach (var brock in GameManager.instance.brocks)
        {
            brock.ChangeTriggerToEnabled();
        }
        Vector3 tempPos = startPos;
        float tempSize = startSize;
        startPos = endPos;
        startSize = endSize;
        endPos = tempPos;
        endSize = tempSize;
        timer = 0f;
        isZooming = true;
        Invoke("canMove", transitionTime);
    }

    void canMove()
    {
        PlayerScript.instance.canMoveMode();
    }


}
