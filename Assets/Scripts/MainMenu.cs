using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
