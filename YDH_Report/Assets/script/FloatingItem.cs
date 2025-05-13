using UnityEngine;
using DG.Tweening;

public class FloatingItem : MonoBehaviour
{
    public float floatHeight = 0.3f;     // 부유 높이
    public float floatDuration = 1.5f;   // 한 왕복에 걸리는 시간

    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;

        // Y축 기준으로 위아래 부유
        transform.DOMoveY(originalPos.y + floatHeight, floatDuration)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo);
    }
}
