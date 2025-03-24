using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private RectTransform menu;
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform start;
    public float moveSpeed = 10000f;
    public Vector2 moveDirection = new Vector2(1, 0); // Moves right by default

    private int lastButtonIndex = 0;
    
    public void PlayGame()
    {
        StartCoroutine(CheckIfActive());
    }

    IEnumerator CheckIfActive()
    {
        while (true)
        {
            if (!start.gameObject.activeSelf)
            {
                SceneManager.LoadScene("Game");
                break;
            }
            yield return null;
        }
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
