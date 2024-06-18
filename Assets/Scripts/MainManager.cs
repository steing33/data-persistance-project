using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        if (Persistance.Instance)
            HighScoreText.text = $"{Persistance.Instance.Highscores[0].Name} highscore: {Persistance.Instance.Highscores[0].Score}";

        // Persistance.Instance.OnHighScoreChanged += Persistance_OnHighcoreChanged;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        // at any point hit esc to pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape pressed");

            updateHighScores();
            Persistance.Instance.Save();

            //TODO just quit for now -> Pause
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                updateHighScores();
                Persistance.Instance.Save();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void updateHighScores()
    {
        // better than the current low highscore
        if (m_Points > Persistance.Instance._lowScore)
        {
            // find position in sorted list
            int idx = 9;
            for (; idx >= 0; idx--)
                if (m_Points >= Persistance.Instance.Highscores[idx].Score)
                    break;
            
            Persistance.Instance.Highscores.Insert(idx,
                new Persistance.ScoreData()
                {
                    Name = Persistance.Instance.Name,
                    Score = m_Points
                });
        }
        // new highest highscore
        if (m_Points > Persistance.Instance._highScore)
        {
            Debug.Log($"new highscore acheived");
            
            Persistance.Instance._highScore = m_Points;
            HighScoreText.text = $"{Persistance.Instance.Name} highscore: {m_Points}";
        }
    }


    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
