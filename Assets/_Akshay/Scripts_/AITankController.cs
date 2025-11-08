using UnityEngine;

// Requires the Rigidbody and TankHealth components
[RequireComponent(typeof(Rigidbody), typeof(TankHealth))]
public class AITankController : MonoBehaviour
{
    // --- Public Variables (Tunable in Inspector) ---
    [Header("Visual Feedback")]
    [Tooltip("Renderer of the main body part that should change color based on health.")]
    public MeshRenderer mainBodyRenderer;

    [Header("AI Settings")]
    [Tooltip("The player tank's transform for the AI to follow.")]
    public Transform playerTarget;
    [Tooltip("Distance at which the AI starts chasing the player.")]
    public float chaseDistance = 15f;
    [Tooltip("Distance at which the AI stops and starts firing.")]
    public float fireDistance = 10f;
    [Tooltip("AI movement speed.")]
    public float moveSpeed = 7f;
    [Tooltip("AI turning speed.")]
    public float turnSpeed = 50f;

    [Header("Firing Settings")]
    [Tooltip("Transform where the projectile will spawn.")]
    public Transform fireTransform;
    [Tooltip("Prefab of the shell/projectile to fire.")]
    public GameObject shellPrefab;
    [Tooltip("Initial velocity applied to the shell.")]
    public float launchForce = 18f;
    [Tooltip("Minimum time between AI shots.")]
    public float minFireDelay = 2.0f;
    [Tooltip("Maximum time between AI shots.")]
    public float maxFireDelay = 4.0f;

    // --- Private Variables ---
    private Rigidbody rb;
    private TankHealth health;
    private float nextFireTime;
    private Color fullHealthColor = Color.green; // Full health color
    private Color lowHealthColor = Color.red;    // Low health color

    // --- Unity Lifecycle Methods ---

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<TankHealth>();
        nextFireTime = Time.time + Random.Range(minFireDelay, maxFireDelay);

        // Ensure the AI tank starts at full health color if renderer is assigned
        if (mainBodyRenderer != null)
        {
            mainBodyRenderer.material.color = fullHealthColor;
        }
    }

    private void Update()
    {
        // 1. Check for player target
        if (playerTarget == null)
        {
            // Try to find the player tank in the scene (requires PlayerTank to be tagged!)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTarget = player.transform;
            }
            return;
        }

        // 2. Update color based on health
        //UpdateTankColor();

        // 3. Run AI Logic (This is the method the compiler couldn't find before)
        AILogic();
    }

    // --- AI Logic Methods ---

    private void AILogic()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);

        if (distanceToTarget < fireDistance)
        {
            // State: Attack (Stop and Fire)
            StopMoving();
            RotateTowardsTarget();
            TryFire();
        }
        else if (distanceToTarget < chaseDistance)
        {
            // State: Chase
            RotateTowardsTarget();
            MoveForward();
        }
        else
        {
            // State: Patrol/Idle
            StopMoving();
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = playerTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the tank body
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    private void MoveForward()
    {
        // Use FixedUpdate for physics movement, but we call this from Update, so let's stick to Time.deltaTime
        Vector3 movement = transform.forward * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void StopMoving()
    {
        // Stop the tank by zeroing out its velocity and angular velocity
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void TryFire()
    {
        if (Time.time > nextFireTime)
        {
            // Fire sequence
            GameObject shellInstance = Instantiate(shellPrefab, fireTransform.position, fireTransform.rotation);
            Rigidbody shellRb = shellInstance.GetComponent<Rigidbody>();
            if (shellRb != null)
            {
                shellRb.linearVelocity = fireTransform.forward * launchForce;
            }

            // Set next fire time
            nextFireTime = Time.time + Random.Range(minFireDelay, maxFireDelay);
        }
    }

    // --- Visual Feedback Method ---

    private void UpdateTankColor()
    {
        // Linearly interpolate the color based on health percentage.
        float healthRatio = health.CurrentHealth / health.maxHealth;
        if (mainBodyRenderer != null)
        {
            // Lerp from Red (Low Health) to Green (Full Health)
            mainBodyRenderer.material.color = Color.Lerp(lowHealthColor, fullHealthColor, healthRatio);
        }
    }
}