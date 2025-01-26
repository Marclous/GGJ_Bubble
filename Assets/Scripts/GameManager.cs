using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [Tooltip("Drag your Player GameObject here (the one with PlayerMovement).")]
    [SerializeField] private GameObject player;

    [Tooltip("Drag your Game Over UI panel here.")]
    [SerializeField] private GameObject gameOverUI;

    // The position where the player will respawn after death
    private Vector3 lastSavePosition;

    private void Awake()
    {
        // Enforce a singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // (Optional) DontDestroyOnLoad(gameObject) if you want it persistent across scenes
    }

    private void Start()
    {
        // 1. Hide Game Over UI initially
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        // 2. Initialize the spawn point to the player's starting position
        if (player != null)
            lastSavePosition = player.transform.position;
    }

    /// <summary>
    /// Update the current savepoint position to a new location.
    /// Called by SavePoint script.
    /// </summary>
    public void UpdateSavePoint(Vector3 newSavePosition)
    {
        lastSavePosition = newSavePosition;
        Debug.Log("GameManager: Updated savepoint to " + newSavePosition);
    }

    /// <summary>
    /// Called by hazards or death logic to show the Game Over UI.
    /// </summary>
    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // If you want to pause the game, do so
        //Time.timeScale = 0f;
    }

    /// <summary>
    /// This should be linked to your Game Over UI's Restart button (OnClick).
    /// </summary>
    public void RestartGame()
    {
        // Hide the UI
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        // Unpause
        //Time.timeScale = 1f;

        // If we still have a player reference, move it to the last savepoint
        if (player != null)
        {
            // Enable the player's movement script if it was disabled
            player.transform.position = lastSavePosition;

            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = true;
            // Reset IsDead to false, so the player is "alive" again
            var playerAnim = player.GetComponent<PlayerAnimation>();
            if (playerAnim != null)
                playerAnim.SetDead(false);
        }

        // If you prefer to reload the scene instead, you can do:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // But then you'd need to store lastSavePosition in a way that persists (e.g., static).
    }
}
