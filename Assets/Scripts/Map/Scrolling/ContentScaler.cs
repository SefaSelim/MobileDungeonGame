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

                // Convert to content local position
                Vector2 mousePositionContent;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(contentRectTransform, mousePositionScreen, canvas.worldCamera, out mousePositionContent);

                // Calculate new scale
                Vector3 newScale = contentRectTransform.localScale * (1f + scrollInput * scrollSensitivity);
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = 1f;

                // Calculate scale factor
                float scaleFactor = newScale.x / contentRectTransform.localScale.x;

                // Calculate the position delta
                Vector2 mouseDelta = mousePositionContent - (Vector2)contentRectTransform.InverseTransformPoint(contentRectTransform.position);
                Vector2 positionDelta = mouseDelta - mouseDelta * scaleFactor;

                // Apply new scale
                contentRectTransform.localScale = newScale;

                // Adjust content position to zoom towards mouse
                contentRectTransform.anchoredPosition += positionDelta;

                // Ensure scroll view updates its content
                Canvas.ForceUpdateCanvases();
                scrollRect.velocity = Vector2.zero;
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
            }
        }
    }
}