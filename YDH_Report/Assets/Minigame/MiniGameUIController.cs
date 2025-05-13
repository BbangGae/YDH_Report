using UnityEngine;

public class MiniGameUIController : MonoBehaviour
{
    public GameObject miniGameUIPanel;         // 미니게임 UI 패널
    public PlayerController playerController;  // 플레이어 컨트롤러
    public MiniGameManager miniGameManager;    // 미니게임 매니저 참조

    public MiniGameTrigger miniGameTrigger; // 인스펙터 연결 필요

    public Transform miniGameStartPoint; // 시작 위치 (인스펙터에서 지정)
    public Transform returnPointAfterExit; 



   public void OnStartMiniGame()
{
    if (miniGameManager.isMiniGameActive) return;

    Debug.Log("미니게임 시작!");
    miniGameUIPanel?.SetActive(false);

    // ✅ 시작 위치로 이동
    if (miniGameStartPoint != null)
        playerController.transform.position = miniGameStartPoint.position;

    ResetMiniGameState();
}




  public void OnExitMiniGame()
{
    Debug.Log("미니게임 종료");

    bool wasGameOver = miniGameManager.isGameOver;
    bool wasMovementBlocked = !playerController.canMove;
    SoundManager.Instance.PlayBGM(SoundManager.Instance.fieldBGM);
    if (wasGameOver && !miniGameManager.isMiniGameActive)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        miniGameManager.topDownShooter.enabled = false;
        miniGameManager.playerController.enabled = true;

        miniGameManager.isGameOver = false;
    }

    if (wasMovementBlocked)
    {
        playerController.canMove = true;
    }

    // ✅ 종료 후 지정된 위치로 이동
    if (returnPointAfterExit != null)
        playerController.transform.position = returnPointAfterExit.position;

    miniGameManager.ExitMiniGame();

    // ✅ UI는 마지막에 비활성화
    if (miniGameUIPanel != null)
        miniGameUIPanel.SetActive(false);
        
         if (miniGameManager.healthBarUI != null) miniGameManager.healthBarUI.SetActive(false);
}





    private void OnEnable()
    {
        // UI가 활성화될 때는 플레이어 이동을 제한
        if (playerController != null && !miniGameManager.isMiniGameActive)
        {
            playerController.canMove = false;  // 미니게임 UI가 활성화되면 플레이어 이동 비활성화
        }
    }

    private void ResetPlayerStats()
{
    // 체력 복구
    CharacterStats stats = playerController.GetComponent<CharacterStats>();
    if (stats != null)
    {
        stats.currentHealth = stats.maxHealth;
        Debug.Log("❤️ 체력 초기화 완료");
    }

    // 공격력/버프 상태 복구
    TopDownShooter shooter = playerController.GetComponent<TopDownShooter>();
    if (shooter != null)
    {
        shooter.baseDamage = 10;
        shooter.SendMessage("EndShotgun", SendMessageOptions.DontRequireReceiver); // 안전하게 shotgun 종료
        shooter.SendMessage("EndStatBoost", SendMessageOptions.DontRequireReceiver); // STup 종료용 함수가 있다면

        // 또는 직접 설정
        shooter.Invoke("EndShotgun", 0f);
        shooter.StopAllCoroutines(); 
        shooter.ResetPowerups();     
    }
}
private void ResetMiniGameState()
{
    Debug.Log("🧼 미니게임 상태 초기화 시작");

    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (var enemy in enemies)
    {
        Destroy(enemy);
    }

    ResetPlayerStats();

    playerController.canMove = false;


    // ✅ 이 함수만 호출 (내부에서 코루틴으로 웨이브 생성)
    miniGameManager.EnterMiniGame();

    miniGameManager.isGameOver = false;

    Debug.Log("✅ 미니게임 상태 초기화 완료");
}


}
