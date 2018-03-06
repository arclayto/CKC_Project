using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour {

	public Transform[] waypoints;
	public float speed = 0.002f;
	private int current = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (current >= waypoints.Length) {
			current = 0;
		}

		if (transform.position.y != waypoints [current].transform.position.y) {
			transform.position = Vector3.MoveTowards (transform.position, waypoints [current].transform.position, speed * Time.deltaTime);
		}

		if (transform.position.y == waypoints [current].transform.position.y) {
			current++;
		}
	}
}
