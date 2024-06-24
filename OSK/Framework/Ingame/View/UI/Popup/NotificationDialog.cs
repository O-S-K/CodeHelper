using System;
using DG.Tweening;
using OSK;
using TMPro;
using UnityEngine;

public class NotificationDialog : OSK.Dialog
{
    [SerializeField] private RectTransform pivotTransform;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private CanvasGroup canvasGroup;

    // public static NotificationDialog Create(string path)
    // {
    //     var popup = Instantiate(Resources.Load<NotificationDialog>(path), DialogManager.Instance.parrentAllDialog);
    //     popup.transform.localPosition = Vector3.zero;
    //     popup.name = path;
    //     popup.Show();
    //     return popup;
    // }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Show(Action onShow = null)
    {
        base.Show(onShow);
        pivotTransform.DOAnchorPosY(0, 0);
    }

    public void ShowNotification(string message, float delayHide, float pointYSpawn, float moveY)
    {
        textMeshPro.text = message;

        canvasGroup.DOFade(1, 0.1f);
        pivotTransform.DOAnchorPosY(pointYSpawn, 0);
        pivotTransform.DOAnchorPosY(moveY, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            canvasGroup.DOFade(0, 0.2f).SetDelay(delayHide).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}