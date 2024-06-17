using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Persistance : MonoBehaviour
{
    public event EventHandler OnHighScoreChanged;

    public static Persistance Instance;
    public string Name;
    public List<ScoreData> Highscores;

    private const string SAVE_FILE_NAME = "/saveFile_persistence.json";
    private string _savePath;
    public int _lowScore;
    public int _highScore;

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
        Highscores = loadFromPersistence();
        Highscores.Sort((a, b) => b.Score.CompareTo(a.Score));
        int idx = Highscores.Count < 10 ? Highscores.Count-1 : 9;
        _lowScore = Highscores[idx].Score;
        _highScore = Highscores[0].Score;

        OnHighScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    private void saveToPersistence()
    {
        SaveData data = new()
        {
            HighScores = this.Highscores,
        };
        string json_str = JsonUtility.ToJson(data);
        File.WriteAllText(_savePath, json_str);
    }

    private List<ScoreData> loadFromPersistence() {
        if (File.Exists(_savePath) ) {
            string json_str = File.ReadAllText(_savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json_str);
            return data.HighScores;
        }
        return new List<ScoreData>(10);
    }

    [System.Serializable]
    public struct SaveData
    {
        public List<ScoreData> HighScores;
    }

    [System.Serializable]
    public struct ScoreData
    {
        public string Name;
        public int Score;
    }
}
