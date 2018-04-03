using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour {

	public GameObject treasure;
	public bool destroyTreasure = false;
	public bool triggerAnimation = false;

	void Start () {
		
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
			if (triggerAnimation) {
				Animator animTreasure = treasure.GetComponent<Animator> ();
				animTreasure.SetTrigger ("objectTrigger"); 					//use objectTrigger for triggering Animation from objects
			}
        	else if (destroyTreasure) {
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
			if (triggerAnimation) {
				Animator animTreasure = treasure.GetComponent<Animator> ();
				animTreasure.SetTrigger ("objectTrigger");
			}
			else if (destroyTreasure) {
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
