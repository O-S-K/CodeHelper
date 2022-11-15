using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
    public class UIAnimate : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;
        public RectTransform RectT { get { return transform as RectTransform; } }
        public CanvasGroup CG
        {
            get
            {
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.GetComponent<CanvasGroup>();

                    if (canvasGroup == null)
                    {
                        canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    }
                }

                return canvasGroup;
            }
        }


        [System.Serializable]
        public class OnTransitionEvent : UnityEngine.Events.UnityEvent { }

        [System.Serializable]
        public class TransitionInfo
        {
            public enum Type
            {
                Fade,
                Swipe 
            }

            public bool animate = false;
            public Type animationType = Type.Fade;
            public float animationDuration = 0;
            public UIAnimation.Style animationStyle = UIAnimation.Style.Linear;
            public AnimationCurve animationCurve = null;
            public OnTransitionEvent onTransition = null;
        }

        [Space]
        public TransitionInfo showTransition = null;
        public TransitionInfo hideTransition = null;


        protected void Transition(TransitionInfo transitionInfo, bool back, bool immediate, bool show, System.Action cbOncompleted = null)
        {
            if (transitionInfo.animate)
            {
                // Make sure the screen is showing for the animation
                SetVisibility(true); 
                float animationDuration = immediate ? 0 : transitionInfo.animationDuration;

                switch (transitionInfo.animationType)
                {
                    case TransitionInfo.Type.Fade:
                        StartFadeAnimation(transitionInfo, show, animationDuration, cbOncompleted);
                        break;
                    case TransitionInfo.Type.Swipe:
                        StartSwipeAnimation(transitionInfo, show, back, animationDuration, cbOncompleted);
                        break; 
                }

                if (!show)
                {
                    if (immediate)
                    {
                        SetVisibility(false);
                    }
                }
            }
            else
            {
                // No animations, set the screen to active or de-active
                SetVisibility(show);
            }

            transitionInfo.onTransition.Invoke();
        }

        /// <summary>
        /// Starts the fade screen transition animation
        /// </summary>
        protected void StartFadeAnimation(TransitionInfo transitionInfo, bool show, float duration, System.Action cbOncompleted = null)
        {
            float fromAlpha = show ? 0f : 1f;
            float toAlpha = show ? 1f : 0f;

            UIAnimation anim = UIAnimation.Alpha(gameObject, fromAlpha, toAlpha, duration);
            anim.style = transitionInfo.animationStyle;
            anim.animationCurve = transitionInfo.animationCurve;
            anim.startOnFirstFrame = true;

            if (!show)
            {
                anim.OnAnimationFinished = (GameObject obj) =>
                {
                    cbOncompleted?.Invoke();
                    SetVisibility(false);
                };
            }
            else
            {
                cbOncompleted?.Invoke();
                anim.OnAnimationFinished = null;
            }

            anim.Play();
        }

        /// <summary>
        /// Starts the scale screen transition animation
        /// </summary>
        protected void StartScaleAnimation(TransitionInfo transitionInfo, bool show, float duration, System.Action cbOncompleted = null)
        {
            float screenWidth = RectT.rect.width;
            float to = 1;

            UIAnimation anim = UIAnimation.ScaleX(RectT, to, duration);
            anim.style = transitionInfo.animationStyle;
            anim.animationCurve = transitionInfo.animationCurve;
            anim.startOnFirstFrame = true;

            if (!show)
            {
                anim.OnAnimationFinished = (GameObject obj) =>
                {
                    cbOncompleted?.Invoke();
                    SetVisibility(false);
                };
            }
            else
            {
                cbOncompleted?.Invoke();
                anim.OnAnimationFinished = null;
            }

            anim.Play();
        }

        /// <summary>
        /// Starts the swipe screen transition animation
        /// </summary>
        protected void StartSwipeAnimation(TransitionInfo transitionInfo, bool show, bool back, float duration, System.Action cbOncompleted = null)
        {
            float screenWidth = RectT.rect.width;
            float fromX = 0f;
            float toX = 0f;

            if (show && back)
            {
                fromX = -screenWidth;
                toX = 0;
            }
            else if (show && !back)
            {
                fromX = screenWidth;
                toX = 0;
            }
            else if (!show && back)
            {
                fromX = 0;
                toX = screenWidth;
            }
            else if (!show && !back)
            {
                fromX = 0;
                toX = -screenWidth;
            }

            UIAnimation anim = UIAnimation.PositionX(RectT, fromX, toX, duration);

            anim.style = transitionInfo.animationStyle;
            anim.animationCurve = transitionInfo.animationCurve;
            anim.startOnFirstFrame = true;

            if (!show)
            {
                anim.OnAnimationFinished = (GameObject obj) =>
                {
                    cbOncompleted?.Invoke();
                    SetVisibility(false);
                };
            }
            else
            {
                cbOncompleted?.Invoke();
                anim.OnAnimationFinished = null;
            }

            anim.Play();
        }

        /// <summary>
        /// Sets the visibility.
        /// </summary>
        protected void SetVisibility(bool isVisible)
        {
            CG.alpha = isVisible ? 1f : 0f;
            CG.interactable = isVisible ? true : false;
            CG.blocksRaycasts = isVisible ? true : false;
        }
    }
}
