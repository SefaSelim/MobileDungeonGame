using UnityEngine;
using UnityEngine.UI;

public class ContentScaler : MonoBehaviour
{
    public float scrollSensitivity = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 3.0f;

    private RectTransform contentRectTransform;
    private ScrollRect scrollRect;
    private Canvas canvas;
    private RectTransform viewportRectTransform;

    void Start()
    {
        contentRectTransform = GetComponent<RectTransform>();
        scrollRect = GetComponentInParent<ScrollRect>();
        canvas = GetComponentInParent<Canvas>();
        viewportRectTransform = scrollRect.viewport;

        if (contentRectTransform == null || scrollRect == null || canvas == null || viewportRectTransform == null)
        {
            Debug.LogError("ContentScaler: Missing required components.");
        }
    }

    void Update()
    {
        if (contentRectTransform != null && scrollRect != null && canvas != null && viewportRectTransform != null)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0.0f)
            {
                // Get mouse position in screen space
                Vector2 mousePositionScreen = Input.mousePosition;

                // Convert screen position to viewport local position
                Vector2 mousePositionViewport;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRectTransform, mousePositionScreen, canvas.worldCamera, out mousePositionViewport);

                // Adjust for viewport's position within the canvas
                Vector3[] viewportCorners = new Vector3[4];
                viewportRectTransform.GetWorldCorners(viewportCorners);
                Vector2 viewportTopLeft = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, viewportCorners[0]);
                mousePositionViewport += (Vector2)viewportRectTransform.position - viewportTopLeft;

                // Convert to content space
                Vector2 mousePositionContent = mousePositionViewport + scrollRect.content.anchoredPosition;

                // Calculate new scale
                Vector3 newScale = contentRectTransform.localScale * (1f + scrollInput * scrollSensitivity);
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = 1f;

                // Calculate scale factor
                float scaleFactor = newScale.x / contentRectTransform.localScale.x;

                // Apply new scale
                contentRectTransform.localScale = newScale;

                // Adjust content position to zoom towards mouse
                Vector2 contentSpaceCoordinate = mousePositionContent - contentRectTransform.anchoredPosition;
                Vector2 newContentSpaceCoordinate = contentSpaceCoordinate * scaleFactor;
                Vector2 positionDelta = newContentSpaceCoordinate - contentSpaceCoordinate;

                contentRectTransform.anchoredPosition -= positionDelta;

                // Ensure scroll view updates its content
                Canvas.ForceUpdateCanvases();
                scrollRect.velocity = Vector2.zero;
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
            }
        }
    }
}