using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OSK
{
    public class Popup : UIMonoBehaviour
    {
        public bool canAndroidBackClose = true;

        [Header("Anim Settings")]
        [SerializeField] protected float animDuration;
        [SerializeField] protected AnimationCurve animCurve;
        [SerializeField] protected RectTransform animContainer;



        private bool isInitialized;
        private bool isShowing;
        private PopupClosed callback;



        public bool CanAndroidBackClose { get { return canAndroidBackClose; } }


        public delegate void PopupClosed(bool cancelled, object[] outData);


        public virtual void Initialize()
        {

        }

        public void Show()
        {
            Show(null, null);
        }

        public void Show(object[] inData, PopupClosed callback)
        {
            this.callback = callback;

            if (isShowing)
            {
                return;
            }

            isShowing = true;

            // Show the popup object
            gameObject.SetActive(true);

            // Start the popup show animations
            UIAnimation anim = null;

            anim = UIAnimation.Alpha(gameObject, 0f, 1f, animDuration);
            anim.style = UIAnimation.Style.EaseOut;
            anim.startOnFirstFrame = true;
            anim.Play();

            anim = UIAnimation.ScaleX(animContainer, 0f, 1f, animDuration);
            anim.style = UIAnimation.Style.Custom;
            anim.animationCurve = animCurve;
            anim.startOnFirstFrame = true;
            anim.Play();

            anim = UIAnimation.ScaleY(animContainer, 0f, 1f, animDuration);
            anim.style = UIAnimation.Style.Custom;
            anim.animationCurve = animCurve;
            anim.startOnFirstFrame = true;
            anim.Play();

            OnShowing(inData);
        }

        public void Hide(bool cancelled)
        {
            Hide(cancelled, null);
        }

        public void Hide(bool cancelled, object[] outData)
        {
            if (!isShowing)
            {
                return;
            }

            isShowing = false;

            if (callback != null)
            {
                callback(cancelled, outData);
            }

            // Start the popup hide animations
            UIAnimation anim = null;

            anim = UIAnimation.Alpha(gameObject, 1f, 0f, animDuration);
            anim.style = UIAnimation.Style.EaseOut;
            anim.startOnFirstFrame = true;

            anim.OnAnimationFinished += (GameObject target) =>
            {
                gameObject.SetActive(false);
            };

            anim.Play();

            OnHiding();
        }

        public virtual void OnShowing(object[] inData)
        {
        }

        public virtual void OnHiding()
        {
            PopupManager.Instance.OnPopupHiding(this);
        }
    }
}