using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    [HideInInspector]
    public Board board;

    private Vector2 firstTouchPosition, finalTouchPosition;

    private bool mousePressed;
    private float swipeAngle = 0f;
    private Gem otherGem;
    public enum GemType { Red, Blue, Green, Yellow, Purple, Bomb };
    public GemType Type;
    public bool isMatched;

    private Vector2Int previousPos;
    public GameObject destroyEffect;
    public int blastSize = 2;//how many gems to destroy when bomb is destroyed
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0);
            board.allGems[posIndex.x, posIndex.y] = this;
        }
        if (mousePressed && Input.GetMouseButtonUp(0) && board.roundManager.roundTime > 0)
        {
            mousePressed = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }
    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }
    private void OnMouseDown()
    {
        if (board.currentState == Board.BoardState.move && board.roundManager.roundTime > 0)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressed = true;
        }
    }
    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        if (Vector3.Distance(finalTouchPosition, firstTouchPosition) > 0.5f)
        {
            MovePieces();
        }
    }
    private void MovePieces()
    {
        previousPos = posIndex;
        if (swipeAngle > -45 && swipeAngle <= 45 && posIndex.x < board.width - 1)
        {
            //Right
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            board.allGems[posIndex.x + 1, posIndex.y] = this;
            board.allGems[posIndex.x, posIndex.y] = otherGem;
            posIndex.x++;
            otherGem.posIndex.x--;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            //Up
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            board.allGems[posIndex.x, posIndex.y + 1] = this;
            board.allGems[posIndex.x, posIndex.y] = otherGem;
            posIndex.y++;
            otherGem.posIndex.y--;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && posIndex.x > 0)
        {
            //Left
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            board.allGems[posIndex.x - 1, posIndex.y] = this;
            board.allGems[posIndex.x, posIndex.y] = otherGem;
            posIndex.x--;
            otherGem.posIndex.x++;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            //Down
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            board.allGems[posIndex.x, posIndex.y - 1] = this;
            board.allGems[posIndex.x, posIndex.y] = otherGem;
            posIndex.y--;
            otherGem.posIndex.y++;
        }
        StartCoroutine(CheckMoveCo());
    }
    public IEnumerator CheckMoveCo()
    {
        board.currentState = Board.BoardState.wait;//wait for the other gem to move
        yield return new WaitForSeconds(0.5f);
        board.matchFind.FindAllMatches();//check for matches
        if (otherGem != null)
        {
            if (!isMatched && !otherGem.isMatched)
            {
                otherGem.posIndex = posIndex;
                posIndex = previousPos;

                board.allGems[posIndex.x, posIndex.y] = this;
                board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

                yield return new WaitForSeconds(0.5f);//wait for the other gem to move
                board.currentState = Board.BoardState.move;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
