using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 현재 따라가는 대상 (기본은 플레이어)
    public float followSpeed = 5f;
    public float zoomSpeed = 2f;
    public float defaultZoom = 5f;

    private Camera cam;
    

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetToPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            SetTarget(playerObj.transform);
        }
    }

    public void StartCinematicZoom(Vector3 worldPos, float zoomSize, float duration)
    {
        StartCoroutine(CinematicSequence(worldPos, zoomSize, duration));
    }

    private IEnumerator CinematicSequence(Vector3 worldPos, float zoomSize, float duration)
    {
        
        SetTarget(null); // 추적 중지

        Vector3 targetPos = worldPos;
        targetPos.z = transform.position.z;

        float timer = 0f;
        float startZoom = cam.orthographicSize;
        Vector3 startPos = transform.position;

        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, timer / duration);
            cam.orthographicSize = Mathf.Lerp(startZoom, zoomSize, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        cam.orthographicSize = zoomSize;

        // 연출 유지 시간 후 복귀
        yield return new WaitForSeconds(1.5f);
        ResetToPlayer();
        StartCoroutine(ResetZoom(startZoom));
    }

    private IEnumerator ResetZoom(float originalSize)
    {
        while (Mathf.Abs(cam.orthographicSize - originalSize) > 0.05f)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalSize, zoomSpeed * Time.deltaTime);
            yield return null;
        }

        cam.orthographicSize = originalSize;
        
    }

    public void StartTourSequence(List<Transform> points, float zoomSize, float duration)
{
    StartCoroutine(TourSequence(points, zoomSize, duration));
}

private IEnumerator TourSequence(List<Transform> points, float zoomSize, float duration)
{
    
    SetTarget(null); // 추적 중단

    float startZoom = cam.orthographicSize;
    Vector3 startPos = transform.position;

    foreach (var point in points)
    {
        if (point == null) continue;

        Vector3 targetPos = point.position;
        targetPos.z = transform.position.z;

        float timer = 0f;
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, timer / duration);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        cam.orthographicSize = zoomSize;

        yield return new WaitForSeconds(1.2f); // 포인트 고정 시간
    }

    // 복귀
    yield return new WaitForSeconds(0.5f);
    ResetToPlayer();
    StartCoroutine(ResetZoom(startZoom));
}


public void RestoreLastCameraPosition(Vector3 lastPos, float holdTime = 1.5f)
{
    StartCoroutine(RestoreAndResume(lastPos, holdTime));
}

private IEnumerator RestoreAndResume(Vector3 lastPos, float holdTime)
{
    SetTarget(null); // 일단 추적 중단
    transform.position = lastPos;

    yield return new WaitForSeconds(holdTime);

    ResetToPlayer(); // 다시 플레이어 따라가도록 설정
}


}
