using UnityEngine;
using System.IO;

[System.Serializable]
public class ScoreData
{
    public int highScore = 0;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    private ScoreData scoreData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "scoreData.json");
            LoadScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetHighScore()
    {
        return scoreData?.highScore ?? 0;
    }

    public void TrySetNewHighScore(int newScore)
    {
        if (newScore > scoreData.highScore)
        {
            scoreData.highScore = newScore;
            SaveScore();
            Debug.Log($"\ud83c\udfc6 \uc0c8\ub85c\uc6b4 \ucd5c\uace0\uc810\uc218 \uacb0\uacfc: {newScore}");
        }
    }

    private void SaveScore()
    {
        string json = JsonUtility.ToJson(scoreData, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
        }
        else
        {
            scoreData = new ScoreData();
            SaveScore();
        }
    }
}
