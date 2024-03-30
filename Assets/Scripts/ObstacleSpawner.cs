using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject bonusObstaclePrefab;
    public int bonusObstacleRate = 5;

    public Vector2 randomSpawnRate = Vector2.zero;
    private Transform trans;
    private int obstacleCount = 0;

    private void Awake()
    {
        trans = transform;
    }

    private void OnEnable()
    {
        Spawn();
    }

    private void OnDisable()
    {
        obstacleCount = 0;
        CancelInvoke();
    }

    private void Spawn()
    {
        if (obstacleCount > 0 && ((obstacleCount + 1) % bonusObstacleRate == 0)) Instantiate(bonusObstaclePrefab);
        else Instantiate(obstaclePrefab);
        obstacleCount++;

        Invoke(nameof(Spawn), Random.Range(randomSpawnRate.x, randomSpawnRate.y));
    }
}
