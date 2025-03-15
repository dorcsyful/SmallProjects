using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PuzzleScript : MonoBehaviour
{
    enum ECOLOR
    {
        RED,
        GREEN,
        BLUE,
        YELLOW,
    }
    [SerializeField] private GameObject redPieces;
    [SerializeField] private GameObject greenPieces;
    [SerializeField] private GameObject bluePieces;
    [SerializeField] private GameObject yellowPieces;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    public int stepScore;
    public float stepSpeed;
    public float speedTimer;
    public Color baseQuestionColor;
    
    private InputSystem_Actions inputActions;
    private Image[][] puzzlePieces;
    private Dictionary<ECOLOR, Color> colorIndex;
    private int row;
    private int col;
    private float lastRepeatRate = 2f;
    private GameObject toNull;
    private bool clicked;
    private int clickCount = 0;
    private int score = 0;
    private void Awake()
    {
        gameOverText.SetActive(false);
        colorIndex = new Dictionary<ECOLOR, Color>();
        colorIndex.Add(ECOLOR.RED, new Color(239f / 255f, 71f / 255f, 111f / 255f));
        colorIndex.Add(ECOLOR.GREEN, new Color(255f / 255f, 209f / 255f, 102f / 255f));
        colorIndex.Add(ECOLOR.BLUE, new Color(6f / 255f, 214f / 255f, 160f / 255f));
        colorIndex.Add(ECOLOR.YELLOW, new Color(17f / 255f, 138f / 255f, 178f / 255f));
        
        puzzlePieces = new Image[5][];
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            puzzlePieces[i] = new Image[5];
            for (int j = 0; j < puzzlePieces[i].Length; j++)
            {
                puzzlePieces[i][j] = gameObject.transform.GetChild(i).GetChild(j).GetComponent<Image>();
                var i1 = i;
                var j1 = j;
                puzzlePieces[i][j].gameObject.GetComponent<Button>().onClick.AddListener(delegate { PuzzlePress(i1, j1); });
            }
        }

        clicked = true;
        InvokeRepeating("NewColor", 0f, lastRepeatRate);
    }

    private void PuzzlePress(int i, int j)
    {
        puzzlePieces[col][row].color = Color.white;
        Debug.Log("Puzzle Press: " + i + ", " + j);
        if (puzzlePieces[i][j].color == colorIndex[ECOLOR.RED])
        {
            FillRow(redPieces, colorIndex[ECOLOR.RED]);
        }
        else if (puzzlePieces[i][j].color == colorIndex[ECOLOR.GREEN])
        {
            FillRow(greenPieces, colorIndex[ECOLOR.GREEN]);
        }
        else if (puzzlePieces[i][j].color == colorIndex[ECOLOR.BLUE])
        {
            FillRow(bluePieces, colorIndex[ECOLOR.BLUE]);
        }
        else if (puzzlePieces[i][j].color == colorIndex[ECOLOR.YELLOW])
        {
            FillRow(yellowPieces, colorIndex[ECOLOR.YELLOW]);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOverText.SetActive(true);
        Debug.Log("Game Over " + lastRepeatRate);
        CancelInvoke();
    }
    
    private void FillRow(GameObject go, Color color)
    {
        clicked = true;

        for (int i = 0; i < 4; i++)
        {
            if (go.transform.GetChild(i).GetComponent<Image>().color != color)
            {
                go.transform.GetChild(i).GetComponent<Image>().color = color;
                if (i == 3)
                {
                    toNull = go;
                    Invoke("ResetColor", 0.5f);
                }
                return;
            }
        }
        
    }

    private void ResetColor()
    {
        scoreText.text += stepScore;

        for (int i = 0; i < toNull.transform.childCount; i++)
        {
            toNull.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        
        score += stepScore;
        scoreText.text = score.ToString();
        toNull = null;
    }
    private void NewColor()
    {
        if (!clicked)
        {
            GameOver();
            return;
        }

        if (clickCount == 10)
        {
            clickCount = 0;
            CancelInvoke();
            lastRepeatRate *= speedTimer;
            Debug.Log("Speed up: " + lastRepeatRate);
            InvokeRepeating("NewColor", 2f, lastRepeatRate);

        }
        clicked = false;
        clickCount++;
        puzzlePieces[col][row].color = Color.white;
        var newRow = Random.Range(0, 4);
        var newCol = Random.Range(0, 4);
        puzzlePieces[newCol][newRow].color = colorIndex[(ECOLOR)(Random.Range(0, 40) / 10f)];
        col = newCol;
        row = newRow;
    }

    public void Reset()
    {
        score = 0;
        scoreText.text = score.ToString();
        lastRepeatRate = 2f;
        clickCount = 0;
        clicked = false;
        col = -1;
        row = -1;
    }
}
