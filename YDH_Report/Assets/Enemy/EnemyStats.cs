using UnityEngine;

public class EnemyStats : CharacterStats
{
    public GameObject[] powerupPrefabs; // 프리팹 3종 연결

    public AudioClip deathSound;
    protected override void Die()
{
    Debug.Log($"{characterName}가 사망함. (EnemyStats)");

     // ✅ 개별 사망 효과음 재생
    if (deathSound != null)
    {
        SoundManager.Instance.PlaySFX(deathSound);
    }

    DropPowerup();

    // ✅ 웨이브 시스템에 알림
    MiniGameManager.Instance.EnemyDefeated();

    // ✅ 기본 사망 처리도 실행
    base.Die();

     MiniGameManager.Instance.AddKill();

    // ✅ 적 오브젝트 파괴
    Destroy(gameObject);
}

    private void DropPowerup()
{
    if (powerupPrefabs.Length == 0) return;
    if (Random.value > 0.3f) return; // 50% 확률로 드롭

    GameObject powerup = Instantiate(powerupPrefabs[Random.Range(0, powerupPrefabs.Length)], transform.position, Quaternion.identity);
}
}
