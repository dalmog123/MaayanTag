using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 5f;
    public float baseJumpForce = 12f;        // Reduced from 20f for lower jump
    public float highJumpForce = 16f;        // Reduced from 30f for lower high jump
    public float gravity = 47.5f;
    
    [Header("Advanced Jump Settings")]
    public float initialJumpGravity = 47.5f;
    public float fallGravity = 47.5f;
    public float terminalVelocity = 45f;
    public float airControlFactor = 0.95f;   // Increased from 0.8f for better air control
    
    [Header("Jump Buffer Settings")]
    public float jumpBufferTime = 0.2f;
    public float coyoteTime = 0.15f;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float fallSpeed;
    private float lastGroundedTime;
    private float lastJumpPressedTime;
    private bool isJumping;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Update timers
        if (controller.isGrounded)
            lastGroundedTime = Time.time;
            
        if (Input.GetButtonDown("Jump"))
            lastJumpPressedTime = Time.time;

        // Check grounded state
        isGrounded = controller.isGrounded;

        // Handle jump input with buffer and coyote time
        bool canJump = Time.time - lastGroundedTime <= coyoteTime;
        bool wantsToJump = Time.time - lastJumpPressedTime <= jumpBufferTime;

        if (isGrounded)
        {
            isJumping = false;
            if (fallSpeed < 0)
                fallSpeed = -2f;

            // Jump logic with normal and high jump
            if (wantsToJump)
            {
                isJumping = true;
                fallSpeed = Input.GetKey(KeyCode.LeftShift) ? highJumpForce : baseJumpForce;
                lastJumpPressedTime = 0; // Reset jump buffer
            }
        }
        else if (canJump && wantsToJump && !isJumping)
        {
            // Handle coyote time jump
            isJumping = true;
            fallSpeed = Input.GetKey(KeyCode.LeftShift) ? highJumpForce : baseJumpForce;
            lastJumpPressedTime = 0;
        }

        float currentGravity = gravity;
        
        // Apply gravity with enhanced control
        if (!isGrounded)
        {
            // Cut jump height if jump button is released early
            if (fallSpeed > 0 && !Input.GetButton("Jump"))
                fallSpeed *= 0.5f;

            fallSpeed -= currentGravity * Time.deltaTime;
            if (fallSpeed < -terminalVelocity)
                fallSpeed = -terminalVelocity;
        }

        // Movement input with enhanced air control
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveFactor = isGrounded ? 1f : airControlFactor;
        
        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * moveSpeed * moveFactor;
        velocity = new Vector3(move.x, fallSpeed, move.z);
        
        controller.Move(velocity * Time.deltaTime);
    }
}