using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public sealed class EnemyHop : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Min(1.01f)] private float gravityMultiplier = 2.5f;
    [SerializeField, Min(0f)] private float jumpSpeed = 8f;
    [SerializeField, Min(0f)] private float moveSpeed = 4f;
    [SerializeField, Min(0f)] private float groundPause = 0.35f;
    [SerializeField, Min(0f)] private float stoppingDistance = 1.1f;

    private Rigidbody2D body;
    private Collider2D bodyCollider;
    private ContactFilter2D groundContacts;
    private float pauseRemaining;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        groundContacts.SetLayerMask(groundLayer);
        groundContacts.SetNormalAngle(45f, 135f);
        pauseRemaining = groundPause;
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        body.gravityScale = Mathf.Max(1f, player.gravityScale * gravityMultiplier);
        Vector2 velocity = body.linearVelocity;
        if (velocity.y > 0.1f || !bodyCollider.IsTouching(groundContacts))
        {
            return;
        }

        pauseRemaining -= Time.fixedDeltaTime;
        float offset = player.position.x - body.position.x;
        float distance = Mathf.Abs(offset) - stoppingDistance;
        velocity.x = 0f;

        float downwardAcceleration = -Physics2D.gravity.y * body.gravityScale;
        if (pauseRemaining <= 0f && distance > 0f &&
            jumpSpeed > 0f && downwardAcceleration > 0f)
        {
            float flightTime = 2f * jumpSpeed / downwardAcceleration;
            velocity.x = Mathf.Sign(offset) * Mathf.Min(moveSpeed, distance / flightTime);
            velocity.y = jumpSpeed;
            pauseRemaining = groundPause;
        }

        body.linearVelocity = velocity;
    }
}