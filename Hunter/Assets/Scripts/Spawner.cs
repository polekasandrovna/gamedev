using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject wolf;
    [SerializeField] private GameObject deer;
    [SerializeField] private float spawnRangeX = 10f;
    [SerializeField] private float spawnRangeY = 5f;
    [SerializeField] private float bounds = 0.5f;

    private Vector2 GenerateRandomSpawnPoint()
    {
        float randX = UnityRandom.Range(-spawnRangeX + bounds, spawnRangeX - bounds);
        float randY = UnityRandom.Range(-spawnRangeY + bounds, spawnRangeY - bounds);
        return new Vector2(randX, randY);
    }
    void Start()
    {
        for (int i = 0; i < FindObjectOfType<GameController>().RabbitCount; i++)
        {
            Instantiate(rabbit, GenerateRandomSpawnPoint(), Quaternion.identity);
        }
        for (int i = 0; i < FindObjectOfType<GameController>().WolfCount; i++)
        {
            Instantiate(wolf, GenerateRandomSpawnPoint(), Quaternion.identity);
        }
        for (int i = 0; i < FindObjectOfType<GameController>().DeerCount; i++)
        {
            Instantiate(deer, GenerateRandomSpawnPoint(), Quaternion.identity);
        }
    }

    public float SpawnRangeX => spawnRangeX;
    public float SpawnRangeY => spawnRangeY;
    public float Bounds => bounds;
}
