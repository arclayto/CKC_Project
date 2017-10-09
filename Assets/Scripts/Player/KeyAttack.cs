using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    /*
	void OnCollisionEnter(Collision col) {
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
			if (col.gameObject.tag == "Enemy") {
				Destroy (col.gameObject);
			}
		}
	}
	void OnCollisionStay(Collision col) {
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
			if (col.gameObject.tag == "Enemy") {
				Destroy (col.gameObject);
			}
		}
	}
    */
}
