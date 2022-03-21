using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText, timerText, winScore, winText;
    public GameObject winstars1, winstars2, winstars3;//winstars is one star, winstars2 is two stars, winstars3 is three stars
    public GameObject roundOverScreen;
    private Board theBoard;
    public string LevelSelect;
    public GameObject pauseScreen;
    private void Awake()
    {
        theBoard = FindObjectOfType<Board>();
    }

    void Start()
    {
        winstars1.SetActive(false);
        winstars2.SetActive(false);
        winstars3.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if (!pauseScreen.activeInHierarchy)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
    }
    public void ShuffleBoard()
    {
        theBoard.ShuffleBoard();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToLevelSelect()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(LevelSelect);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
