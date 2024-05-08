using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Text text;
    public Appearer appearer;
    public Image bg;
    public Color normalColor, hoverColor;
    
    // public string activatesScene;
    public UnityEvent action;
    public bool hidesOnClick = true;
    
    public bool isRotating = true;

    private bool done;
    private bool hovered;

    private void Start()
    {
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (done) return;

        if(hidesOnClick)
        {
            appearer.Hide();
            done = true;
        }

        // if (!string.IsNullOrEmpty(activatesScene))
        //     SceneChanger.Instance.ChangeScene(activatesScene);

        if (action != null) action.Invoke();

        //Invoke("RemoveHover", 0.25f);

        AudioManager.Instance.Lowpass(false);
        AudioManager.Instance.Highpass(false);
    }

    private void RemoveHover()
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (done) return;

        hovered = true;

        if (Tweener.Instance)
        {
            Tweener.Instance.ScaleTo(transform, Vector3.one * 1.1f, 0.2f, 0f, TweenEasings.BounceEaseOut);
            Tweener.Instance.ScaleTo(text.transform, Vector3.one * 0.9f, 0.3f, 0f, TweenEasings.BounceEaseOut);

            bg.color = hoverColor;
            text.color = hoverColor;
        }
        if(isRotating)
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (done) return;

        hovered = false;

        if (Tweener.Instance)
        {
            Tweener.Instance.ScaleTo(transform, Vector3.one, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
            Tweener.Instance.ScaleTo(text.transform, Vector3.one, 0.1f, 0f, TweenEasings.QuadraticEaseOut);

            bg.color = normalColor;
            text.color = normalColor;
        }

        if(isRotating)
            transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public bool IsHovered()
    {
        return hovered;
    }
}
