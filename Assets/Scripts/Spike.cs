using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    // Drag your Game Over UI Panel (or Canvas) here in the Inspector.
    [SerializeField] private GameObject gameOverUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the collided child object is tagged "Player"
        if (collision.CompareTag("Player"))
        {
            // 1. Stop the player's movement
            PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // 2. Stop the player's Rigidbody2D (if they have one)
            /*Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                // If you want the player *completely* immovable:
                // rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }*/

            GameManager.Instance.ShowGameOverUI();

        }
    }

    // Optional: A restart function if your Game Over UI button needs it
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}