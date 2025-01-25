using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MinimapIconController : MonoBehaviour
{
    [Header("Minimap UI")]
    public RectTransform minimapRect;
    public float mapMinX, mapMaxX;
    public float mapMinY, mapMaxY;
    public GameObject bubbleIconPrefab;

    private List<Transform> bubbleList;

    private float mapWidth;
    private float mapHeight;

    void Start()
    {
        mapWidth = minimapRect.rect.width;
        mapHeight = minimapRect.rect.height;

        bubbleList = BubbleTrigger.instance.bubbleList.Select(b => b.transform).ToList();
    }

    void Update()
    {
        foreach (Transform bubble in bubbleList)
        {
            UpdateBubbleIcon(bubble);
        }
    }

    void UpdateBubbleIcon(Transform bubble)
    {
        GameObject iconObj = GameObject.Find("BubbleIcon_" + bubble.name);
        if (iconObj == null)
        {
            iconObj = Instantiate(bubbleIconPrefab, minimapRect);
            iconObj.name = "BubbleIcon_" + bubble.name;
        }

        Vector2 minimapPos = WorldToMinimapPos(bubble.position);

        bool outOfBoundX = (minimapPos.x < 0 || minimapPos.x > mapWidth);
        bool outOfBoundY = (minimapPos.y < 0 || minimapPos.y > mapHeight);
        bool isOutOfBound = (outOfBoundX || outOfBoundY);

        Vector2 finalPos;
        if (isOutOfBound)
        {
            float clampedX = Mathf.Clamp(minimapPos.x, 0, mapWidth);
            float clampedY = Mathf.Clamp(minimapPos.y, 0, mapHeight);
            finalPos = new Vector2(clampedX, clampedY);
        }
        else
        {
            finalPos = minimapPos;
        }

        RectTransform iconRect = iconObj.GetComponent<RectTransform>();
        iconRect.anchoredPosition = finalPos;
    }

    Vector2 WorldToMinimapPos(Vector3 worldPos)
    {
        float u = (worldPos.x - mapMinX) / (mapMaxX - mapMinX);
        float v = (worldPos.y - mapMinY) / (mapMaxY - mapMinY);

        float x = u * mapWidth;
        float y = v * mapHeight;

        return new Vector2(x, y);
    }
}

