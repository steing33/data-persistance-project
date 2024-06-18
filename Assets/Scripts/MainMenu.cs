using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public Text _highscoreText;
    public TMP_InputField _nameInputField;

    private void Start()
    {
        Persistance.Instance.OnHighScoreChanged += Persistance_OnHighScoreChanged;
        Persistance.Instance.OnNameLoad += Persistance_OnNameLoad;
        _nameInputField.text = Persistance.Instance != null ? Persistance.Instance.Name : "guest";
    }

    public void StartApp()
    {
        Debug.Log("loading main scene...");
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Debug.Log("exiting application...");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void EditName(string name)
    {
        Persistance.Instance.Name = name;
    }

    private void updateHighScore()
    {
        string name = Persistance.Instance.Highscores[0].Name;
        int score = Persistance.Instance.Highscores[0].Score;

        _highscoreText.text = $"HIGHSCORE - {name}: {score}";
        Debug.Log($"new highscore -- {name}: {score}");
    }

    private void Persistance_OnHighScoreChanged(object sender, EventArgs e)
    {
        Debug.Log("on high score changed - main menu");
        updateHighScore();
    }

    private void Persistance_OnNameLoad(object sender, EventArgs e)
    {
        Debug.Log("on name load - main menu");
        _nameInputField.text = Persistance.Instance.Name;
    }
}
