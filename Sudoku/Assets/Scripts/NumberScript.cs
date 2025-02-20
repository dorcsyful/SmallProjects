using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberScript : MonoBehaviour
{
    [SerializeField] private GameObject puzzle;
    public int number;
    public void OnClick()
    {
        foreach (TileScript current in puzzle.GetComponentsInChildren<TileScript>())
        {
            if (current.isClicked)
            {
                current.gameObject.GetComponent<TileScript>().InputNumber(number);
            }
        }
    }

}
