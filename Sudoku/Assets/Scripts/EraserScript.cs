using UnityEngine;

public class EraserScript : MonoBehaviour
{
    [SerializeField] private GameObject puzzle;

    public void OnClick()
    {
        foreach (TileScript current in puzzle.GetComponentsInChildren<TileScript>())
        {
            if (current.isClicked)
            {
                current.EraseInput();
                FindFirstObjectByType<SudokuScript>().EraseNumber(gameObject);
            }
        }
    }

}
