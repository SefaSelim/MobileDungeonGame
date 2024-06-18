using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered the UI element.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited the UI element.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI element clicked.");
    }
}
