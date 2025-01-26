using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]  // So OnValidate/OnDrawGizmos works nicely in the Editor
public class Spike : MonoBehaviour
{
    [Header("Spike Movement Settings")]
    [SerializeField] private bool isMovingSpike = false;
    [SerializeField] private float speed = 2f;
    [SerializeField] private MovementMode movementMode = MovementMode.PingPong;

    // These will be auto-created if not assigned
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Vector3 startPos;
    private Vector3 endPos;
    private float journeyProgress = 0f;
    private bool movingForward = true;

    public enum MovementMode
    {
        PingPong, // Moves A <-> B repeatedly
        OneWay,   // Moves A -> B once
        Loop      // Moves A -> B, snaps back to A, repeat
    }

    private void OnValidate()
    {
        // Automatically create pointA and pointB if they are not yet assigned
        if (pointA == null)
        {
            GameObject goA = new GameObject($"{name}_PointA");
            goA.transform.SetParent(transform);
            goA.transform.localPosition = Vector3.zero; // Start at Spike's position
            pointA = goA.transform;
        }

        if (pointB == null)
        {
            GameObject goB = new GameObject($"{name}_PointB");
            goB.transform.SetParent(transform);
            goB.transform.localPosition = Vector3.zero; // Start at Spike's position
            pointB = goB.transform;
        }
    }

    private void Start()
    {
        // Store positions for point A and B
        startPos = pointA.position;
        endPos = pointB.position;
    }

    private void Update()
    {
        if (!Application.isPlaying && !isMovingSpike)
        {
            // Keep updating the saved positions in the editor if you move them around
            startPos = pointA.position;
            endPos = pointB.position;
        }
        else if (Application.isPlaying && isMovingSpike && pointA != null && pointB != null)
        {
            MoveSpike();
        }
    }

    private void MoveSpike()
    {
        switch (movementMode)
        {
            case MovementMode.PingPong:
                PingPongMovement();
                break;
            case MovementMode.OneWay:
                OneWayMovement();
                break;
            case MovementMode.Loop:
                LoopMovement();
                break;
        }
    }

    private void PingPongMovement()
    {
        if (movingForward)
        {
            journeyProgress += Time.deltaTime * speed;
            if (journeyProgress >= 1f)
            {
                journeyProgress = 1f;
                movingForward = false;
            }
        }
        else
        {
            journeyProgress -= Time.deltaTime * speed;
            if (journeyProgress <= 0f)
            {
                journeyProgress = 0f;
                movingForward = true;
            }
        }

        transform.position = Vector3.Lerp(startPos, endPos, journeyProgress);
    }

    private void OneWayMovement()
    {
        if (journeyProgress < 1f)
        {
            journeyProgress += Time.deltaTime * speed;
            journeyProgress = Mathf.Clamp01(journeyProgress);
            transform.position = Vector3.Lerp(startPos, endPos, journeyProgress);
        }
    }

    private void LoopMovement()
    {
        journeyProgress += Time.deltaTime * speed;
        if (journeyProgress >= 1f)
        {
            transform.position = startPos;
            journeyProgress = 0f;
        }
        else
        {
            transform.position = Vector3.Lerp(startPos, endPos, journeyProgress);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Stop the player's movement
            PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.DisableMovement();
                playerMovement.enabled = false;
            }

            // 2. Optionally stop player's rigidbody
            /*
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                // rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            */

            // 3. Trigger 'dead' animation if used
            PlayerAnimation playerAnim = collision.GetComponentInParent<PlayerAnimation>();
            if (playerAnim != null)
            {
                playerAnim.SetDead(true);
            }

            // 4. Show GameOver (via GameManager)
            GameManager.Instance.ShowGameOverUI();
        }
    }

    // Optional restart
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Visualize points in the Editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (pointA != null)
        {
            Gizmos.DrawLine(transform.position, pointA.position);
            Gizmos.DrawSphere(pointA.position, 0.15f);
        }
        if (pointB != null)
        {
            Gizmos.DrawLine(transform.position, pointB.position);
            Gizmos.DrawSphere(pointB.position, 0.15f);
        }
    }
}
