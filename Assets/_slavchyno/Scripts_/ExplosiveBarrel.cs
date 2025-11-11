using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float explosionRadius = 6f;
    public float maxDamage = 50f;
    public GameObject explosionVFX;
    public float upwardForce = 200f;

    public void Explode()
    {
        // VFX
        if (explosionVFX) Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // Apply physics explosion
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null) rb.AddExplosionForce(upwardForce, transform.position, explosionRadius);

            // Damage tanks
            TankHealth th = hit.GetComponent<TankHealth>();
            if (th != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                float damage = Mathf.Clamp01(1 - (dist / explosionRadius)) * maxDamage;
                th.ApplyDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        // If hit by a shell or heavy impact, explode
        if (col.relativeVelocity.magnitude > 5f || col.gameObject.CompareTag("Projectile"))
        {
            Explode();
        }
    }
}
