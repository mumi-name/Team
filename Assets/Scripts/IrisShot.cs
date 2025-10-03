using UnityEngine;
using DG.Tweening;
//using Coffee.UIExtensions;
public class IrisShot : MonoBehaviour
{
    public static IrisShot instance;

    [SerializeField] RectTransform unmask;
    readonly Vector2 IRIS_IN_SCALE = new Vector2(30, 30);
    //public Vector3 IRIS_IN_SCALE = Vector3.one;
    readonly float SCALE_DURATION = 1f;

    void Start()
    {
        unmask.localScale = IRIS_IN_SCALE;
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void IrisIN()
    {

        unmask.DOScale(new Vector3(0, 0, 0), SCALE_DURATION).SetEase(Ease.OutCubic);

        /*unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION)
          .SetEase(Ease.InCubic)
          .OnComplete(() => {
              onComplete?.Invoke();
          });
        */
    }

    public void IrisOut()
    {
        unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION).SetEase(Ease.InCubic);
        
        /*
        unmask.DOScale(Vector3.zero, SCALE_DURATION)
          .SetEase(Ease.OutCubic)
          .OnComplete(() => {
              onComplete?.Invoke();
          });
        */
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