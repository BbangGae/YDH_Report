using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("⏱ UI")]
    public TextMeshProUGUI timerText;  // 게임 중 화면 상단에 표시할 텍스트

    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            // UI에 실시간 시간 표시
            if (timerText != null)
                timerText.text = $"{GetFormattedTime()}";
        }
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public float GetElapsedTime() => elapsedTime;

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;

        // UI 초기화도 함께
        if (timerText != null)
            timerText.text = "00:00";
    }
}
