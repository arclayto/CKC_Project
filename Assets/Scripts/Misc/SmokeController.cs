using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour {

	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}
	
	void Update() {
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            // Animation complete, destroy gameobject
            Destroy(gameObject);
        }
	}
}
