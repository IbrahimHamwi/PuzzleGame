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
                            if (leftGem.Type == currentGem.Type && rightGem.Type == currentGem.Type)
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
                            if (upGem.Type == currentGem.Type && downGem.Type == currentGem.Type)
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
    }
}
