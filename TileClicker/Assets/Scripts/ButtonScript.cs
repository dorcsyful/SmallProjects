using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonScript :MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Camera cam;
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float enlargeSize;
    [SerializeField] private float enlargeTime;

    private void Awake()
    {
        if(cam == null)
            cam = Camera.main;
    }

    public void OnClick()
    {
        StartCoroutine(Enlarge());
        Invoke(nameof(StartMove), enlargeTime);
    }

    private void StartMove()
    {
        if(!target)
            StartCoroutine(Move());
    }

    
    private bool IsButtonVisible(RectTransform button)
    {
        Vector3[] corners = new Vector3[4];
        button.GetWorldCorners(corners);
        foreach (Vector3 corner in corners)
        {
            Vector3 vpPos = cam.WorldToViewportPoint(corner);
            if (vpPos.x >= 0f && vpPos.x <= 1f && vpPos.y >= 0f && vpPos.y <= 1f && vpPos.z > 0f)
            {
                return true;
            }
        }
        return false;
    }
    
    IEnumerator Move()
    {
        if(!target)
        {
            var isButtonVisible = IsButtonVisible(rectTransform);
            while (isButtonVisible)
            {
                rectTransform.anchoredPosition += Vector2.right * (moveSpeed * Time.deltaTime);
                isButtonVisible = IsButtonVisible(rectTransform);
                yield return null;
            }

            gameObject.SetActive(false);
        }
        else
        {
            Vector2 basePosition = rectTransform.anchoredPosition;
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
    
    IEnumerator Enlarge()
    {
        float time = 0;
        float baseScale = transform.localScale.x;
        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(baseScale, enlargeSize, time / enlargeTime);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(enlargeSize,baseScale, time / enlargeTime);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            time += Time.deltaTime;
            yield return null;
        }
        
        time = 0;
    }
}
