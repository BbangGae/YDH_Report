using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public AudioClip hitSound;
    public string characterName = "Unnamed";
    public int maxHealth = 100;
    public int currentHealth;
    public int level = 1;

    public GameObject hitEffectPrefab;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        Debug.Log($"{characterName}가 {damage} 피해를 입음. 남은 체력: {currentHealth}");

          // ✅ 히트 이펙트 출력
    if (hitEffectPrefab != null)
    {
        GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f); // 자동 제거

    }

      if (CompareTag("Player"))
    {
        PlayerDamageFlash flash = GetComponent<PlayerDamageFlash>();
        if (flash != null)
            flash.TriggerFlash();
    }

     if (hitSound != null)
        SoundManager.Instance.PlaySFX(hitSound);

         // ✅ 공통 피격 사운드도 중복 출력
    if (CompareTag("Player"))
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.playerHitSFX);
    }
    else if (CompareTag("Enemy"))
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyHitSFX);
    }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

  protected virtual void Die()
{
    Debug.Log($"{characterName}가 사망함.");

    if (CompareTag("Player") && MiniGameManager.Instance != null && MiniGameManager.Instance.isMiniGameActive)
    {
        MiniGameManager.Instance.HandlePlayerDeath();
    }
    
}



    public void ApplyLevelScaling()
{
    float multiplier = 1f + (level - 1) * 0.3f; // 예: 30%씩 강화
    maxHealth = Mathf.RoundToInt(maxHealth * multiplier);
    currentHealth = maxHealth;
}
}
