using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    // Assign this from the Inspector
    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        // If an instance already exists and it’s not this one, destroy this.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, make this the singleton instance
        Instance = this;

        // Optional: if you want this manager to persist across scenes
        // DontDestroyOnLoad(gameObject);
    }

    // Called by anything that needs to show Game Over
    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
}