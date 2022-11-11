using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace OSK
{
	public class UIScreen : UIMonoBehaviour
	{
		#region Classes

		[System.Serializable]
        public class OnTransitionEvent : UnityEngine.Events.UnityEvent {}

		[System.Serializable]
        public class TransitionInfo
		{
			public enum Type
			{
				Fade,
				Swipe
			}

			public bool					animate					= false;
			public Type					animationType			= Type.Fade;
			public float				animationDuration		= 0;
			public UIAnimation.Style	animationStyle			= UIAnimation.Style.Linear;
			public AnimationCurve		animationCurve			= null;
			public OnTransitionEvent	onTransition			= null;
		}

		#endregion

		#region Inspector Variables

		public string id = "";
		[Space]
		public TransitionInfo	showTransition = null;
		public TransitionInfo	hideTransition = null;

		#endregion

		#region Properties

		public string Id { get { return id; } }

		#endregion

		#region Public Methods

		public virtual void Initialize()
		{
		}

		public virtual void Show(bool back, bool immediate)
		{
			Transition(showTransition, back, immediate, true);
		}

		public virtual void Hide(bool back, bool immediate)
		{
			Transition(hideTransition, back, immediate, false);
		}

		#endregion

		#region Private Methods

		private void Transition(TransitionInfo transitionInfo, bool back, bool immediate, bool show)
		{
			if (transitionInfo.animate)
			{
				// Make sure the screen is showing for the animation
				SetVisibility(true);

				float animationDuration = immediate ? 0 : transitionInfo.animationDuration;

				switch (transitionInfo.animationType)
				{
					case TransitionInfo.Type.Fade:
						StartFadeAnimation(transitionInfo, show, animationDuration);
						break;
					case TransitionInfo.Type.Swipe:
						StartSwipeAnimation(transitionInfo, show, back, animationDuration);
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
		private void StartFadeAnimation(TransitionInfo transitionInfo, bool show, float duration)
		{
			float fromAlpha	= show ? 0f : 1f;
			float toAlpha	= show ? 1f : 0f;

			UIAnimation anim = UIAnimation.Alpha(gameObject, fromAlpha, toAlpha, duration);

			anim.style				= transitionInfo.animationStyle;
			anim.animationCurve		= transitionInfo.animationCurve;
			anim.startOnFirstFrame	= true;

			if (!show)
			{
				anim.OnAnimationFinished = (GameObject obj) => 
				{
					SetVisibility(false);
				};
			}
			else
			{
				anim.OnAnimationFinished = null;
			}

			anim.Play();
		}

		/// <summary>
		/// Starts the swipe screen transition animation
		/// </summary>
		private void StartSwipeAnimation(TransitionInfo transitionInfo, bool show, bool back, float duration)
		{
			float screenWidth	= RectT.rect.width;
			float fromX			= 0f;
			float toX			= 0f;

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

			anim.style				= transitionInfo.animationStyle;
			anim.animationCurve		= transitionInfo.animationCurve;
			anim.startOnFirstFrame	= true;

			if (!show)
			{
				anim.OnAnimationFinished = (GameObject obj) => 
				{
					SetVisibility(false);
				};
			}
			else
			{
				anim.OnAnimationFinished = null;
			}

			anim.Play();
		}

		/// <summary>
		/// Sets the visibility.
		/// </summary>
		private void SetVisibility(bool isVisible)
		{
			CG.alpha			= isVisible ? 1f : 0f;
			CG.interactable		= isVisible ? true : false;
			CG.blocksRaycasts	= isVisible ? true : false;
		}
		#endregion
	}
}
