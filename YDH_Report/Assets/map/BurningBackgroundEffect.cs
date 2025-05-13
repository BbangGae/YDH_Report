using UnityEngine;
using DG.Tweening;

public class BurningBackgroundEffect : MonoBehaviour
{
    public SpriteRenderer backgroundSprite;
    public float colorFlickerSpeed = 0.5f;
    public float flickerIntensity = 0.2f;
    public float scaleWobbleAmount = 0.05f;
    public float scaleWobbleSpeed = 1f;

    private Color originalColor;
    private Vector3 originalScale;

    private void Start()
    {
        if (backgroundSprite == null)
            backgroundSprite = GetComponent<SpriteRenderer>();

        originalColor = backgroundSprite.color;
        originalScale = transform.localScale;

        FlickerColor();
        WobbleScale();
    }

    private void FlickerColor()
    {
        DOTween.To(() => backgroundSprite.color, 
                   x => backgroundSprite.color = x, 
                   originalColor * (1f + flickerIntensity), 
                   colorFlickerSpeed)
               .SetLoops(-1, LoopType.Yoyo)
               .SetEase(Ease.InOutSine);
    }

    private void WobbleScale()
    {
        transform.DOScale(originalScale * (1f + scaleWobbleAmount), scaleWobbleSpeed)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }
}
