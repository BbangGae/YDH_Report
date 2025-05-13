using UnityEngine;
using DG.Tweening;

public class PortalRotator : MonoBehaviour
{
    public float rotationDuration = 3f;  // 한 바퀴 도는 데 걸리는 시간
    public float rotationAngle = 360f;   // 회전 각도

    private void Start()
    {
        RotateContinuously();
    }

    private void RotateContinuously()
    {
        transform.DORotate(new Vector3(0f, 0f, rotationAngle), rotationDuration, RotateMode.FastBeyond360)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart); // 무한 반복
    }
}
