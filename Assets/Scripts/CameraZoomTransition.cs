using UnityEngine;

public class CameraZoomTransition : MonoBehaviour
{

    public static CameraZoomTransition instance;
    public float transitionTime = 1f; // ズームにかける秒数
    public float projectionTime = 1f;//カメラ遷移後映す時間

    public Camera mainCamera;         // メインカメラ
    public Camera itemZoomCamera;    //最初にズームするカメラ
    public Camera zoomCamera;         // ズームカメラ（最終的な位置・サイズだけ使う

    private Vector3 startPos;
    private Vector3 endPos;

    private float timer = 0f;
    private float startSize;
    private float endSize;
    private bool isZooming = false;

    void Start()
    {
        instance = this;
        SceneStartZoom();
        PlayerScript.instance.cannotMoveMode();
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

    //突貫工事で設定。アイテムがあるシーンは2点間のカメラをアニメーションさせる
    public void SceneStartZoom()
    {
        
        startPos = mainCamera.transform.position;
        startSize = mainCamera.orthographicSize;
        endPos = zoomCamera.transform.position;
        endSize = zoomCamera.orthographicSize;
        timer = 0f;
        isZooming = true;
        Invoke("SceneStartZoom2", projectionTime + transitionTime-1f);

    }

    public void SceneStartZoom2()
    {
        startPos = zoomCamera.transform.position;
        startSize = zoomCamera.orthographicSize;
        endPos = itemZoomCamera.transform.position;
        endSize = itemZoomCamera.orthographicSize;
        timer = 0f;
        isZooming = true;

        Invoke("ReturnZoom2", projectionTime + transitionTime-1f);
        Invoke("canMove", transitionTime);
    }

    public void ReturnZoom2()
    {
        
        startPos = itemZoomCamera.transform.position;
        startSize= itemZoomCamera.orthographicSize;
        endPos = new Vector3(0, 0, -10);
        endSize = 5;
        timer = 0f;
        isZooming = true;

        
    }


    public void StartZoom(bool change=false)
    {
        
        Camera zoom;
        if (change) zoom = zoomCamera;
        else zoom = itemZoomCamera;

        startPos = mainCamera.transform.position;
        startSize = mainCamera.orthographicSize;
        endPos = zoom.transform.position;
        endSize = zoom.orthographicSize;
        timer = 0f;
        isZooming = true;
        if (change)
        {
            Invoke("ChangeAnimation", transitionTime);
            Invoke("ReturnZoom", projectionTime + transitionTime);
        }
        else
        {
            Invoke("ReturnZoom", projectionTime + transitionTime);
        }

    }


    //チュートリアル時は、カメラ遷移後にアニメーションが起きる
    void ChangeAnimation()
    {
        int num = PlayerScript.instance.GetMode();
        bool on = false;

        if (num > 0) on = true;
        else if (num < 0) on = false;

        foreach (var brock in GameManager.instance.brocks)
        {
            if (brock.GetChanged() == true) return;
            
            brock.on = !brock.on;
            //if (on) brock.ON(true);
            //else brock.OFF(true);
            brock.OnOff(on,true);
            brock.OnfadeAnimation();
            brock.SetOnChanged();

        }
    }

    public void ReturnZoom()
    {
        //判定を元に戻す
        if (endPos != itemZoomCamera.gameObject.transform.position)
        {
            foreach (var brock in GameManager.instance.brocks)
            {
                brock.ChangeTriggerToEnabled();
            }
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
