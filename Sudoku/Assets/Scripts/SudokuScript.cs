using UnityEngine;
using UnityEngine.UI;

public class SudokuScript : MonoBehaviour
{
    public GameObject parent;
    [SerializeField] private GameObject success;
    [SerializeField] private GameObject restart;
    private GameObject[,] grid;
    private Solver solver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solver = GetComponent<Solver>();
        solver.SolveSudoku();
        solver.RemoveCells(40);
        grid = new GameObject[9, 9];
        for (int row = 0; row < 9; row++)
        {
            Transform currentRow = parent.transform.GetChild(row);
            for (int col = 0; col < 9; col++)
            {
                grid[row, col] = currentRow.GetChild(col).gameObject;
                grid[row,col].GetComponentInChildren<TMPro.TMP_Text>().text = solver.Grid[row,col] == 0 ? "" : solver.Grid[row,col].ToString();
                grid[row, col].GetComponent<Button>().interactable = solver.Grid[row, col] == 0;
            }
        }
    }

    public void EraseNumber(GameObject tile)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row, col] == tile)
                {
                    solver.EraseGrid(row, col);
                }
            }
        }
    }
    public void UnselectButton()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                grid[row,col].GetComponentInChildren<TileScript>().UnSelect();
            }
        }
    }

    public void CheckFinished()
    {
        if (solver.CheckIfFinished())
        {
            success.SetActive(true);
            restart.SetActive(true);
        }
    }
    
    public bool IsCorrect(GameObject cell, int number)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row, col] == cell)
                {
                    return solver.CheckNumber(row, col, number);
                }
            }
        }
        return false;
    }

    public void Restart()
    {
        solver.GenerateNewRNG();
        solver.SolveSudoku();
        solver.RemoveCells(79);
        success.SetActive(false);
        restart.SetActive(false);
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                grid[row, col].GetComponent<TileScript>().Reset();
                grid[row,col].GetComponentInChildren<TMPro.TMP_Text>().text = solver.Grid[row,col] == 0 ? "" : solver.Grid[row,col].ToString();
                grid[row,col].GetComponentInChildren<TMPro.TMP_Text>().color = Color.black;
                grid[row, col].GetComponent<Button>().interactable = solver.Grid[row, col] == 0;
            }
        }

    }
}
