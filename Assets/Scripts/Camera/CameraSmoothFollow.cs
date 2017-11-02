using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

	public Transform target;
	public bool lookAtTarget;
	public bool useOffset;
	public Vector3 offset;

	private Vector3 velocity = Vector3.zero;
	private float cameraSpeed;

	Vector3 smoothed;

	void Start () {
		//generally keep checked
		if (!useOffset) {
			offset = target.position - transform.position;
		}

		if (lookAtTarget) {
			transform.LookAt (target);
		}

		cameraSpeed = 0.1f;
	}

	void Update()
	{
		Vector3 desired = target.position - offset;
		smoothed = Vector3.SmoothDamp(transform.position, desired, ref velocity, cameraSpeed);
		transform.position = smoothed;

		/*if (Input.GetButton("C")) {
			offset = new Vector3(-offset.z, offset.y, -offset.x);
			transform.position = target.position - transform.position;
			transform.LookAt (target);
		}*/

		//add this if you're ok with non-fixed rotation
		//for our purposes, it's ugly
		//transform.LookAt (target);
	}

	public void setFocus(Transform t)
	{
		StartCoroutine (changeFocus (t));
	}

	IEnumerator changeFocus(Transform newTarget)
	{
		Transform oldTarget = target;
		oldTarget.GetComponentInParent<PlayerController> ().AllowMovement (false);
		cameraSpeed = 0.5f;
		target = newTarget;
		yield return new WaitForSeconds (4);
		target = oldTarget;
		StartCoroutine (ChangeSpeedBack ());
	}

	IEnumerator ChangeSpeedBack()
	{
		yield return new WaitForSeconds(0.55f);
		target.GetComponentInParent<PlayerController> ().AllowMovement (true);
		cameraSpeed = 0.1f;
	}
}
