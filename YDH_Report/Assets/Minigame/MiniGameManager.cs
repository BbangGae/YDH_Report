using UnityEngine;
using System.Collections;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    public int currentLevel = 1;
    private int monstersAlive = 0;
    private int currentWaveEnemyCount = 0; // ✅ 현재 웨이브의 몬스터 수

    public MiniGameTrigger miniGameTrigger;
    public PlayerController playerController;
    public TopDownShooter topDownShooter;

    public bool isWaveCleared = false;

    public bool isGameOver = false;

    private bool isSpawningWave = false;
    public GameObject healthBarUI;

    



    public bool isMiniGameActive = false;

    public GameObject miniGameUI; 

    public GameObject gameOverUI; // ✅ 게임오버 UI 직접 연결

    private int killCount = 0;    // ✅ 킬 수 기록

    private void Awake()
    {
        Instance = this;
    }

    public void EnterMiniGame()
{
    if (isMiniGameActive) return;

    StartCoroutine(EnterMiniGameCoroutine());
}

private IEnumerator EnterMiniGameCoroutine()
{
    isMiniGameActive = true;
    SoundManager.Instance.PlayBGM(SoundManager.Instance.miniGameBGM);
    currentLevel = 1;
    monstersAlive = 0;
    currentWaveEnemyCount = 0;

    playerController.enabled = false;
    topDownShooter.enabled = true;
    ScoreManager.Instance?.StartScore();
    if (healthBarUI != null) healthBarUI.SetActive(true);

    Debug.Log("🕐 대응 시간 시작 - 플레이어 준비 시간");

    yield return new WaitForSeconds(2f); // 💡 대응 시간 2초

    Debug.Log("🚨 적 소환 시작");
    yield return StartCoroutine(SpawnInitialWave());
}


    private IEnumerator SpawnInitialWave()
    {
        yield return new WaitForSeconds(2f); // 효과나 연출 여유
        miniGameTrigger.SpawnEnemies(currentLevel);
    }

    public void ExitMiniGame()
    {
        if (!isMiniGameActive) return;

          

        isMiniGameActive = false;
        playerController.enabled = true;
        topDownShooter.enabled = false;

        ScoreManager.Instance?.StopScore();       // ✅ 점수 멈춤
        ScoreManager.Instance?.ShowFinalScore();  // ✅ 최종 점수 표기
        if (healthBarUI != null) healthBarUI.SetActive(false); 
    }

    public void RegisterEnemy()
    {
        monstersAlive++;
        Debug.Log($"🧟‍♂️ 적 등록됨. 현재 총 {monstersAlive}마리");
    }

    public void ResetWaveEnemyCount()
    {
        currentWaveEnemyCount = 0;
    }

    public void SetWaveEnemyCount(int count)
    {
        currentWaveEnemyCount = count;
        Debug.Log($"🎯 이번 그룹 총 적 수 설정됨: {count}마리");
    }

    public void EnemyDefeated()
    {
        monstersAlive--;
        monstersAlive = Mathf.Max(monstersAlive, 0);

        currentWaveEnemyCount--;
        currentWaveEnemyCount = Mathf.Max(currentWaveEnemyCount, 0);

        Debug.Log($"☠️ 적 사망. 남은 그룹 적 수: {currentWaveEnemyCount}");

        if (currentWaveEnemyCount <= 0)
        {
            currentLevel++;
            Debug.Log($"🎉 레벨 {currentLevel - 1} 클리어! 다음 웨이브 시작");
            StartCoroutine(SpawnNextWave());
        }
    }

    private IEnumerator SpawnNextWave()
{
    if (isSpawningWave) yield break; // 🔒 중복 방지
    isSpawningWave = true;

    yield return new WaitForSeconds(2f);
    miniGameTrigger.SpawnEnemies(currentLevel);

    isSpawningWave = false;
}

    public void HandlePlayerDeath()
{
    if (!isMiniGameActive) return;

    Debug.Log("💀 플레이어 사망 - 미니게임 종료");
    isMiniGameActive = false;
    isGameOver = true; // ✅ 게임오버 상태 기록

    playerController.enabled = false;
    topDownShooter.enabled = false;

    ScoreManager.Instance?.StopScore();
    ScoreManager.Instance?.ShowFinalScore();

    if (miniGameUI != null)
        miniGameUI.SetActive(true);
}


    public void AddKill()
    {
        killCount++;
        ScoreManager.Instance?.AddKill(); // ✅ 점수 반영
    }
}
