using UnityEngine;
using DG.Tweening;
public class IrisShot : MonoBehaviour
{
    [SerializeField] RectTransform unmask;
    readonly Vector2 IRIS_IN_SCALE = new Vector2(30, 30);
    readonly float SCALE_DURATION = 1f;

    public void IrisIN()
    {
        //unmask.DOScale(IRIS_IN_SCALE, SCALE_DURATION).SetEase(Ease.InCubic);
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
