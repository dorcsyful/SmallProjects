using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float enlargeTime;
    [SerializeField] private float enlargeSize;
    [SerializeField] private float moveSpeed = 150f;
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

        time = 0;
        while (time < enlargeTime)
        {
            float currentScale = Mathf.Lerp(enlargeSize, baseScale, time / enlargeTime);
            trans.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
    }
}
