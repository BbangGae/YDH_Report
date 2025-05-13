using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;         // 실시간 점수
    public TextMeshProUGUI finalScoreText;    // 게임 종료 시 점수

    public TextMeshProUGUI highScoreText;

    private float timeSurvived = 0f;
    private int killCount = 0;
    private bool gameActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 초기 텍스트 상태 설정
        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (finalScoreText != null) finalScoreText.text = "Final Score: 0";
    }

    private void Update()
    {
        if (!gameActive) return;

        timeSurvived += Time.deltaTime;
        UpdateScoreUI();
    }

    public void StartScore()
    {
        timeSurvived = 0f;
        killCount = 0;
        gameActive = true;

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            UpdateScoreUI(); // 초기화 반영
        }

         // ✅ 하이스코어 출력
    if (highScoreText != null)
    {
        int highScore = SaveManager.Instance?.GetHighScore() ?? 0;
        highScoreText.text = $"High Score: {highScore}";
    }
    }

    public void StopScore()
    {
        gameActive = false;
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    }

    public void AddKill()
    {
        killCount++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            int score = CalculateScore();
            scoreText.text = $"Score: {score}";
        }
    }

    public void ShowFinalScore()
{
    int final = CalculateScore();

    if (finalScoreText != null)
        finalScoreText.text = $"Final Score: {final}";

    SaveManager.Instance?.TrySetNewHighScore(final);

    // ✅ 하이스코어 다시 표시 (갱신됐을 수도 있으니)
    if (highScoreText != null)
    {
        int high = SaveManager.Instance?.GetHighScore() ?? 0;
        highScoreText.text = $"High Score: {high}";
    }
}


    private int CalculateScore()
    {
        return Mathf.RoundToInt(timeSurvived * 10f) + (killCount * 100);
    }
}
