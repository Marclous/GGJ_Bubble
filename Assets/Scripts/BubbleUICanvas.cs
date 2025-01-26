using UnityEngine;
using UnityEngine.UI;

public class BubbleUICanvas : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The UI Image component displaying bubble count sprites.")]
    [SerializeField] private Image bubbleImage;

    [Header("Bubble Count Sprites")]
    [Tooltip("Sprites to display for each bubble count (0,1,2,3...). The element index = bubble count.")]
    [SerializeField] private Sprite[] bubbleCountSprites;

    private void Update()
    {
        // Make sure we have a valid instance of BubbleTrigger
        if (BubbleTrigger.instance == null)
            return;

        // Get the current bubble count
        int count = BubbleTrigger.instance.bubbleList.Count;

        // Clamp to the array range if needed
        if (count < 0) count = 0;
        if (count >= bubbleCountSprites.Length)
            count = bubbleCountSprites.Length - 1;

        // Update the UI Image with the corresponding sprite
        bubbleImage.sprite = bubbleCountSprites[count];
    }
}