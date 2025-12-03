using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float interval = 1f;   // freely changeable at runtime

    float t;

    void Update()
    {
        t += Time.deltaTime;
        if (t >= interval)
        {
            Instantiate(prefab, transform.position, transform.rotation);
            t = 0f;
        }
    }

    public void SetInterval(float newInterval)
    {
        interval = newInterval;
    }
}