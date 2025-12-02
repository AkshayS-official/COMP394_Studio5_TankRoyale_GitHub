using UnityEngine;

public class DestroyObjectCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Destroy the object that touched this trigger
        Destroy(other.gameObject);
    }
}
