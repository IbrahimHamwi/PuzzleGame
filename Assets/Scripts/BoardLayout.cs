using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLayout : MonoBehaviour
{
    public LayoutRow[] allRows;
    public Gem[,] GetLayout()
    {
        Gem[,] theLayout = new Gem[allRows[0].gemsInRow.Length, allRows.Length];
        for (int x = 0; x < allRows.Length; x++)
        {
            for (int y = 0; y < allRows[x].gemsInRow.Length; y++)
            {
                if (x < theLayout.GetLength(0))
                {
                    if (allRows[y].gemsInRow[x] != null)
                    {
                        theLayout[x, allRows.Length - y - 1] = allRows[y].gemsInRow[x];
                    }
                }
            }
        }
        return theLayout;
    }
}
[System.Serializable]
public class LayoutRow
{
    public Gem[] gemsInRow;
}
