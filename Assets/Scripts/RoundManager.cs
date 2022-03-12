using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;
    private bool endingRound = false;
    private Board board;
    public int currentScore;
    public float displayScore;
    public int scoreTarget1, scoreTarget2, scoreTarget3;//scoreTarget is the score needed to get one star, scoreTarget2 is the score needed to get two stars, scoreTarget3 is the score needed to get three stars

    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;
            uiManager.timerText.text = roundTime.ToString("0.0") + "s";
        }
        else
        {
            uiManager.timerText.text = "0" + "s";
            endingRound = true;
        }
        if (endingRound && board.currentState == Board.BoardState.move)
        {
            endingRound = false;
            WinCheck();
        }
        displayScore = Mathf.Lerp(displayScore, currentScore, Time.deltaTime * 5f);
        uiManager.scoreText.text = displayScore.ToString("0");
    }
    public void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);
        uiManager.winScore.text = currentScore.ToString("0");
        if (currentScore >= scoreTarget3)
        {
            uiManager.winText.text = "Congratulations! You earned 3 stars!";
            uiManager.winstars3.SetActive(true);
        }
        else if (currentScore >= scoreTarget2)
        {
            uiManager.winText.text = "Congratulations! You earned 2 stars!";
            uiManager.winstars2.SetActive(true);
        }
        else if (currentScore >= scoreTarget1)
        {
            uiManager.winText.text = "Congratulations! You earned 1 star!";
            uiManager.winstars1.SetActive(true);
        }
        else
        {
            uiManager.winText.text = "You didn't earn any stars. Try again!";
        }
    }
}