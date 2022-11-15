using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
	[RequireComponent(typeof(Button))]
	public class ClickableListItem : MonoBehaviour
	{
		private Button uiButton;
		public int							Index				{ get; set; }
		public object						Data				{ get; set; }
		public System.Action<int, object>	OnListItemClicked	{ get; set; }

		/// <summary>
		/// Gets the Button component attached to this GameObject
		/// </summary>
		private Button UIButton
		{
			get
			{
				if (uiButton == null)
				{
					uiButton = gameObject.GetComponent<Button>();
				}

				return uiButton;
			}
		}

		private void Start()
		{
			if (UIButton != null)
			{
				UIButton.onClick.AddListener(OnButtonClicked);
			}
			else
			{
				Debug.LogError("[ClickableListItem] There is no Button component on this GameObject.");
			}
		}

		private void OnButtonClicked()
		{
			if (OnListItemClicked != null)
			{
				OnListItemClicked(Index, Data);
			}
			else
			{
				Debug.LogWarning("[ClickableListItem] OnListItemClicked has not been set on object " + gameObject.name);
			}
		}
	}
}
