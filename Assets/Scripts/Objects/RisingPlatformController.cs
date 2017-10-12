using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatformController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Animator animator = transform.GetComponentInChildren<Animator>();
			animator.SetTrigger("PlatformUp");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Animator animator = transform.GetComponentInChildren<Animator>();
			animator.SetTrigger("PlatformDown");
		}
	}
}
