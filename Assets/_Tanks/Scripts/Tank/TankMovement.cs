
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Tanks.Scripts.Tank
{
    public class TankMovement : MonoBehaviour
    {
        [Header("Tank Stats")]
        public TankStats m_TankStats;
        
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
        public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
        
        [HideInInspector] public bool m_IsComputerControlled = false;  // Whether this tank is controlled by AI
        
        public int ControlIndex { get; set; } = 1;  // Control index: 1 = left keyboard or pad, 2 = right keyboard, -1 = no control
        
        private TankInputUser m_InputUser;          // The Input User component for that tanks. Contains the Input Actions.
        private InputAction m_MoveAction;           // The Input Action for moving forward/back
        private InputAction m_TurnAction;           // The Input Action for turning left/right
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        
        // Explosion force variables
        private Vector3 m_ExplosionForce = Vector3.zero;
        private float m_ExplosionDecay = 5f;        // How quickly the explosion force diminishes

        public Rigidbody Rigidbody => m_Rigidbody;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            
            // Apply stats from ScriptableObject if available
            if (m_TankStats != null)
            {
                m_Speed = m_TankStats.movementSpeed;
                m_TurnSpeed = m_TankStats.turnSpeed;
            }
            
            // Get or add the TankInputUser component
            m_InputUser = GetComponent<TankInputUser>();
            if (m_InputUser == null)
                m_InputUser = gameObject.AddComponent<TankInputUser>();
        }

        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;
        }

        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;
        }

        private void Start()
        {
            // Store the original pitch of the audio source.
            if (m_MovementAudio != null)
            {
                m_OriginalPitch = m_MovementAudio.pitch;
            }
            
            // Get the input actions from the InputUser's action asset
            // Try to find actions - they might be named "Vertical" and "Horizontal" or similar
            if (m_InputUser.ActionAsset != null)
            {
                // Try common action names for movement
                m_MoveAction = m_InputUser.ActionAsset.FindAction("Vertical");
                if (m_MoveAction == null)
                    m_MoveAction = m_InputUser.ActionAsset.FindAction("Move");
                
                m_TurnAction = m_InputUser.ActionAsset.FindAction("Horizontal");
                if (m_TurnAction == null)
                    m_TurnAction = m_InputUser.ActionAsset.FindAction("Turn");
                
                // Enable the actions if found
                if (m_MoveAction != null)
                    m_MoveAction.Enable();
                if (m_TurnAction != null)
                    m_TurnAction.Enable();
            }
        }

        private void Update()
        {
            // Only process input if not computer controlled
            if (!m_IsComputerControlled)
            {
                // Read input values from the new Input System actions
                if (m_MoveAction != null)
                    m_MovementInputValue = m_MoveAction.ReadValue<float>();
                else
                    m_MovementInputValue = 0f;
                    
                if (m_TurnAction != null)
                    m_TurnInputValue = m_TurnAction.ReadValue<float>();
                else
                    m_TurnInputValue = 0f;

                EngineAudio();
            }
        }

        private void EngineAudio()
        {
            if (m_MovementAudio == null)
                return;

            // If there is no input (the tank is stationary)...
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        private void FixedUpdate()
        {
            // Apply explosion force if any exists
            if (m_ExplosionForce.magnitude > 0.01f)
            {
                m_Rigidbody.AddForce(m_ExplosionForce, ForceMode.Force);
                // Decay the explosion force over time
                m_ExplosionForce = Vector3.Lerp(m_ExplosionForce, Vector3.zero, m_ExplosionDecay * Time.fixedDeltaTime);
            }

            // Only move if not computer controlled (AI handles its own movement)
            if (!m_IsComputerControlled)
            {
                Move();
                Turn();
            }
        }

        private void Move()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        private void Turn()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }

        /// <summary>
        /// Adds an explosion force to the tank's rigidbody
        /// </summary>
        public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
        {
            // Calculate direction from explosion to tank
            Vector3 direction = transform.position - explosionPosition;
            float distance = direction.magnitude;

            // Normalize the direction
            if (distance > 0.01f)
            {
                direction.Normalize();
            }
            else
            {
                // If we're at the explosion center, push upward and in a random direction
                direction = Vector3.up + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                direction.Normalize();
            }

            // Calculate force falloff based on distance
            float forceFalloff = 1f - Mathf.Clamp01(distance / explosionRadius);
            
            // Store the explosion force to be applied over time
            m_ExplosionForce = direction * explosionForce * forceFalloff;
        }
    }
}