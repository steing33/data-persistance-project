using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Persistance : MonoBehaviour
{
    public event EventHandler OnDataLoaded;

    public static Persistance Instance;
    public string Name;
    public List<ScoreData> Highscores;
    public int _lowScore;
    public int _highScore;

    // C:\Users\steing\AppData\LocalLow\DefaultCompany\SimpleBreakout
    private const string SAVE_FILE_NAME = "/saveFile_persistence.json";
    private string _savePath;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // persist between scene loads
    }

    private void Start()
    {
        _savePath = Application.persistentDataPath + SAVE_FILE_NAME;
        loadFromPersistence();
        Highscores.Sort((a, b) => b.Score.CompareTo(a.Score));
        _lowScore = Highscores[9].Score;
        _highScore = Highscores[0].Score;

        OnDataLoaded?.Invoke(this, EventArgs.Empty);
    }

    public void Save()
    {
        this.Highscores.Sort((a, b) => b.Score.CompareTo(a.Score));
        this.Highscores = this.Highscores.GetRange(0, 10);  // only keep the top 10

        SaveData data = new()
        {
            LastUsedName = this.Name,
            HighScores = this.Highscores,
        };
        string json_str = JsonUtility.ToJson(data);
        File.WriteAllText(_savePath, json_str);
    }

    private void loadFromPersistence()
    {
        if (File.Exists(_savePath))
        {
            string json_str = File.ReadAllText(_savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json_str);
            this.Highscores = data.HighScores;
            this.Name = data.LastUsedName;
        }
        else
        {
            this.Name = "guest";
            this.Highscores = new List<ScoreData>(10);
            for (int i = 0; i < 10; i++)
            {
                this.Highscores.Add(new ScoreData() { Name = "guest", Score = 0 });
            }
        }
    }

    [System.Serializable]
    public struct SaveData
    {
        public string LastUsedName;
        public List<ScoreData> HighScores;
    }

    [System.Serializable]
    public struct ScoreData
    {
        public string Name;
        public int Score;
    }
}
