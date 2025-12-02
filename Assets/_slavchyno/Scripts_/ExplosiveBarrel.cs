using UnityEngine;

namespace Tanks.Complete
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class ExplosiveBarrel : MonoBehaviour
    {
        [Header("Health")]
        [Tooltip("Сколько HP у бочки изначально")]
        public float m_StartingHealth = 30f;

        [Header("Explosion Settings")]
        [Tooltip("Префаб взрыва (частицы + звук), можно взять такой же, как у танка")]
        public GameObject m_ExplosionPrefab;
        [Tooltip("Максимальный урон, если танк вплотную к бочке")]
        public float m_MaxDamage = 80f;
        [Tooltip("Сила взрывной волны, передаётся в TankMovement.AddExplosionForce")]
        public float m_ExplosionForce = 40f;
        [Tooltip("Радиус взрыва. Дальше этого расстояния урон = 0")]
        public float m_ExplosionRadius = 5f;

        private float m_CurrentHealth;
        private bool m_Exploded;

        private ParticleSystem m_ExplosionParticles;
        private AudioSource m_ExplosionAudio;

        private void Awake()
        {
            // НИЧЕГО не трогаем в настройках коллайдера и rigidbody,
            // чтобы они работали как обычный физический объект.

            if (m_ExplosionPrefab != null)
            {
                m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
                m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

                m_ExplosionParticles.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            m_CurrentHealth = m_StartingHealth;
            m_Exploded = false;
        }

        private void OnDestroy()
        {
            if (m_ExplosionParticles != null)
                Destroy(m_ExplosionParticles.gameObject);
        }

        public void TakeDamage(float amount)
        {
            if (m_Exploded)
                return;

            m_CurrentHealth -= amount;

            if (m_CurrentHealth <= 0f)
            {
                Explode();
            }
        }

        // Обычная физическая коллизия (Is Trigger = false у бочки)
        private void OnCollisionEnter(Collision collision)
        {
            if (m_Exploded)
                return;

            Debug.Log($"[Barrel] Collision with: {collision.gameObject.name}", this);

            HandleHit(collision.collider);
        }

        // На всякий случай оставим поддержку триггеров, если что-то ещё триггерное ударит бочку
        private void OnTriggerEnter(Collider other)
        {
            if (m_Exploded)
                return;

            Debug.Log($"[Barrel] Trigger with: {other.gameObject.name}", this);

            HandleHit(other);
        }

        private void HandleHit(Collider other)
        {
            GameObject otherGO = other.gameObject;

            // 1) Попадание снаряда
            if (otherGO.CompareTag("Shell"))
            {
                Debug.Log("[Barrel] Hit by Shell -> explode", this);
                Explode();
                return;
            }

            // 2) Контакт с танком (TankHealth на родителе)
            TankHealth tankHealth = other.GetComponentInParent<TankHealth>();
            if (tankHealth != null)
            {
                Debug.Log("[Barrel] Hit tank -> explode", this);
                Explode();
                return;
            }
        }

        private void Explode()
        {
            if (m_Exploded)
                return;

            m_Exploded = true;
            Debug.Log("[Barrel] EXPLODE!", this);

            if (m_ExplosionParticles != null)
            {
                m_ExplosionParticles.transform.position = transform.position;
                m_ExplosionParticles.gameObject.SetActive(true);
                m_ExplosionParticles.Play();
            }

            if (m_ExplosionAudio != null)
            {
                m_ExplosionAudio.Play();
            }

            // Находим всех в радиусе
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius);

            foreach (Collider col in colliders)
            {
                TankHealth targetHealth = col.GetComponentInParent<TankHealth>();
                if (targetHealth == null)
                    continue;

                Transform tankTransform = targetHealth.transform;
                Debug.Log($"[Barrel] Damage tank: {tankTransform.name}", this);

                TankMovement targetMovement = col.GetComponentInParent<TankMovement>();
                if (targetMovement != null)
                {
                    targetMovement.AddExplosionForce(
                        m_ExplosionForce,
                        transform.position,
                        m_ExplosionRadius
                    );
                }

                float damage = CalculateDamage(tankTransform.position);
                if (damage > 0f)
                {
                    targetHealth.TakeDamage(damage);
                }
            }

            Destroy(gameObject);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            Vector3 explosionToTarget = targetPosition - transform.position;
            float distance = explosionToTarget.magnitude;

            float relativeDistance = (m_ExplosionRadius - distance) / m_ExplosionRadius;
            relativeDistance = Mathf.Clamp01(relativeDistance);

            float damage = relativeDistance * m_MaxDamage;
            return damage;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_ExplosionRadius);
        }
    }
}
