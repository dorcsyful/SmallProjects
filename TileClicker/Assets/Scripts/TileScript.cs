using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color defaultColor;
    private bool isActive;
    private ECOLOR colorCode;
    private Color color;
    
    public Color GetColor() { return color; }
    public ECOLOR GetColorCode() { return colorCode; }
    public bool IsActive() { return isActive; }
    public void Activate(Color newColor, ECOLOR newColorCode)
    {
        isActive = true;
        color = newColor;
        colorCode = newColorCode;
        StartCoroutine(Flip());
    }

    public void ResetColor()
    {
        colorCode = ECOLOR.DEFAULT;
        isActive = false;
        image.color = defaultColor;
    }

    IEnumerator Flip()
    {
        // First half: Shrink the button to 0 on X-axis
        for (float i = 1f; i >= 0f; i -= 0.1f)
        {
            transform.localScale = new Vector3(i, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }
        
        image.color = color;
        
        // Second half: Expand the button back to normal size
        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            transform.localScale = new Vector3(i, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
