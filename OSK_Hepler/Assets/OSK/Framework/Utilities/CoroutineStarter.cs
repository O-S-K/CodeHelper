using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK 
{
	public class CoroutineStarter : MonoBehaviour
	{ 

		public static void Start(IEnumerator routine)
		{
			new GameObject("routine").AddComponent<CoroutineStarter>().RunCoroutine(routine);
		}
		 

		private void RunCoroutine(IEnumerator routine)
		{
			StartCoroutine(RunCoroutineHelper(routine));
		}

		private IEnumerator RunCoroutineHelper(IEnumerator routine)
		{
			yield return routine; 
			Destroy(gameObject);
		}
		 
	}
}
