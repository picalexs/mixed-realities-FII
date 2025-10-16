using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Prefabs")]
    public GameObject basketballPrefab;
    public GameObject miniGolfBallPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform playerOrigin;
    [SerializeField] private Vector3 offsetSpawnPoint = new Vector3(0, 0.5f, 1.0f);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnBall(basketballPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnBall(miniGolfBallPrefab);
        }
    }

    private void SpawnBall(GameObject ballPrefab)
    {
        if (!ballPrefab) return;

        Transform origin = playerOrigin ? playerOrigin : transform;
        Vector3 spawnPos = origin.position + origin.TransformDirection(offsetSpawnPoint);

        Instantiate(ballPrefab, spawnPos, Quaternion.identity);
    }
}
