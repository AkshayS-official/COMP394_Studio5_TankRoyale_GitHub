using UnityEngine;

namespace _Tanks.Scripts
{
    [CreateAssetMenu(fileName = "TankStats", menuName = "Scriptable Objects/TankStats")]
    public class TankStats : ScriptableObject
    {
        [Header("Health")] [Tooltip("Maximum health of the tank")]
        public float maxHealth = 100f;

        [Header("Movement")] [Tooltip("Speed at which the tank moves")]
        public float movementSpeed = 12f;

        [Tooltip("Speed at which the tank turns")]
        public float turnSpeed = 180f;

        [Header("Attack")] [Tooltip("How fast the tank can charge its shots (higher = faster charge)")]
        public float attackSpeed = 1f;

        [Tooltip("Damage multiplier for projectiles")]
        public float damage = 1f;

        [Header("Projectile")] [Tooltip("How far the tank's projectiles can travel")]
        public float range = 1f;

        [Tooltip("Launch force multiplier for projectiles")]
        public float launchForce = 1f;

        [Header("Charge")] [Tooltip("Minimum time to charge a shot")]
        public float minChargeTime = 0.75f;

        [Tooltip("Maximum time to fully charge a shot")]
        public float maxChargeTime = 1.5f;
    }
}
