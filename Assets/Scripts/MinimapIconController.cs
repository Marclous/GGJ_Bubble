using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Camera))]
public class MinimapIconController : MonoBehaviour
{
    [Header("Minimap UI")]
    public RectTransform iconsParent;

    [Tooltip("Bubble Dot Prefab")]
    public GameObject bubbleIconPrefab;

    private Camera minimapCam;
    private List<Transform> bubbleList = new List<Transform>();
    private List<GameObject> spawnedDots = new List<GameObject>();

    void Start()
    {
        minimapCam = GetComponent<Camera>();
    }

    void Update()
    {
        ClearSpawnedIcons();

        bubbleList = BubbleTrigger.instance.bubbleList.Select(b => b.transform).ToList();

        foreach (Transform bubble in bubbleList)
            CheckBubblesOutOfCamera(bubble);
    }

    private void ClearSpawnedIcons()
    {
        foreach (var icon in spawnedDots)
            Destroy(icon);
        spawnedDots.Clear();
    }

    private void CheckBubblesOutOfCamera(Transform bubble)
    {
        Vector3 camPos = minimapCam.transform.position;
        float halfHeight = minimapCam.orthographicSize;
        float halfWidth = halfHeight * minimapCam.aspect;

        float left = camPos.x - halfWidth;
        float right = camPos.x + halfWidth;
        float bottom = camPos.y - halfHeight;
        float top = camPos.y + halfHeight;

        Vector2 bubblePos2D = bubble.position;

        bool isOutOfView = (bubblePos2D.x < left || bubblePos2D.x > right ||
                            bubblePos2D.y < bottom || bubblePos2D.y > top);

        // The bubble is in the bound
        if (!isOutOfView) return;

        Vector2 offsetWorld = bubblePos2D - new Vector2(camPos.x, camPos.y);

        float uiWidth = iconsParent.rect.width;
        float uiHeight = iconsParent.rect.height;

        float scaleX = (uiWidth * 0.5f) / halfWidth;
        float scaleY = (uiHeight * 0.5f) / halfHeight;

        Vector2 offsetUI = new Vector2(offsetWorld.x * scaleX, offsetWorld.y * scaleY);

        float halfW = uiWidth * 0.5f;
        float halfH = uiHeight * 0.5f;

        float clampedX = Mathf.Clamp(offsetUI.x, -halfW, halfW);
        float clampedY = Mathf.Clamp(offsetUI.y, -halfH, halfH);
        Vector2 finalPos = new Vector2(clampedX, clampedY);

        GameObject iconObj = Instantiate(bubbleIconPrefab, iconsParent);
        RectTransform iconRect = iconObj.GetComponent<RectTransform>();
        iconRect.anchoredPosition = finalPos;

        spawnedDots.Add(iconObj);
    }
}
