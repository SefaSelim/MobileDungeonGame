using UnityEngine;
using TMPro;

public class AdjustTextHeight : MonoBehaviour
{
    public TextMeshProUGUI historyText;
    public RectTransform contentRectTransform;
    public float padding = 10f; // İstediğiniz bir padding değeri

    private string lastText;

    void Start()
    {
        AdjustHeight();
        lastText = historyText.text;
    }

    void Update()
    {
        if (historyText.text != lastText)
        {
            AdjustHeight();
            lastText = historyText.text;
        }
    }

    void AdjustHeight()
    {
        // TextMeshPro'nun tercih edilen yüksekliğini hesaplayın
        historyText.ForceMeshUpdate();
        float preferredHeight = historyText.GetRenderedValues(false).y;

        // Yüksekliği, metnin tercih edilen yüksekliği ve padding kullanarak ayarlayın
        RectTransform rt = historyText.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, preferredHeight + padding);

        // Content'in boyutunu da ayarlayın
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, preferredHeight + padding);
    }
}
