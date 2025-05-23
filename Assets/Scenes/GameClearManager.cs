using UnityEngine;
using TMPro;

public class GameClearManager : MonoBehaviour
{
    public TextMeshProUGUI clearTimeText;
    public string gameSceneName = "MainGame";

    void Start()
    {
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StopTimer();
            clearTimeText.text = $"클리어 시간: {GameTimer.Instance.GetFormattedTime()}";
        }
        else
        {
            clearTimeText.text = "클리어 시간: 알 수 없음";
        }
    }

    public void Retry()
    {
        GameTimer.Instance?.ResetTimer(); // 다시 시작할 때 초기화
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
