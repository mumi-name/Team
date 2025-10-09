using UnityEngine;

public class CameraZoomTransition : MonoBehaviour
{
    public Camera mainCamera;         // ���C���J����
    public Camera zoomCamera;         // �Y�[���J�����i�ŏI�I�Ȉʒu�E�T�C�Y�����g���j
    public float transitionTime = 1f; // �Y�[���ɂ�����b��
    public float projectionTime = 1f;//�J�����J�ڌ�f������
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
            timer += Time.unscaledDeltaTime; // �X���[�ł���葬�x
            float t = Mathf.Clamp01(timer / transitionTime);

            // �ʒu���
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            // �T�C�Y��ԁi2D�̏ꍇ��orthographicSize�ŃY�[���j
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

    //�`���[�g���A�����́A�J�����J�ڌ�ɃA�j���[�V�������N����
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
        //��������ɖ߂�
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
