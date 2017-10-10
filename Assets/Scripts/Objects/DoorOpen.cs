using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorOpen : MonoBehaviour {

	private Animator animator;
	private bool doorOpen;

	// Use this for initialization
	void Start () {
		doorOpen = false;
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			//animation for door goes here
			//destroy door as placeholder for now
			//Destroy(gameObject);
			//player.SetEquippedItem (0);
		}
	}

	void SetAnimation(string direction){
		animator.SetTrigger (direction);
	}
		
	// Update is called once per frame
	void Update () {
			
	}
}
