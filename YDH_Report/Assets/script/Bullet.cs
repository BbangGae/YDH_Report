using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float maxTravelDistance = 10f;
    public int damage = 10;

    private Vector3 startPosition;

    [HideInInspector]
    public GameObject shooter; // 🔑 누가 발사했는지 저장

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 🛡️ 자기 자신(또는 부모 오브젝트)일 경우 충돌 무시
        if (other.gameObject == shooter) return;

        CharacterStats targetStats = other.GetComponent<CharacterStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
