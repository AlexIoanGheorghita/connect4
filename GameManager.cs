using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        // Starting a game means loading a scene.
        // Because we have only one scene we want to load it.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void StopGame()
    {
        // The following is a preprocessor directive.
        // It tells the compiler to compile only if the statement is met (meaning, only if we are in the Unity Editor).
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
