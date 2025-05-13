using UnityEngine;

public class EnemyController : PlayerBaseController
{
    public float minChaseDistance = 3f;
    public float maxChaseDistance = 7f;
    public float preferredDistance = 5f;
    public float attackCooldown = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    private CharacterStats targetStats;

    private Transform player;
    private float attackTimer = 0f;

    // 🔁 개별 배회 방향을 위한 랜덤 시드
    private float wanderSeedX;
    private float wanderSeedY;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
             targetStats = playerObj.GetComponent<CharacterStats>();

        wanderSeedX = Random.Range(0f, 100f);
        wanderSeedY = Random.Range(0f, 100f);
    }


        protected override void Update()
{
    // 🛡️ 타겟이 없거나 체력이 0 이하이면 아무 행동도 하지 않음
    if (player == null || targetStats == null || targetStats.currentHealth <= 0)
    {
        moveInput = Vector2.zero;
        return;
    }

    Vector2 directionToPlayer = player.position - transform.position;
    float distance = directionToPlayer.magnitude;

    if (distance < minChaseDistance)
    {
        moveInput = -directionToPlayer.normalized;
    }
    else if (distance > maxChaseDistance)
    {
        Vector2 baseDirection = directionToPlayer.normalized;
        Vector2 noise = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
        moveInput = (baseDirection + noise).normalized;
    }
    else
    {
        moveInput = GetWanderDirection();
        TryShoot();
    }

    // 🧭 방향 전환 처리
    if (player.position.x > transform.position.x && !facingRight)
    {
        Flip(true);
    }
    else if (player.position.x < transform.position.x && facingRight)
    {
        Flip(false);
    }
}

        
    

    // 🎲 개별 배회 방향 생성
    private Vector2 GetWanderDirection()
    {
        float x = Mathf.PerlinNoise(Time.time + wanderSeedX, 0f) - 0.5f;
        float y = Mathf.PerlinNoise(0f, Time.time + wanderSeedY) - 0.5f;
        return new Vector2(x, y).normalized;
    }

    private void TryShoot()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            ShootAtPlayer();
            attackTimer = attackCooldown;
        }
    }

    private void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 shootDirection = (player.position - firePoint.position).normalized;
            rb.linearVelocity = shootDirection * bulletSpeed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.shooter = this.gameObject;
        }
    }

   
}
