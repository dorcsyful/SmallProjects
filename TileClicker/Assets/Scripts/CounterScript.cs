using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float enlargeTime;
    [SerializeField] private float enlargeSize;
    [SerializeField] private float moveTime = 1f;
    [SerializeField] private Transform moveObject;
    private Color color;
    
    
    public void EnlargeSection(Color setColor)
    {
        color = setColor;
        int section = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Image>().color == Color.white)
            {
                section = i;
                break;
            }
        }
        StartCoroutine(Enlarge(transform.GetChild(section)));
        if (section == 3)
        {
            Invoke(nameof(StartMove), enlargeTime * 2f);
        }
    }

    private void StartMove()
    {
        StartCoroutine(CombineAllChildren());
    }

    IEnumerator MoveToTarget()
    {
        moveObject.gameObject.SetActive(true);
        float time = 0;
        while (time < moveTime)
        {
            moveObject.position = Vector3.Lerp(transform.position, target.position, time / moveTime);
            moveObject.rotation *= Quaternion.Euler(0, 0, 720 * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        FindAnyObjectByType<PuzzleScript>().AddScore();
        moveObject.gameObject.SetActive(false);
    }
    
    IEnumerator CombineAllChildren()
    {
        float time = 0;
        float baseScale = transform.GetChild(0).localScale.x;
        Vector3[] basePosition = { transform.GetChild(0).position, transform.GetChild(1).position, transform.GetChild(2).position, transform.GetChild(3).position };
        
        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(baseScale, enlargeSize, time / enlargeTime);
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector3(currentScale, currentScale, currentScale);
                transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(Color.white, color, time / enlargeTime);
                transform.GetChild(i).position = Vector3.Lerp(basePosition[i], transform.position, time / enlargeTime);
                
            }
            time += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(MoveToTarget());
        time = 0;

        transform.GetChild(0).GetComponent<Image>().color = color;
        transform.GetChild(1).GetComponent<Image>().color = color;
        transform.GetChild(2).GetComponent<Image>().color = color;
        transform.GetChild(3).GetComponent<Image>().color = color;
        
        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(enlargeSize, baseScale, time / enlargeTime);
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector3(currentScale, currentScale, currentScale);
                transform.GetChild(i).GetComponent<Image>().color = Color.Lerp(color, Color.white, time / enlargeTime);

                transform.GetChild(i).position = Vector3.Lerp(transform.position, basePosition[i], time / enlargeTime);

            }            
            time += Time.deltaTime;
            yield return null;
        }
        
        transform.GetChild(0).GetComponent<Image>().color = Color.white;
        transform.GetChild(1).GetComponent<Image>().color = Color.white;
        transform.GetChild(2).GetComponent<Image>().color = Color.white;
        transform.GetChild(3).GetComponent<Image>().color = Color.white;
    }
    
    IEnumerator Enlarge(Transform trans)
    {
        float time = 0;
        float baseScale = trans.transform.localScale.x;
        Image component = trans.GetComponent<Image>();

        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(baseScale, enlargeSize, time / enlargeTime);
            trans.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            component.color = Color.Lerp(Color.white, color, time / enlargeTime);
            time += Time.deltaTime;
            yield return null;
        }
        component.color = color;
        time = 0;
        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(enlargeSize, baseScale, time / enlargeTime);
            trans.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            time += Time.deltaTime;
            yield return null;
        }
        
    }
}
