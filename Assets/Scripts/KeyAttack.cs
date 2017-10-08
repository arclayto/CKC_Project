using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAttack : MonoBehaviour {

	MeshCollider keyCollider;
	Animator animator;

	// Use this for initialization
	void Start () {
		keyCollider = GetComponent<MeshCollider>();
		animator = transform.parent.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		animator = transform.parent.GetComponent<Animator>();
	}

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
}
