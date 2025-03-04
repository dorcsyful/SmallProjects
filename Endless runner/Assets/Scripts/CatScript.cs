using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatScript : MonoBehaviour
{
    
    private CatInput playerInput;
    
    
    private bool isJumping = false;
    private float jumpHeight = 5f;
    private float jumpDuration = 0.7f;
    void Start()
    {
        playerInput = new CatInput();
        playerInput.Enable();
        playerInput.Player.Disable();
        playerInput.UI.Enable();
        playerInput.Player.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        //GetComponent<Animator>().enabled = false;
        Debug.Log("CatScript.OnDisable");
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
       if(!isJumping) StartCoroutine(Jump());
    }
    
    IEnumerator Jump()
    {
        //GetComponent<Animator>().enabled = false;
        isJumping = true;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * jumpHeight;

        // Ascend
        while (elapsedTime < jumpDuration * 0.6f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / (jumpDuration / 2f));
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        // elapsedTime = 0f;
        // while (elapsedTime < jumpCooldown)
        // {
        //     transform.position = targetPosition;
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
        elapsedTime = 0f;

        // Descend
        while (elapsedTime < jumpDuration * 0.4f)
        {
            transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / (jumpDuration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //GetComponent<Animator>().enabled = true;

        transform.position = startPosition;
        isJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        FindFirstObjectByType<ObstacleScript>().enabled = false;
    }

    public void GameOver()
    {
        //GetComponent<Animator>().enabled = false;
        playerInput.Player.Disable();
        playerInput.UI.Enable();
    }

    public void PlayAgain()
    {
        //GetComponent<Animator>().enabled = true;
        playerInput.Player.Enable();
        playerInput.UI.Disable();
    }
}
