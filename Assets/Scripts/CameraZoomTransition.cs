using UnityEngine;

public class CameraZoomTransition : MonoBehaviour
{

    public static CameraZoomTransition instance;
    public float transitionTime = 1f; // �Y�[���ɂ�����b��
    public float projectionTime = 1f;//�J�����J�ڌ�f������

    public Camera mainCamera;         // ���C���J����
    public Camera itemZoomCamera;    //�ŏ��ɃY�[������J����
    public Camera zoomCamera;         // �Y�[���J�����i�ŏI�I�Ȉʒu�E�T�C�Y�����g��

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

    //�ˊэH���Őݒ�B�A�C�e��������V�[����2�_�Ԃ̃J�������A�j���[�V����������
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
    }

    public void ReturnZoom2()
    {
        startPos= itemZoomCamera.transform.position;
        startSize= itemZoomCamera.orthographicSize;
        endPos = new Vector3(0, 0, -10);
        endSize = 5;
        timer = 0f;
        isZooming = true;

        Invoke("canMove", transitionTime);
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
            //if (on) brock.ON(true);
            //else brock.OFF(true);
            brock.OnOff(on,true);
            brock.OnfadeAnimation();
            brock.SetOnChanged();

        }
    }

    public void ReturnZoom()
    {
        //��������ɖ߂�
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
