using System.Collections;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI pointCounter;
    public float enlargeTime = 0.01f;
    public float enlargeSize = 1.25f;
    private int targetPoints;
    private int currentPoints;
    private bool isEnlarging;
    private bool isCounting;
    
    private void Start()
    {
        targetPoints = 0;
        currentPoints = 0;
        isEnlarging = false;
    }
    
    public void PointAdded()
    {
        targetPoints = FindAnyObjectByType<PuzzleScript>().GetScore();
        if (!isEnlarging)
        {
            StartCoroutine(Enlarge());
        }
        else
        {
            transform.localScale *= 0.7f;
        }
    }

    IEnumerator AddNumber()
    {
        isCounting = true;
        while (currentPoints < targetPoints)
        {
            currentPoints++;
            pointCounter.text = currentPoints.ToString();
            yield return null;
        }
        isCounting = false;
    }
    
    IEnumerator Enlarge()
    {
        if(!isCounting) StartCoroutine(AddNumber());

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
            float currentScale = Mathf.Lerp(enlargeSize, baseScale, time / enlargeTime);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            time += Time.deltaTime;
            yield return null;
        }

    }
}
