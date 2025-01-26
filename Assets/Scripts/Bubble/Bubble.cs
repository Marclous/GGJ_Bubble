using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Bubble : MonoBehaviour
{
    public int health = 10;
    public AudioClip bubbleGenerate, bubblePutdown, bubbleBreak;
    private AudioSource audioSource;
    private Collider2D bubbleCollider;
    private Animator animator;

    private void Awake()
    {
        bubbleCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        StartCoroutine(DecreaseHealthOverTime());
        audioSource.clip = bubbleGenerate;
        audioSource.Play();
    }

    private IEnumerator DecreaseHealthOverTime()
    {
        while (health > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            health--;

            if (health <= 0)
            {
                HandleDeath();
            }
        }
    }

    /// <summary>
    /// Called when the bubble's health reaches 0.
    /// Removes from the Bubble list, disables the collider, 
    /// triggers the death animation, and destroys the object after.
    /// </summary>
    private void HandleDeath()
    {
        // Remove from BubbleTrigger's list so it's no longer tracked
        if (BubbleTrigger.instance != null)
            BubbleTrigger.instance.bubbleList.Remove(this);

        // Disable the collider so it won't interact
        bubbleCollider.enabled = false;

        // Trigger the death animation (ensure you have a "Death" trigger in your Animator)
        animator.SetTrigger("Death");

        // Start coroutine to wait for the animation to finish
        StartCoroutine(PlayDeathThenDestroy());
    }

    /// <summary>
    /// Waits until the Death animation finishes, then destroys the object.
    /// </summary>
    private IEnumerator PlayDeathThenDestroy()
    {
        // Option 1: Wait until we enter the "Death" state, then wait until it's done
        // (Make sure your Animator has a state named "Death" or adjust accordingly.)
        audioSource.clip = bubbleBreak;
        audioSource.Play();
        // 1. Wait for the Animator to switch to the Death state
        //    (Prevents us from checking normalizedTime on the wrong clip)
        yield return new WaitUntil(() => 
            animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

        // 2. Wait for the Death animation to finish (normalizedTime >= 1 means it's done)
        yield return new WaitUntil(() => 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        // Now the animation has finished, destroy the bubble
        
        Destroy(gameObject);
    }
}
