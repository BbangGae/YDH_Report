using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton;

    private bool isMenuOpen = false;

    private void Start()
    {
        menuPanel.SetActive(false);

        continueButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
    }

   private void Update()
{

    // 일반적인 ESC 키 처리 (도움말이 닫혀 있을 때만)
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

    void OpenSettings()
{
    // 메뉴 닫고 설정 열기
    menuPanel.SetActive(false);
    isMenuOpen = false;
    Time.timeScale = 1f;

    SettingsMenuManager settings = FindObjectOfType<SettingsMenuManager>();
    if (settings != null)
    {
        settings.OpenSettingsPanel();
    }
    else
    {
        Debug.LogWarning("⚠️ SettingsMenuManager를 찾을 수 없습니다.");
    }
}

 void OpenHelp()
{
    // ✅ 메뉴를 즉시 닫음 (ResumeGame() 대신 수동 처리)
    menuPanel.SetActive(false);
    isMenuOpen = false;
    Time.timeScale = 1f;

}



    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
