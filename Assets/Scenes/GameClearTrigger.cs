using UnityEngine;

public class GameClearTrigger : MonoBehaviour
{
    public string clearSceneName = "GameClear"; // 전환할 씬 이름

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎉 게임 클리어!");

            if (FadeManager.Instance != null)
                FadeManager.Instance.FadeToScene(clearSceneName);
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(clearSceneName);
        }
    }
}
