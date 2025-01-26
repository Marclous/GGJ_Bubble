using UnityEngine;
using System.Collections.Generic;

public class BubbleTrigger : MonoBehaviour
{
    public Texture2D bubbleCursor;
    public Texture2D defaultCursor;
    public GameObject bubblePrefab;
    public GameObject postProcessObject;

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

        if (postProcessObject != null)
            postProcessObject.SetActive(false);
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
                    // Vector2 hotspot = new Vector2(bubbleCursor.width * 0.5f, bubbleCursor.height * 0.5f);
                    isBubbleCursor = true;
                }
                if (postProcessObject != null)
                    postProcessObject.SetActive(true);
            }
            else
            {
                SetDefaultCursor();
                if (postProcessObject != null)
                    postProcessObject.SetActive(false);
            }
        }

        if (isBubbleCursor && Input.GetMouseButtonDown(0))
        {
            if (bubbleList.Count >= maxBubbleCount)
                return;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            InstantiateBubble(mousePosition);

            SetDefaultCursor();
            if (postProcessObject != null)
                postProcessObject.SetActive(false);
        }

        if (isBubbleCursor) 
        {
            Time.timeScale = 0.1f;
        }
        else
        {
            Time.timeScale = 1f;
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
