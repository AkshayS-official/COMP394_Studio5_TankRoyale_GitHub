using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BetterTankMovement : MonoBehaviour
{
    [Header("Movement")]
    public float maxForwardSpeed = 8f;
    public float maxReverseSpeed = 4f;
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float turnSpeed = 120f; // degrees per second

    Rigidbody rb;
    float currentSpeed = 0f;
    float inputMove;
    float inputTurn;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // Use continuous collision detection for better accuracy in preventing tunneling
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        // Read inputs in Update (be careful to apply in FixedUpdate)
        inputMove = Input.GetAxis("Vertical");   // W/S or Up/Down
        inputTurn = Input.GetAxis("Horizontal"); // A/D or Left/Right
    }

    void FixedUpdate()
    {
        // Handle forward/back speed with accel/decel
        float desiredSpeed = 0f;
        if (inputMove > 0f) desiredSpeed = inputMove * maxForwardSpeed;
        else if (inputMove < 0f) desiredSpeed = inputMove * maxReverseSpeed;

        // Smooth acceleration / deceleration
        if (Mathf.Abs(desiredSpeed) > Mathf.Abs(currentSpeed))
        {
            // accelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // decelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, deceleration * Time.fixedDeltaTime);
        }

        // Compute movement vector in local forward direction
        Vector3 forwardMove = transform.forward * currentSpeed;

        // Apply horizontal movement via Rigidbody velocity (preserving y velocity for gravity)
        Vector3 newVel = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z);
        rb.linearVelocity = newVel;

        // Rotation: apply angular velocity using MoveRotation for smooth physics-friendly turning
        float turnAmount = inputTurn * turnSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
