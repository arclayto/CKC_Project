using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

	public Transform target;
	public bool lookAtTarget;
	public bool useOffset;
	public Vector3 offset;

	private Vector3 velocity = Vector3.zero;
	Vector3 smoothed;

	void Start () {
		//generally keep checked
		if (!useOffset) {
			offset = target.position - transform.position;
		}

		if (lookAtTarget) {
			transform.LookAt (target);
		}
	}

	void Update()
	{
		Vector3 desired = target.position - offset;
		smoothed = Vector3.SmoothDamp(transform.position, desired, ref velocity, 0.1f);
		transform.position = smoothed;

		/*if (Input.GetButton("C")) {
			offset = new Vector3(-offset.y, offset.y, -offset.z);
			transform.LookAt (target);
		}*/

		//add this if you're ok with non-fixed rotation
		//for our purposes, it's ugly
		//transform.LookAt (target);
	}
}
