using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField] Text bestScoreText;
    [SerializeField] InputField playerField;

    public string playerName = "";

    public string bestPlayerName;
    public int bestPlayerScore;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadInfoDataPlayer();
    }

    private void Start()
    {
        if (bestPlayerName != "")
        {
            bestScoreText.text = "Best Score : " + bestPlayerName + " : " + bestPlayerScore;
        }
    }

    public void SetBestPlayerScore(int score)
    {
        if (score > bestPlayerScore)
        {
            bestPlayerScore = score;
            bestPlayerName = playerName;
            SaveInfoDataPlayer();
            MainManager.Instance.bestScoreText.text = "Best Score : " + bestPlayerName + " : " + bestPlayerScore;
        }
    }

    public void StartNewGame()
    {
        if (playerField.text != "")
        {
            playerName = playerField.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Please enter a name!");
        }
    }

    public void EixtGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }

    public void GetPlayerName(string player)
    {
        player = playerName;
    }

    [System.Serializable]
    class SaveGameData
    {
        public string playerNameSaved;
        public int bestScoreSaved;
    }

    public void SaveInfoDataPlayer()
    {
        SaveGameData data = new SaveGameData();
        data.playerNameSaved = bestPlayerName;
        data.bestScoreSaved = bestPlayerScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInfoDataPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveGameData data = JsonUtility.FromJson<SaveGameData>(json);

            bestPlayerName = data.playerNameSaved;
            bestPlayerScore = data.bestScoreSaved;
        }
    }
}
