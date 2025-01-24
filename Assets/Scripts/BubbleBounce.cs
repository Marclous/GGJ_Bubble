using UnityEngine;
using System.Collections;

public class BubbleBounceTrigger : MonoBehaviour
{
    public float bounceDistance = 2f;
    public float bounceTime = 0.2f; // Time it takes to move up

    private bool isBouncing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && !isBouncing && other.CompareTag("Player"))
        {
            // If you only want to bounce if falling:
            if (rb.velocity.y >= 0f) return;

            other.GetComponent<PlayerMovement>().InitJump(0);
        }
    }

    private IEnumerator BounceRoutine(Rigidbody2D rb)
    {
        isBouncing = true;

        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + (Vector2.up * bounceDistance);

        float elapsed = 0f;
        while (elapsed < bounceTime)
        {
            // We use fixedDeltaTime and MovePosition in Sync with physics
            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;

            // Interpolate between startPos and endPos
            float t = Mathf.Clamp01(elapsed / bounceTime);
            Vector2 newPos = Vector2.Lerp(startPos, endPos, t);

            // MovePosition smoothly each physics step
            rb.MovePosition(newPos);
        }

        isBouncing = false;
    }
}
