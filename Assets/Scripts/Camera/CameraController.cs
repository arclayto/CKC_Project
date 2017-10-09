using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public bool useOffset;

	private Vector3 offset;


	// Use this for initialization
	void Start () {
		if (!useOffset) {
			offset = target.position - transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position - offset;

		transform.LookAt (target);
	}
}
