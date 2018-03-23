using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumballController : MonoBehaviour {

	Rigidbody rb;
	GameObject player;

	void Start() {
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindWithTag("Player");
		Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>(), GetComponent<Collider>());
	}

	void FixedUpdate() {
		if (rb.velocity.magnitude <= 0.1f && rb.velocity.magnitude > 0.0f) {
			rb.velocity = Vector3.zero;
			rb.isKinematic = true;
			Debug.Log("Stop!");
		}
	}

	private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && rb.velocity.magnitude <= 0.5f) {
            rb.velocity = Vector3.zero;
			rb.isKinematic = true;
			Debug.Log("Stop!");
        }
    }

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
            rb.isKinematic = false;
            Vector3 dir = (transform.position - other.transform.position).normalized;
            dir.y = 0.0f;
            rb.AddForce(dir * 7.5f, ForceMode.Impulse);
            Debug.Log("Hit!");
        }
    }
}
