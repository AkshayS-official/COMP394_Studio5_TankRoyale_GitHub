using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTankController : MonoBehaviour
{
    // NEW: The Rigidbody's constraints must be set correctly in the Inspector!
    // Freeze Position Y: Must be OFF
    // Freeze Rotation X & Z: Must be ON (to prevent tipping)

    [Header("Visual Feedback")]
    [Tooltip("Renderer of the main body part that should change color.")]
    public MeshRenderer mainBodyRenderer;
    [Tooltip("Time the tank remains red after firing (visual feedback).")]
    public float fireColorDuration = 0.2f;

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float turnSpeed = 80f;

    [Header("Firing Settings")]
    public Transform fireTransform;
    public GameObject shellPrefab;
    public float launchForce = 20f;

    // --- Private Variables ---
    private Rigidbody rb;
    private Color originalColor;
    private float movementInput;
    private float turnInput;
    private bool isFiring = false;

    // --- Unity Lifecycle Methods ---

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Safety Check: Freeze unwanted rotation
        rb.freezeRotation = true; // Better to control rotation manually

        if (mainBodyRenderer != null)
        {
            // Store the original material color
            originalColor = mainBodyRenderer.material.color;
        }
    }

    private void Update()
    {
        // Get player input
        movementInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isFiring)
            {
                StartCoroutine(FireAndFlash());
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        // **CRITICAL FIX: Use velocity to move along the tank's forward direction.**

        // 1. Calculate the desired horizontal velocity vector
        Vector3 desiredVelocity = transform.forward * movementInput * moveSpeed;

        // 2. Preserve the current vertical velocity (y-axis) 
        //    This allows gravity to still affect the tank, preventing the 'flying up' issue.
        desiredVelocity.y = rb.linearVelocity.y;

        // 3. Apply the final velocity to the Rigidbody
        rb.linearVelocity = desiredVelocity;
    }

    private void Turn()
    {
        float turn = turnInput * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply rotation directly to the transform or use rb.MoveRotation in FixedUpdate
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    // --- Firing and Visual Feedback using a Coroutine ---

    private IEnumerator FireAndFlash()
    {
        isFiring = true;

        // 1. Fire the shell
        GameObject shellInstance = Instantiate(shellPrefab, fireTransform.position, fireTransform.rotation);
        Rigidbody shellRb = shellInstance.GetComponent<Rigidbody>();
        if (shellRb != null)
        {
            shellRb.linearVelocity = fireTransform.forward * launchForce;
        }

        // Destroy the shell after 3 seconds to clean up
        Destroy(shellInstance, 3f);

        // 2. Apply visual feedback (Flash Red)
        if (mainBodyRenderer != null)
        {
            mainBodyRenderer.material.color = Color.red;
        }

        // Wait for the specified flash duration
        yield return new WaitForSeconds(fireColorDuration);

        // 3. Reset color and allow firing
        if (mainBodyRenderer != null)
        {
            mainBodyRenderer.material.color = originalColor;
        }
        isFiring = false;
    }
}