using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width, height;
    public Gem[] gems;
    public Gem[,] allGems;
    public float gemSpeed;

    [HideInInspector]
    public MatchFinder matchFind;
    public enum BoardState { wait, move }
    public BoardState currentState = BoardState.move;

    public GameObject bgTilePrefab;
    public Gem bomb;
    public float bombChance = 2f;
    [HideInInspector] public RoundManager roundManager;
    private float bonusMultiplier;//how many times the score increases when a gem is destroyed within certain time
    public float bonusAmount = .5f;
    private BoardLayout boardLayout;
    private Gem[,] layoutStore;
    private void Awake()
    {
        matchFind = FindObjectOfType<MatchFinder>();
        roundManager = FindObjectOfType<RoundManager>();
        boardLayout = FindObjectOfType<BoardLayout>();
    }
    void Start()
    {
        allGems = new Gem[width, height];
        layoutStore = new Gem[width, height];
        Setup();
    }
    private void Update()
    {
        // matchFind.FindAllMatches();
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShuffleBoard();
        }
    }
    public void ShuffleBoard()//shuffle the board without matches
    {
        if (currentState != BoardState.wait)
        {
            currentState = BoardState.wait;
            List<Gem> gemsFromBoard = new List<Gem>();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    gemsFromBoard.Add(allGems[i, j]);
                    allGems[i, j] = null;
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int gemToUse = Random.Range(0, gemsFromBoard.Count);
                    int iterations = 0;
                    while (MatchesAt(new Vector2Int(i, j), gemsFromBoard[gemToUse]) && iterations < 100 && gemsFromBoard.Count > 0)
                    {
                        gemToUse = Random.Range(0, gemsFromBoard.Count);
                        iterations++;
                    }
                    gemsFromBoard[gemToUse].SetupGem(new Vector2Int(i, j), this);
                    allGems[i, j] = gemsFromBoard[gemToUse];
                    gemsFromBoard.RemoveAt(gemToUse);
                }
            }
            StartCoroutine(FillBoardCo());
        }
    }

    void Setup()
    {
        if (boardLayout != null)
        {
            layoutStore = boardLayout.GetLayout();
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bgTile = Instantiate(bgTilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                bgTile.transform.parent = gameObject.transform;
                bgTile.name = "(" + i + "," + j + ")";

                if (layoutStore[i, j] != null)
                {
                    SpawnGem(new Vector2Int(i, j), layoutStore[i, j]);
                }
                else
                {

                    int gemToUse = Random.Range(0, gems.Length);

                    int iterations = 0;
                    while (MatchesAt(new Vector2Int(i, j), gems[gemToUse]) && iterations < 100)
                    {
                        gemToUse = Random.Range(0, gems.Length);
                        iterations++;
                    }
                    SpawnGem(new Vector2Int(i, j), gems[gemToUse]);
                }
            }
        }

    }
    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            gemToSpawn = bomb;
        }
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y + height, 0), Quaternion.identity);
        gem.transform.parent = gameObject.transform;
        gem.name = "Gem (" + pos.x + "," + pos.y + ")";
        allGems[pos.x, pos.y] = gem;

        gem.SetupGem(pos, this);
    }
    bool MatchesAt(Vector2Int posToCheck, Gem gemToCheck)
    {
        if (posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].Type == gemToCheck.Type && allGems[posToCheck.x - 2, posToCheck.y].Type == gemToCheck.Type)
            {
                return true;
            }
        }
        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].Type == gemToCheck.Type && allGems[posToCheck.x, posToCheck.y - 2].Type == gemToCheck.Type)
            {
                return true;
            }
        }

        return false;
    }
    private void DestroyMatchedGemAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y].isMatched)
        {
            if (allGems[pos.x, pos.y].Type == Gem.GemType.Stone)
            {
                SFXManager.instance.PlayExplode();
            }
            else if (allGems[pos.x, pos.y].Type == Gem.GemType.Bomb)
            {
                SFXManager.instance.PlayExplode();
            }
            else
            {
                SFXManager.instance.PlayGemBreak();
            }
            //Destroy(allGems[pos.x, pos.y].gameObject);
            Instantiate(allGems[pos.x, pos.y].destroyEffect, allGems[pos.x, pos.y].transform.position, Quaternion.identity);
            Destroy(allGems[pos.x, pos.y].gameObject);
            allGems[pos.x, pos.y] = null;//nullify the gem
        }
    }
    public void DestroyMatches()
    {
        for (int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if (matchFind.currentMatches[i] != null)
            {
                DestroyMatchedGemAt(matchFind.currentMatches[i].posIndex);//destroy the gem
                ScoreCheck(matchFind.currentMatches[i]);
            }
        }
        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.2f);
        int nullCounter = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allGems[i, j].posIndex.y -= nullCounter;
                    allGems[i, j - nullCounter] = allGems[i, j];
                    allGems[i, j] = null;
                }
            }
            nullCounter = 0;
        }
        StartCoroutine(FillBoardCo());
    }
    private IEnumerator FillBoardCo()//fill the board with new gems
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        yield return new WaitForSeconds(.5f);
        matchFind.FindAllMatches(); //check for new matches
        if (matchFind.currentMatches.Count > 0)
        {
            bonusMultiplier++;//increase the multiplier
            print("bonus multiplier: " + bonusMultiplier);
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            bonusMultiplier = 0f;//reset the multiplier
            yield return new WaitForSeconds(.5f);
            currentState = BoardState.move;
        }
    }
    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allGems[i, j] == null)
                {
                    int gemToUse = Random.Range(0, gems.Length);
                    SpawnGem(new Vector2Int(i, j), gems[gemToUse]);
                }
            }
        }
        CheckMisplacedGems();
    }
    private void CheckMisplacedGems()
    {
        List<Gem> foundGems = new List<Gem>();
        foundGems.AddRange(FindObjectsOfType<Gem>());
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (foundGems.Contains(allGems[i, j]))
                {
                    foundGems.Remove(allGems[i, j]);
                }
            }
        }
        foreach (Gem g in foundGems)
        {
            Destroy(g.gameObject);
        }
    }
    public void ScoreCheck(Gem gemToCheck)
    {
        roundManager.currentScore += gemToCheck.scoreValue;
        if (bonusMultiplier > 0)
        {
            float bonusToAdd = gemToCheck.scoreValue * bonusMultiplier * bonusAmount;
            roundManager.currentScore += Mathf.RoundToInt(bonusToAdd);
        }
    }
}

