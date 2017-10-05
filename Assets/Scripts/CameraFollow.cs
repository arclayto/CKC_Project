using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//under construction
public class CameraFollow : MonoBehaviour {

	public Transform target;
	public Transform toTransform;
	public float speed;

	// Use this for initialization
	void Start () {
		toTransform.position = target.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Math.Abs (toTransform.position.x - target.position.x) > .005 && Math.Abs (toTransform.position.z - target.position.z) > .005) {
			toTransform.LookAt (target);
			toTransform.Translate (Vector3.forward * speed * Time.deltaTime);
		}
	}
}
