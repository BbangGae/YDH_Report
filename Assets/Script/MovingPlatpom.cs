using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.up; // 기본 위아래
    public float moveDistance = 3f;            // 이동 거리
    public float moveSpeed = 2f;               // 이동 속도

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool goingToTarget = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveDirection.normalized * moveDistance;
    }

    void Update()
    {
        Vector3 destination = goingToTarget ? targetPos : startPos;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            goingToTarget = !goingToTarget;
        }
    }
}
