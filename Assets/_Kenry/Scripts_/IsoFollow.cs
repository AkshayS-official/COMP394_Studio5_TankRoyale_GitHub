using UnityEngine;

public class IsoFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
    }
}