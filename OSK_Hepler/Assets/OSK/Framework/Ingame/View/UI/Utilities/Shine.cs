using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine : MonoBehaviour {

	public float distance = 0.1f;
	public Transform mirrorParent;
	public bool checkRotation = false;
	public Vector3 focus = Vector3.up * 10f;
	private Vector3 originalPos;

	private void Start () {
		originalPos = transform.localPosition;
	}
	
	private void Update () {
		Vector3 direction = (focus - transform.position).normalized;
		direction.z = originalPos.z;
		direction.x = mirrorParent ? mirrorParent.localScale.x * direction.x : direction.x;

		if (checkRotation) {
			float angle = transform.parent.rotation.eulerAngles.z;
			float aMod = Mathf.Sign (transform.parent.lossyScale.x);
			direction = Quaternion.Euler(new Vector3(0, 0, -angle * aMod)) * direction;
		}

		transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPos + direction * distance, 0.1f);
	}
}
