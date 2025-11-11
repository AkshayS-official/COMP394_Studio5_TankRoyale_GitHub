using UnityEngine;

public class SimpleTankMovement : MonoBehaviour
{
    public float moveSpeed = 8f;       // forward / backward speed
    public float turnSpeed = 120f;     // rotation speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevents tank from tipping over
    }

    void FixedUpdate()
    {
        // Get player input (using WASD)
        float moveInput = Input.GetAxis("Vertical");   // W/S or ↑↓
        float turnInput = Input.GetAxis("Horizontal"); // A/D or ←→

        // Move forwards/backwards
        Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate left/right
        float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
    }
}
