using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour {

	public GameObject treasure;
	public bool destroyTreasure = false;

	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
        	if (destroyTreasure) {
        		treasure.SetActive(false);
        	}
        	else {
		        treasure.SetActive(true);
		        treasure.transform.parent = null;
	    	}

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
	        if (destroyTreasure) {
        		treasure.SetActive(false);
        	}
        	else {
		        treasure.SetActive(true);
		        treasure.transform.parent = null;
	    	}

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }
    }
}
