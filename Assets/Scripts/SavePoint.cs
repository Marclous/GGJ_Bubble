using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [Tooltip("Has this savepoint been activated already?")]
    [SerializeField] private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider's GameObject is tagged 'Player'
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            // Update the GameManager's current spawn point to this position
            GameManager.Instance.UpdateSavePoint(transform.position);

            Debug.Log("SavePoint activated at " + transform.position);

            // Destroy this savepoint immediately so it can't be used again
            Destroy(gameObject);
        }
    }
}