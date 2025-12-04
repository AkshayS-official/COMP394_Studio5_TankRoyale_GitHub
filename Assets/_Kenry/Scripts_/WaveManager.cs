using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<GameObject> yellowPrefabs;
    public List<GameObject> greenPrefabs;
    public List<GameObject> purplePrefabs;
    public List<GameObject> redPrefabs;

    public Transform[] yellowPoints;
    public Transform[] greenPoints;
    public Transform[] purplePoints;
    public Transform[] redPoints;

    public int yellowCount = 1;
    public int greenCount = 1;
    public int purpleCount = 1;
    public int redCount = 1;

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        int yellowAlive = GameObject.FindGameObjectsWithTag("Yellow").Length;
        int greenAlive  = GameObject.FindGameObjectsWithTag("Green").Length;
        int purpleAlive   = GameObject.FindGameObjectsWithTag("Purple").Length;

        if (yellowAlive == 0 && greenAlive == 0 && purpleAlive == 0)
        {
            yellowCount += 2;
            greenCount  += 2;
            purpleCount   += 2;
            redCount    += 1;

            SpawnWave();
        }
    }

    void SpawnWave()
    {
        SpawnFaction(yellowPrefabs, yellowPoints, yellowCount);
        SpawnFaction(greenPrefabs,  greenPoints,  greenCount);
        SpawnFaction(purplePrefabs,   purplePoints,   purpleCount);
        SpawnFaction(redPrefabs,    redPoints,    redCount);
    }

    void SpawnFaction(List<GameObject> prefabs, Transform[] points, int count)
    {
        if (prefabs.Count == 0 || points.Length == 0) return;

        for (int i = 0; i < count; i++)
        {
            var prefab = prefabs[Random.Range(0, prefabs.Count)];
            var point  = points[Random.Range(0, points.Length)];
            Instantiate(prefab, point.position, point.rotation);
        }
    }
}