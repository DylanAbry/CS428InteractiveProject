using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject ballPrefab;
    public int ballCount = 800;

    [Header("Spawn Area")]
    public Vector3 spawnAreaSize = new Vector3(8f, 3f, 11f);
    public Transform spawnCenter;

    [Header("Materials")]
    public Material[] ballMaterials;

    void Start()
    {
        SpawnBalls();
    }

    void SpawnBalls()
    {
        for (int i = 0; i < ballCount; i++)
        {
            Vector3 randomPos = spawnCenter.position + new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(0, spawnAreaSize.y),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            GameObject ball = Instantiate(ballPrefab, randomPos, Random.rotation);

            AssignRandomMaterial(ball);
        }
    }

    void AssignRandomMaterial(GameObject ball)
    {
        if (ballMaterials.Length == 0) return;

        Renderer rend = ball.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = ballMaterials[Random.Range(0, ballMaterials.Length)];
        }
    }
}
