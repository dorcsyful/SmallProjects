using Unity.Mathematics;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private BoxCollider2D cat;
    [SerializeField] private Canvas gameOverCanvas;
    public float movementSpeed = 10;

    private BoxCollider2D[] obstacleColliders;

    private void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
        obstacleColliders = new BoxCollider2D[3];
        obstacleColliders[0] = Instantiate(obstacles[0], Vector3.zero,quaternion.identity, transform).GetComponent<BoxCollider2D>();
        obstacleColliders[1] = Instantiate(obstacles[0], Vector3.zero,quaternion.identity, transform).GetComponent<BoxCollider2D>();
        obstacleColliders[2] = Instantiate(obstacles[0], Vector3.zero,quaternion.identity, transform).GetComponent<BoxCollider2D>();
        float y = cat.bounds.min.y;
        Vector3 temp = new Vector3(OrthographicBounds(Camera.main).max.x, y, cat.transform.position.z);
        obstacleColliders[0].transform.position = temp;
        obstacleColliders[1].transform.position = temp;
        obstacleColliders[2].transform.position = temp;
    }

    public void ResetObstacles()
    {
        Debug.Log("Resetting Obstacles");
        gameOverCanvas.gameObject.SetActive(false);
        float y = cat.bounds.min.y;
        Vector3 temp = new Vector3(OrthographicBounds(Camera.main).max.x, y, cat.transform.position.z);
        obstacleColliders[0].transform.position = temp;
        obstacleColliders[1].transform.position = temp;
        obstacleColliders[2].transform.position = temp;
        enabled = true;

    }
    
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
             if(transform.GetChild(i).TryGetComponent(out BoxCollider2D obstacleCollider))
             {
                 if (obstacleCollider.Distance(cat).isOverlapped)
                 {
                     cat.gameObject.GetComponent<CatScript>().GameOver();
                     gameOverCanvas.gameObject.SetActive(true);
                     enabled = false;
                 }
                 else
                 {
                     if (obstacleCollider.bounds.max.x < OrthographicBounds(Camera.main).min.x)
                     {
                         obstacleCollider.transform.position = new Vector3(OrthographicBounds(Camera.main).max.x,
                             obstacleCollider.transform.position.y, obstacleCollider.transform.position.z);
                     }

                 }
             }
             transform.GetChild(i).position -= Vector3.right * (movementSpeed * Time.deltaTime);

        }
    }
    
    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
