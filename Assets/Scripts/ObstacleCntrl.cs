using UnityEngine;

public class ObstacleCntrl : MonoBehaviour
{
    public Vector2 posYBorders = Vector2.zero;
    public Vector2 distanceBorders = Vector2.zero;
    public float distanceMultiplier = 0f;
    public Transform[] twoParts;
    public float moveSpeed = 1.0f;

    private Transform trans;
    private float leftEdge;
    private float rightEdge;

    private void Awake()
    {
        trans = transform;
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector2.zero).x - 2f;
        rightEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0f)).x + 2f;
        trans.position = new Vector3(rightEdge, Random.Range(posYBorders.x, posYBorders.y), trans.position.z);
    }

    public void SetId(int id)
    {
        float distanceProgress = Mathf.Min(id * distanceMultiplier, 1f);
        float distance = Mathf.Lerp(distanceBorders[1], distanceBorders[0], distanceProgress);
        twoParts[0].localPosition = Vector3.zero + Vector3.down * distance;
        twoParts[1].localPosition = Vector3.zero + Vector3.up * distance;
    }

    private void Update()
    {
        trans.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (trans.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
