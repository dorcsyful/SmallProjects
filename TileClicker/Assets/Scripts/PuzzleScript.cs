using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public enum ECOLOR
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    DEFAULT
}

public class PuzzleScript : MonoBehaviour
{

    [SerializeField] private GameObject redPieces;
    [SerializeField] private GameObject greenPieces;
    [SerializeField] private GameObject bluePieces;
    [SerializeField] private GameObject yellowPieces;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private TMPro.TMP_Text highScoreText;
    public int stepScore;
    public float stepSpeed;
    public float speedTimer;
    public Color baseQuestionColor;

    private InputSystem_Actions inputActions;
    private TileScript[][] puzzlePieces;
    private Dictionary<ECOLOR, Color> colorIndex;
    private int row;
    private int col;
    private float lastRepeatRate = 2f;
    private GameObject toNull;
    private bool clicked;
    private int clickCount;
    private int score;

    private void Awake()
    {
        Saving.Load();
        highScoreText.text = Saving.savedGames.highScore.ToString();
        gameOverText.SetActive(false);
        colorIndex = new Dictionary<ECOLOR, Color>
        {
            { ECOLOR.RED, new Color(239f / 255f, 71f / 255f, 111f / 255f) },
            { ECOLOR.YELLOW, new Color(255f / 255f, 209f / 255f, 102f / 255f) },
            { ECOLOR.GREEN, new Color(6f / 255f, 214f / 255f, 160f / 255f) },
            { ECOLOR.BLUE, new Color(17f / 255f, 138f / 255f, 178f / 255f) }
        };

        puzzlePieces = new TileScript[5][];
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            puzzlePieces[i] = new TileScript[5];
            for (int j = 0; j < puzzlePieces[i].Length; j++)
            {
                puzzlePieces[i][j] = gameObject.transform.GetChild(i).GetChild(j).GetComponent<TileScript>();
                var i1 = i;
                var j1 = j;
                puzzlePieces[i][j].gameObject.GetComponent<Button>().onClick
                    .AddListener(delegate { PuzzlePress(i1, j1); });
            }
        }

        clicked = true;
        InvokeRepeating(nameof(NewColor), 0f, lastRepeatRate);
    }

    public int GetScore()
    {
        return score;
    }

private void PuzzlePress(int i, int j)
    {
        ECOLOR colorCode = puzzlePieces[i][j].GetColorCode();
        switch (colorCode)
        {
            case ECOLOR.RED:
                FillRow(redPieces, colorIndex[ECOLOR.RED]);
                break;
            case ECOLOR.GREEN:
                FillRow(greenPieces, colorIndex[ECOLOR.GREEN]);
                break;
            case ECOLOR.BLUE:
                FillRow(bluePieces, colorIndex[ECOLOR.BLUE]);
                break;  
            case ECOLOR.YELLOW:
                FillRow(yellowPieces, colorIndex[ECOLOR.YELLOW]);
                break;
            case ECOLOR.DEFAULT:
                GameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        puzzlePieces[col][row].ResetColor();

    }
    
    public void AddScore()
    {
        score += stepScore;
        scoreText.text = score.ToString();
        FindAnyObjectByType<PointCounter>().PointAdded();
    }
    
    private void GameOver()
    {
        Debug.Log("Game Over " + lastRepeatRate);
        gameOverText.SetActive(true);
        Saving.OverrideIfHigher(score);
        score = 0;
        CancelInvoke();
    }
    
    private void FillRow(GameObject go, Color color)
    {
        clicked = true;

        go.GetComponent<CounterScript>().EnlargeSection(color);
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
            InvokeRepeating(nameof(NewColor), 2f, lastRepeatRate);

        }
        clicked = false;
        clickCount++;
        puzzlePieces[col][row].ResetColor();
        var newRow = Random.Range(0, 4);
        var newCol = Random.Range(0, 4);
        ECOLOR ecolor = (ECOLOR)(Random.Range(0, 40) / 10f);
        puzzlePieces[newCol][newRow].Activate(colorIndex[ecolor], ecolor);
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
