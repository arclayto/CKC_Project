using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSunController : MonoBehaviour {

	public Transform target;

	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	void Update()
	{
		// Rotate the camera every frame so it keeps looking at the target
		transform.LookAt(target);
	}
	// Update is called once per frame
	void LateUpdate () {
	//	transform.position = player.transform.position + offset;
	}
}
