using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;
    private bool endingRound = false;
    private Board board;
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
            uiManager.timerText.text = "0";
            endingRound = true;
        }
        if (endingRound && board.currentState == Board.BoardState.move)
        {
            endingRound = false;
            WinCheck();
        }
    }
    public void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);
    }
}