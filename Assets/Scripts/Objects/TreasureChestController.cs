using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour {

	public GameObject treasure;

	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
	        treasure.SetActive(true);
	        treasure.transform.parent = null;

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
	        treasure.SetActive(true);
	        treasure.transform.parent = null;

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }
    }
}
