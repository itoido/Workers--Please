using UnityEngine;
using UnityEngine.EventSystems;

public class WindowFocusHandler : MonoBehaviour,
    IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}