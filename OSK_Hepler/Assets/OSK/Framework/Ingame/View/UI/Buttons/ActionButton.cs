using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    public Image border;

    public bool IsClick = false;
    public float scaleHover = 1.2f;

    public void OnPointerDown(PointerEventData eventData)
    {
        HoverIn();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HoverOut();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        HoverOut();
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    public void HoverIn()
    {
        Tweener.Instance.ScaleTo(transform, Vector3.one * scaleHover, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
        border.color = Color.yellow;
    }

    public void HoverOut()
    {
        Tweener.Instance.ScaleTo(transform, Vector3.one, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
        border.color = Color.white;
    }
}
