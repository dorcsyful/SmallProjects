using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Solver : MonoBehaviour
{
    public int[,] Grid;
    public int[,] Solution;
    private System.Random random;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Grid = new int[9, 9];
        Solution = new int[9, 9];
        random = new System.Random();
    }

    public void GenerateNewRNG()
    {
        random = new System.Random();
        Grid = new int[9, 9];
        Solution = new int[9, 9];
    }

    public bool CheckIfFinished()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (Grid[i, j] == 0)
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    public void EraseGrid(int row, int col)
    {
        Grid[row, col] = 0;
    }
    public bool CheckNumber(int row, int col, int value)
    {
        if (Solution[row, col] == value)
        {
            Grid[row, col] = value;
            return true;
        }
        return false;
    }
    public bool SolveSudoku()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (Solution[row, col] == 0) // Find an empty cell
                {
                    List<int> numbers = Enumerable.Range(1, 9).ToList();
                    Shuffle(numbers);

                    foreach (int number in numbers)
                    {
                        if (IsValid(row, col, number))
                        {
                            Solution[row, col] = number;

                            if (SolveSudoku())
                            {
                                return true;
                            }
                            else
                            {
                                Solution[row, col] = 0;
                            }
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }
    
    bool IsValid(int row, int col, int number)
    {
        for (int i = 0; i < 9; i++) if (Solution[row, i] == number) return false;

        for (int i = 0; i < 9; i++) if (Solution[i, col] == number) return false;

        int boxRowStart = row - row % 3;
        int boxColStart = col - col % 3;

        for (int i = boxRowStart; i < boxRowStart + 3; i++)
        {
            for (int j = boxColStart; j < boxColStart + 3; j++)
            {
                if (Solution[i, j] == number) return false;
            }
        }

        return true;
    }

    // Fisher-Yates shuffle algorithm (for randomizing the numbers)
    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public void RemoveCells(int cellsToRemove)
    {

        List<int> list = new List<int>();
        for(int i = 0; i < 81; i++) { list.Add(i); }
        Shuffle(list);
        for(int i = cellsToRemove; i > 0; i--) {list.RemoveAt(i);}
        
        
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (list.Contains(i * 9 + j))
                {
                    Grid[i, j] = 0;
                }
                else
                {
                    Grid[i, j] = Solution[i, j];
                }
            }
        }
    }
    
    
}
