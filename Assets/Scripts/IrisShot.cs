using UnityEngine;
using DG.Tweening;
using Coffee.UIExtensions;
public class IrisShot : MonoBehaviour
{
    public static IrisShot instance;

    [SerializeField] RectTransform unmask;
    private bool startClosed = false;
    private bool hasOpened = false;
    readonly Vector2 IRIS_IN_SCALE = new Vector2(15, 15);
    readonly Vector2 IRIS_MID_SCALE1 = new Vector2(0.8f, 0.8f);
    readonly Vector2 IRIS_MID_SCALE2 = new Vector2(1.2f, 1.2f);
    //public Vector3 IRIS_IN_SCALE = Vector3.one;
    readonly float SCALE_DURATION = 1f;

    void Start()
    {
        

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //unmask.localScale = IRIS_IN_SCALE;
            /*
            if (startClosed)
            {
                unmask.localScale = IRIS_IN_SCALE;//ˆÃ“]‚©‚çŽn‚Ü‚é
            }
            else unmask.localScale = Vector3.zero; ;
            */
        }
        else Destroy(gameObject);
    }
    public void IrisIN()
    {
        if (hasOpened) return;
        hasOpened = true;

        //unmask.DOScale(new Vector3(0, 0, 0), SCALE_DURATION).SetEase(Ease.OutCubic);
        unmask.DOScale(IRIS_MID_SCALE2, 0.4f).SetEase(Ease.InCubic);
        unmask.DOScale(IRIS_MID_SCALE1, 0.2f).SetDelay(0.4f).SetEase(Ease.OutCubic);
        unmask.DOScale(IRIS_IN_SCALE, 0.2f).SetDelay(0.6f).SetEase(Ease.InCubic);
        /*unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION)
          .SetEase(Ease.InCubic)
          .OnComplete(() => {
              onComplete?.Invoke();
          });
        */
    }

    public void IrisOut()
    {
        //unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION).SetEase(Ease.InCubic);
        //unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION).SetEase(Ease.InCubic).OnComplete(() => unmask.localScale = Vector3.zero);
        unmask.DOScale(IRIS_MID_SCALE1, 0.2f).SetEase(Ease.InCubic);
        unmask.DOScale(IRIS_MID_SCALE2, 0.2f).SetDelay(0.2f).SetEase(Ease.OutCubic);
        unmask.DOScale(new Vector2(0, 0), 0.4f).SetDelay(0.4f).SetEase(Ease.InCubic);

        /*
        unmask.DOScale(Vector3.zero, SCALE_DURATION)
          .SetEase(Ease.OutCubic)
          .OnComplete(() => {
              onComplete?.Invoke();
          });
        */
    }

    public void ResetIris()
    {
        hasOpened = false;
        unmask.localScale = IRIS_IN_SCALE;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            IrisIN();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            IrisOut();
        }
    }
}