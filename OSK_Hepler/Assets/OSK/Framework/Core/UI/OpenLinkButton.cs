using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
	[RequireComponent(typeof(Button))]
	public class OpenLinkButton : MonoBehaviour
	{
		#region Inspector Variables

		public string url;

		#endregion

		#region Unity Methods

		private void Start()
		{
			gameObject.GetComponent<Button>().onClick.AddListener(() => { Application.OpenURL(url); });
		}

		#endregion
	}
}
