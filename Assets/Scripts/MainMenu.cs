using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{

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

    public void EditName(string name) {
        Persistance.Instance.Name = name;
    }
}
