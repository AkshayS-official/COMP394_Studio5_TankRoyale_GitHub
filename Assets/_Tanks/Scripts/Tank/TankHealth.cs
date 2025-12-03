using UnityEngine;
using UnityEngine.UI;

namespace _Tanks.Scripts.Tank
{
    public class TankHealth : MonoBehaviour
    {
        [Header("Tank Stats")]
        public TankStats m_TankStats;
        
        public float m_StartingHealth = 100f;               
        public Slider m_Slider;                             
        public Image m_FillImage;                           
        public Color m_FullHealthColor = Color.green;    
        public Color m_ZeroHealthColor = Color.red;      
        public GameObject m_ExplosionPrefab;                
        [HideInInspector] public bool m_HasShield;
        public bool isPlayerTank = false;
        
        
        private AudioSource m_ExplosionAudio;               
        private ParticleSystem m_ExplosionParticles;        
        private float m_CurrentHealth;                      
        private bool m_Dead;                                
        private float m_ShieldValue;                        
        private bool m_IsInvincible;

        private void Awake ()
        {
            // Apply stats from ScriptableObject if available
            if (m_TankStats != null)
            {
                m_StartingHealth = m_TankStats.maxHealth;
            }
            
            // Instantiate the explosion prefab and get a reference to the particle system on it.
            m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

            // Get a reference to the audio source on the instantiated prefab.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

            // Disable the prefab so it can be activated when it's required.
            m_ExplosionParticles.gameObject.SetActive (false);
            
            // Set the slider max value to the max health the tank can have
            m_Slider.maxValue = m_StartingHealth;
        }

        private void OnDestroy()
        {
            if(m_ExplosionParticles != null)
                Destroy(m_ExplosionParticles.gameObject);
        }

        private void OnEnable()
        {
            // When the tank is enabled, reset the tank's health and whether or not it's dead.
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
            m_HasShield = false;
            m_ShieldValue = 0;
            m_IsInvincible = false;

            // Update the health slider's value and color.
            SetHealthUI();
        }


        public void TakeDamage (float amount)
        {
            // Check if the tank is not invincible
            if (!m_IsInvincible)
            {
                // Reduce current health by the amount of damage done.
                m_CurrentHealth -= amount * (1 - m_ShieldValue);

                // Change the UI elements appropriately.
                SetHealthUI ();

                // If the current health is at or below zero and it has not yet been registered, call OnDeath.
                if (m_CurrentHealth <= 0f && !m_Dead)
                {
                    OnDeath ();
                }
            }
        }


        public void IncreaseHealth(float amount)
        {
            // Check if adding the amount would keep the health within the maximum limit
            if (m_CurrentHealth + amount <= m_StartingHealth)
            {
                // If the new health value is within the limit, add the amount
                m_CurrentHealth += amount;
            }
            else
            {
                // If the new health exceeds the starting health, set it at the maximum
                m_CurrentHealth = m_StartingHealth;
            }

            // Change the UI elements appropriately.
            SetHealthUI();
        }


        public void ToggleShield (float shieldAmount)
        {
            // Inverts the value of has shield.
            m_HasShield = !m_HasShield;

            // Stablish the amount of damage that will be reduced by the shield
            if (m_HasShield)
            {
                m_ShieldValue = shieldAmount;
            }
            else
            {
                m_ShieldValue = 0;
            }
        }

        public void ToggleInvincibility()
        {
            m_IsInvincible = !m_IsInvincible;
        }


        private void SetHealthUI ()
        {
            // Set the slider's value appropriately.
            m_Slider.value = m_CurrentHealth;

            // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
            m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }


        private void OnDeath ()
        {
            // Set the flag so that this function is only called once.
            m_Dead = true;

            // Move the instantiated explosion prefab to the tank's position and turn it on.
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive (true);

            // Play the particle system of the tank exploding.
            m_ExplosionParticles.Play ();

            // Play the tank explosion sound effect.
            m_ExplosionAudio.Play();

            // NEW: If this is the player tank, stop the timer and show scoreboard
            if (isPlayerTank)
            {
                Timer timer = FindAnyObjectByType<Timer>();
                if (timer != null)
                {
                    timer.StopTimerOnDeath();
                }
            }

            // Turn the tank off.
            gameObject.SetActive (false);
        }
    }
}