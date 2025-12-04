using System.Collections.Generic;
using UnityEngine;

// Kenry:
// I have no idea where I'm going with this.

public class UnitManager : MonoBehaviour
{
    public List<GameObject> prefabs;
    List<GameObject> pool = new List<GameObject>();

    public GameObject Get()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        GameObject obj = Instantiate(prefab);
        pool.Add(obj);
        return obj;
    }
}
