using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }
    public void FindAllMatches()
    {
        currentMatches.Clear();//clear the list of current matches
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                Gem currentGem = board.allGems[i, j];
                if (currentGem != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        Gem leftGem = board.allGems[i - 1, j];
                        Gem rightGem = board.allGems[i + 1, j];
                        if (leftGem != null && rightGem != null)
                        {
                            if (leftGem.Type == currentGem.Type && rightGem.Type == currentGem.Type && currentGem.Type != Gem.GemType.Stone)
                            {
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;
                                currentGem.isMatched = true;

                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                                currentMatches.Add(currentGem);
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        Gem upGem = board.allGems[i, j + 1];
                        Gem downGem = board.allGems[i, j - 1];
                        if (upGem != null && downGem != null)
                        {
                            if (upGem.Type == currentGem.Type && downGem.Type == currentGem.Type && currentGem.Type != Gem.GemType.Stone)
                            {
                                upGem.isMatched = true;
                                downGem.isMatched = true;
                                currentGem.isMatched = true;

                                currentMatches.Add(upGem);
                                currentMatches.Add(downGem);
                                currentMatches.Add(currentGem);
                            }
                        }
                    }
                }
            }
        }
        if (currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }
        CheckForBombs();
    }
    public void CheckForBombs()
    {
        for (int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];

            int x = gem.posIndex.x;
            int y = gem.posIndex.y;
            if (gem.posIndex.x > 0)
            {
                if (board.allGems[x - 1, y] != null)
                {
                    if (board.allGems[x - 1, y].Type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y), board.allGems[x - 1, y]);
                    }
                }
            }
            if (gem.posIndex.x < board.width - 1)
            {
                if (board.allGems[x + 1, y] != null)
                {
                    if (board.allGems[x + 1, y].Type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), board.allGems[x + 1, y]);
                    }
                }
            }
            if (gem.posIndex.y > 0)
            {
                if (board.allGems[x, y - 1] != null)
                {
                    if (board.allGems[x, y - 1].Type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), board.allGems[x, y - 1]);
                    }
                }
            }
            if (gem.posIndex.y < board.height - 1)
            {
                if (board.allGems[x, y + 1] != null)
                {
                    if (board.allGems[x, y + 1].Type == Gem.GemType.Bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1), board.allGems[x, y + 1]);
                    }
                }
            }
        }
    }
    public void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        for (int x = bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for (int y = bombPos.y - theBomb.blastSize; y <= bombPos.y + theBomb.blastSize; y++)
            {
                if (x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if (board.allGems[x, y] != null)
                    {
                        board.allGems[x, y].isMatched = true;
                        currentMatches.Add(board.allGems[x, y]);
                    }
                }
            }
        }
        currentMatches = currentMatches.Distinct().ToList();//remove duplicates
    }
}

