using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 8f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.15f;

    [SerializeField] private float jumpBufferTime = 0.12f;

    private Rigidbody2D body;
    private float moveInput;
    private float jumpBufferRemaining;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = 0f;

#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        if (keyboard.aKey.isPressed) moveInput -= 1f;
        if (keyboard.dKey.isPressed) moveInput += 1f;
        bool jumpPressed = keyboard.spaceKey.wasPressedThisFrame;
#else
        if (Input.GetKey(KeyCode.A)) moveInput -= 1f;
        if (Input.GetKey(KeyCode.D)) moveInput += 1f;
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
#endif

        // Spaceを押したら、着地まで短時間だけ入力を覚えておく。
        if (jumpPressed)
        {
            jumpBufferRemaining = jumpBufferTime;
        }
    }

    void FixedUpdate()
    {
#if UNITY_6000_0_OR_NEWER
        Vector2 velocity = body.linearVelocity;
#else
        Vector2 velocity = body.velocity;
#endif

        velocity.x = moveInput * moveSpeed;

        if (jumpBufferRemaining > 0f)
        {
            bool isGrounded = groundCheck != null &&
                Physics2D.OverlapCircle(
                    groundCheck.position, groundCheckRadius, groundLayer) != null;

            if (isGrounded && velocity.y <= 0.1f)
            {
                velocity.y = jumpSpeed;
                jumpBufferRemaining = 0f;
            }
            else
            {
                jumpBufferRemaining -= Time.fixedDeltaTime;
            }
        }

#if UNITY_6000_0_OR_NEWER
        body.linearVelocity = velocity;
#else
        body.velocity = velocity;
#endif
    }
}