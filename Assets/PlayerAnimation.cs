using UnityEngine;
using System.Reflection;  // Needed for reflection

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Ground Check (Optional)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private bool isDead = false;

    // For reflection
    private BubbleTrigger bubbleTrigger;
    private FieldInfo bubbleCursorField;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 1) Get the BubbleTrigger component on the same object (if it exists)
        bubbleTrigger = GetComponent<BubbleTrigger>();
        if (bubbleTrigger != null)
        {
            // 2) Use reflection to get the private "isBubbleCursor" field
            System.Type bubbleTriggerType = typeof(BubbleTrigger);
            bubbleCursorField = bubbleTriggerType.GetField(
                "isBubbleCursor",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
        }
    }

    private void Update()
    {
        // If the player is dead, no need to update movement-based animations
        if (isDead) return;

        // 1) Movement animations (unchanged)
        float speed = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", speed);

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        float verticalVelocity = rb.velocity.y;
        bool isJumping = (verticalVelocity > 0.1f && !isGrounded);
        bool isFalling = (verticalVelocity < -0.1f && !isGrounded);
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsFalling", isFalling);

        // 2) Use reflection to detect if "isBubbleCursor" is true
        if (bubbleTrigger != null && bubbleCursorField != null)
        {
            bool isCursorActive = (bool)bubbleCursorField.GetValue(bubbleTrigger);

            // 3) If you have a "UsePhone" bool in Animator, set it
            animator.SetBool("UsePhone", isCursorActive);
        }
    }

    /// <summary>
    /// Call this when the player dies to set isDead in the animator and locally.
    /// </summary>
    public void SetDead(bool dead)
    {
        isDead = dead;
        animator.SetBool("IsDead", dead);
    }
}
