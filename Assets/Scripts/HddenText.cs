using UnityEngine;
using TMPro;

public class HddenText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    private bool isTriggered = false;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();

        if (text != null)
        {
            text.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isTriggered) return;

        Vector2 boxCenter = boxCol.bounds.center;
        Vector2 boxSize = boxCol.bounds.size;

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);

        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Bubble"))
            {

                isTriggered = true;
                if (text != null)
                    text.gameObject.SetActive(true);

                break;
            }
        }
    }
}
