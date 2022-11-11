using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
	public class SpawnObject : UIMonoBehaviour
	{
		#region Properties

		protected RectTransform ParentRectT	{ get { return transform.parent as RectTransform; } }

		#endregion

		#region Public Methods

		public virtual void Spawned()
		{

		}

		#endregion

		#region Protected Methods

		protected void Die()
		{
			ObjectPool.ReturnObjectToPool(gameObject);
		}

		protected void FadeIn(float duration)
		{
			gameObject.GetComponent<CanvasGroup>().alpha = 0f;

			UIAnimation.Alpha(gameObject, 0f, 1f, duration).Play();
		}

		protected void FadeOut(float duration)
		{
			UIAnimation anim = UIAnimation.Alpha(gameObject, 1f, 0f, duration);

			anim.OnAnimationFinished += (GameObject obj) => 
			{
				Die();
			};

			anim.Play();
		}

		#endregion
	}
}
