using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour
{
    public bool isClicked;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    public void InputNumber(int number)
    {
        text.text = number.ToString();

        if (!FindFirstObjectByType<SudokuScript>().IsCorrect(gameObject, number))
        {
            isClicked = !isClicked;
            FindFirstObjectByType<SudokuScript>().UnselectButton();

            ShowIncorrect();
            
            return;
        }
        text.color = Color.blue;
        OnClick();
        FindFirstObjectByType<SudokuScript>().CheckFinished();
    }

    public void EraseInput()
    {
        text.text = string.Empty;
        image.color = Color.white;
        isClicked = false;
    }
    
    public void UnSelect()
    {
        if (image.color != new Color(0.5f, 0, 0, 0.8f) && image.color != new Color(0.8f, 0, 0, 0.8f)) image.color = Color.white;
        else image.color = new Color(0.8f, 0, 0, 0.8f);
    }

    public void ShowIncorrect()
    {
        image.color = isClicked ? new Color(0.5f, 0, 0, 0.8f) : new Color(0.8f,0,0,0.8f);
        text.color = Color.white;
    }
    
    public void OnClick()
    {
        isClicked = !isClicked;
        FindFirstObjectByType<SudokuScript>().UnselectButton();
        if(image.color != new Color(0.8f,0,0,0.8f))
            image.color = isClicked ? new Color(1,1,0.8f,0.5f) : Color.white;
        else
        {
            image.color = new Color(0.5f, 0, 0, 0.8f);
        }
        
    }

    public void Reset()
    {
        text.text = string.Empty;
        image.color = Color.white;
        isClicked = false;
    }
}
