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

	public void setFocus(Transform t, float f)
	{
		StartCoroutine (changeFocus (t, f));
	}

	public void setInstantFocus(Transform t, Vector3 tempOffset)
	{
		StartCoroutine (changeInstantFocus(t,tempOffset));
	}

	IEnumerator changeInstantFocus(Transform newTarget, Vector3 tempOffset)
	{
		Transform oldTarget = target;
		Vector3 oldOffset = offset;
		oldTarget.GetComponentInParent<PlayerController> ().AllowMovement (false);
		target = newTarget;
		transform.position = target.position - tempOffset;
		offset = tempOffset;
		yield return new WaitForSeconds (4);
		oldTarget.GetComponentInParent<PlayerController> ().AllowMovement (true);
		target = oldTarget;
		offset = oldOffset;
		transform.position = target.position - offset;
	}

	IEnumerator changeFocus(Transform newTarget, float focusTime)
	{
		Transform oldTarget = target;
		oldTarget.GetComponentInParent<PlayerController> ().AllowMovement (false);
		cameraSpeed = focusTime;
		target = newTarget;
		yield return new WaitForSeconds (4);
		target = oldTarget;
		StartCoroutine (ChangeSpeedBack (focusTime));
	}

	IEnumerator ChangeSpeedBack(float focusTime)
	{
		cameraSpeed = 0.75f * focusTime;
		yield return new WaitForSeconds(1.0f * focusTime);
		target.GetComponentInParent<PlayerController> ().AllowMovement (true);
		yield return new WaitForSeconds(1.5f * focusTime);
		cameraSpeed = 0.1f;
	}
		
}
