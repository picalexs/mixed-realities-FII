using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Prefabs")] public GameObject prefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPointOne;
    [SerializeField] private Transform spawnPointTwo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnBall(prefab, spawnPointOne.position);
            Debug.Log("Spawned Ball on position 1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnBall(prefab, spawnPointTwo.position);
            Debug.Log("Spawned Ball on position 2");
        }
    }

    private void SpawnBall(GameObject ballPrefab, Vector3 position)
    {
        if (!ballPrefab) return;
        Instantiate(ballPrefab, position, Quaternion.identity);
    }
}
