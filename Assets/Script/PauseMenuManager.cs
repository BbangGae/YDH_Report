using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Button continueButton;
    public Button quitButton;
    public Button retryButton; // ✅ 다시하기 버튼

    private bool isMenuOpen = false;

    private void Start()
    {
        menuPanel.SetActive(false);

        continueButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame); 
        retryButton.onClick.AddListener(RestartGame);       // ✅ 다시하기 추가
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuOpen)
                OpenMenu();
            else
                ResumeGame();
        }
    }

    void OpenMenu()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        isMenuOpen = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        isMenuOpen = false;
    }



    private void QuitGame()
    {
        Debug.Log("게임 종료 시도");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 모드에서 종료
#else
    Application.Quit(); // 실제 빌드된 게임에서 종료
#endif
    }


    void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex); // ✅ 현재 씬 다시 로드
    }
}
