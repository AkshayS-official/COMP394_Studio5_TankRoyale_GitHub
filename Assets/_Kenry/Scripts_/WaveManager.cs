using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;     // multiple prefab types
    public Transform[] spawnPoints;

    int nextWaveSize = 2;

    void Start()
    {
        SpawnWave(nextWaveSize);
    }

    void Update()
    {
        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (alive == 0)
        {
            nextWaveSize += 2;
            SpawnWave(nextWaveSize);
        }
    }

    void SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(prefab, p.position, p.rotation);
        }
    }
}