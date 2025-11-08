using UnityEngine;

public class TankHealth : MonoBehaviour
{
    public float health = 100f;

    public void ApplyDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyHeal(float heal)
    {
        health = Mathf.Min(health + heal, 100f);
    }
}
