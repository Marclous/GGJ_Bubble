using UnityEngine;
using System.Collections.Generic;

public class BubbleTrigger : MonoBehaviour
{
    public Texture2D bubbleCursor;
    public Texture2D defaultCursor;
    public GameObject bubblePrefab;

    public int maxBubbleCount = 3;

    private bool isBubbleCursor = false;

    public List<Bubble> bubbleList = new List<Bubble>();

    public static BubbleTrigger instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        if (defaultCursor != null)
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isBubbleCursor)
            {
                if (bubbleCursor != null)
                {
                    Cursor.SetCursor(bubbleCursor, Vector2.zero, CursorMode.Auto);
                    isBubbleCursor = true;
                }
            }
            else
                SetDefaultCursor();
        }

        if (isBubbleCursor && Input.GetMouseButtonDown(0))
        {
            if (bubbleList.Count >= maxBubbleCount)
                return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            InstantiateBubble(mousePosition);

            SetDefaultCursor();
        }
    }

    private void SetDefaultCursor()
    {
        if (defaultCursor != null)
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        isBubbleCursor = false;
    }

    private void InstantiateBubble(Vector3 position)
    {
        GameObject bubbleObj = Instantiate(bubblePrefab, position, Quaternion.identity);
        Bubble newBubble = bubbleObj.GetComponent<Bubble>();

        bubbleList.Add(newBubble);
    }
}
