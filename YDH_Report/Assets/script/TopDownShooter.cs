using UnityEngine;
using System.Collections;
public class TopDownShooter : PlayerBaseController
{

    [Header("Dash Settings")]
public float dashSpeed = 15f;
public float dashDuration = 0.2f;
public float dashCooldown = 1.0f;

private bool isDashing = false;
private float dashCooldownTimer = 0f;
private float dashTimeRemaining = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.2f;
    private int originalDamage;
private float powerupTimer = 0f;
private bool shotgunMode = false;
private bool statBoostActive = false;
private float bulletScaleMultiplier = 1f; // 기본 1배


    public int baseDamage = 10; // 초기 공격력


    private float fireTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        fireTimer = 0f;
    }

    private void Start()
{
    originalDamage = baseDamage;
}

    protected override void Update()
    {
        base.Update(); // 👈 이동 & 애니메이션 처리

        fireTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireCooldown;
        }

        AimAtMouse();

        // ✅ 대시 입력 처리
    dashCooldownTimer -= Time.deltaTime;
    if (!isDashing && Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0f)
    {
        StartDash();
    }

    if (isDashing)
    {
        dashTimeRemaining -= Time.deltaTime;
        if (dashTimeRemaining <= 0f)
        {
            EndDash();
        }
    }
    }
    

    protected override void FixedUpdate()
{
    if (canMove)
    {
        float speed = isDashing ? dashSpeed : moveSpeed;
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }
}

private void StartDash()
{
    isDashing = true;
    dashTimeRemaining = dashDuration;
    dashCooldownTimer = dashCooldown;

    Debug.Log("💨 대시 시작!");
}

private void EndDash()
{
    isDashing = false;
    Debug.Log("🛑 대시 종료");
}
    private void AimAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = mousePos - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

   private void Shoot()
{
    if (bulletPrefab == null || firePoint == null) return;

    Vector2 baseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;

    if (shotgunMode)
    {
        FireBullet(baseDirection);
        FireBullet(Rotate(baseDirection, 15f));
        FireBullet(Rotate(baseDirection, -15f));
    }
    else
    {
        FireBullet(baseDirection);
    }
}

private void FireBullet(Vector2 direction)
{
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.linearVelocity = direction * bulletSpeed;
    }

    Bullet bulletScript = bullet.GetComponent<Bullet>();
    if (bulletScript != null)
    {
        bulletScript.shooter = this.gameObject;

        // ✅ 현재 레벨 기반 데미지 보정
        int scaledDamage = Mathf.RoundToInt(baseDamage * (1f + (MiniGameManager.Instance.currentLevel - 1) * 0.15f));
        bulletScript.damage = scaledDamage;
    }

    // ✅ STup 상태라면 크기 증가된 값이 multiplier로 반영됨
    bullet.transform.localScale *= bulletScaleMultiplier;
}


private Vector2 Rotate(Vector2 v, float degrees)
{
    float radians = degrees * Mathf.Deg2Rad;
    float sin = Mathf.Sin(radians);
    float cos = Mathf.Cos(radians);
    return new Vector2(
        v.x * cos - v.y * sin,
        v.x * sin + v.y * cos
    ).normalized;
}


public void HealPercent(float percent)
{
    CharacterStats stats = GetComponent<CharacterStats>();
    if (stats != null)
    {
        int healAmount = Mathf.RoundToInt(stats.maxHealth * percent);
        stats.currentHealth = Mathf.Min(stats.maxHealth, stats.currentHealth + healAmount);
        Debug.Log($"❤️ 체력 {healAmount} 회복됨");
    }
}

public void ActivateShotgun(float duration)
{
    shotgunMode = true;
    Invoke(nameof(EndShotgun), duration);
    Debug.Log("🔫 Shotgun 모드 시작!");
}

public void EndShotgun()
{
    shotgunMode = false;
    Debug.Log("🔚 Shotgun 모드 종료");
}

public void ActivateStatBoost(float duration)
{
    if (statBoostActive) return;

    statBoostActive = true;
    baseDamage = Mathf.RoundToInt(baseDamage * 1.5f);
    bulletScaleMultiplier = 1.3f; // 🔺 총알 크기 증가
    powerupTimer = duration;

    Debug.Log("⚡ STup 시작!");
    StartCoroutine(StatBoostTimer());
}


private IEnumerator StatBoostTimer()
{
    float timer = powerupTimer;
    while (timer > 0f)
    {
        timer -= Time.deltaTime;
        yield return null;
    }

    baseDamage = originalDamage;
    bulletScaleMultiplier = 1f; // ⬅️ 총알 크기 복원
    statBoostActive = false;
    Debug.Log("💤 STup 종료");
}

public void ResetPowerups()
{
    shotgunMode = false;
    statBoostActive = false;
    bulletScaleMultiplier = 1f;
    Debug.Log("🧼 모든 버프 초기화 완료");
}


}
