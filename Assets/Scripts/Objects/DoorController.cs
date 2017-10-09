using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	public PlayerController player;

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
			Destroy(gameObject);
			player.SetEquppedItem(0);
		}
	}
}
