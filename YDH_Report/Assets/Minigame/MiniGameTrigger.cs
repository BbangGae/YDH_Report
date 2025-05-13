using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MiniGameTrigger : MonoBehaviour
{
    public GameObject miniGameUI;
    public MiniGameManager miniGameManager;
    private bool hasEntered = false;

    [Header("📦 몬스터 생성 설정")]
    public List<GameObject> enemyPrefabs;            // 몬스터 프리팹 리스트
    public List<Transform> enemySpawnPoints;         // 스폰 위치들
    public bool spawnRandom = false;                 // 랜덤 스포너 여부
    public int spawnCount = 3;                       // 생성할 적 수

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasEntered || miniGameManager.isMiniGameActive) return;

        if (other.CompareTag("Player"))
        {
            hasEntered = true;

            if (miniGameUI != null)
                miniGameUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasEntered = false;
            if (miniGameUI != null)
                miniGameUI.SetActive(false);
        }
    }



   public void SpawnEnemies(int level)
{
    if (enemyPrefabs.Count == 0 || GameObject.FindGameObjectWithTag("Player") == null) return;

    Transform player = GameObject.FindGameObjectWithTag("Player").transform;

    MiniGameManager.Instance.ResetWaveEnemyCount();

    int adjustedSpawnCount = spawnCount + ((level - 1) / 2);
    MiniGameManager.Instance.SetWaveEnemyCount(adjustedSpawnCount);
    MiniGameManager.Instance.isWaveCleared = false;

    for (int i = 0; i < adjustedSpawnCount; i++)
    {
        GameObject prefab;

        if (spawnRandom)
        {
            List<GameObject> availablePrefabs = new();

            for (int j = 0; j < enemyPrefabs.Count; j++)
            {
                if (j == 1 && level < 5) continue; // Enemy_2 제한
                availablePrefabs.Add(enemyPrefabs[j]);
            }

            if (availablePrefabs.Count == 0) return;

            prefab = availablePrefabs[Random.Range(0, availablePrefabs.Count)];
        }
        else
        {
            int index = i % enemyPrefabs.Count;

            // 순차일 때 Enemy_2 제한
            if (index == 1 && level < 5)
                index = 0;

            prefab = enemyPrefabs[index];
        }

        Vector2 spawnPos = GetValidSpawnPosition(player.position, 3f, 6f);
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        var stats = enemy.GetComponent<CharacterStats>();
        if (stats != null)
        {
            stats.level = level;
            stats.ApplyLevelScaling();
        }

        MiniGameManager.Instance.RegisterEnemy();
    }
}



    private Vector2 GetValidSpawnPosition(Vector2 center, float minDist, float maxDist)
    {
        const int maxTries = 10;

        for (int i = 0; i < maxTries; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(minDist, maxDist);
            Vector2 spawnPos = center + randomOffset;

            Collider2D[] hits = Physics2D.OverlapCircleAll(spawnPos, 0.5f);
            bool isEmpty = true;
            foreach (var hit in hits)
            {
                if (!hit.isTrigger)
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isEmpty) return spawnPos;
        }

        return center + Random.insideUnitCircle.normalized * maxDist; // fallback
    }
}