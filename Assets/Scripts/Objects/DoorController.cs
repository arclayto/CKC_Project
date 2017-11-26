﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
	
	public PlayerController player;
	public AudioClip sfxOpen;

	// Use this for initialization
	void Start () {
			
	}

	// Update is called once per frame
	void Update () {
			
	}

	//opening door by contact with player while holding key
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && player.GetEquippedItem () == 1) {
			//animation for door goes here
			//destroy door as placeholder for now
			Animator animator = transform.GetComponentInParent<Animator>();
			animator.SetTrigger("Open");
			player.SetEquippedItem (0);

			GetComponent<LevelLoadTrigger>().initiateLoad();

			col.gameObject.GetComponent<AudioSource>().pitch = 1.5f;
			col.gameObject.GetComponent<AudioSource>().time = 0.5f;
        	col.gameObject.GetComponent<AudioSource>().PlayOneShot(sfxOpen, 0.5f);
		}
	}
}
