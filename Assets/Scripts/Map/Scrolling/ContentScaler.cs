using UnityEngine;
using UnityEngine.UI;

public class ContentScaler : MonoBehaviour
{
    public float scrollSensitivity = 0.1f; // Sensitivity of the scroll input
    public float minScale = 0.1f; // Minimum scale limit
    public float maxScale = 3.0f; // Maximum scale limit

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("ImageScaler script is not attached to a GameObject with a RectTransform component.");
        }
    }

    void Update()
    {
        if (rectTransform != null)
        {
            // Get the mouse scroll input
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0.0f)
            {
                // Calculate the new scale
                Vector3 newScale = rectTransform.localScale + Vector3.one * scrollInput * scrollSensitivity;

                // Clamp the scale to the minimum and maximum limits
                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);

                // Apply the new scale
                rectTransform.localScale = newScale;
            }
        }
    }
}
