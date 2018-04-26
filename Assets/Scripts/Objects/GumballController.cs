using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumballController : MonoBehaviour {

	public AudioClip sfxBumper;

    Vector3 startPosition;
	Rigidbody rb;
	AudioSource audioSource;
	GameObject player;

	void Start() {
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		player = GameObject.FindWithTag("Player");
		Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(gameObject.transform.GetChild(0).gameObject.GetComponent<Collider>(), GetComponent<Collider>());

        startPosition = transform.position;
	}

	void FixedUpdate() {
		//if (rb.velocity.magnitude <= 0.1f && rb.velocity.magnitude > 0.0f) {
		//	rb.velocity = Vector3.zero;
		//	rb.isKinematic = true;
		//}
	}

	private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && rb.velocity.magnitude <= 4.0f) {
            rb.velocity = Vector3.zero;
			rb.isKinematic = true;
        }

        if (other.gameObject.tag == "GumballSwitch") {
        	GumballSwitchController gumballSwitch = other.gameObject.GetComponent<GumballSwitchController>();
        	if (gumballSwitch.isActivated == false) {
        		other.transform.Translate(0.0f, -0.19f, 0.0f);
        		gumballSwitch.isActivated = true;
        		gumballSwitch.ActivateTarget();
        	}
        }
			
    }

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
            rb.isKinematic = false;
            Vector3 dir = (transform.position - other.transform.position).normalized;
            dir.y = 0.0f;
            rb.AddForce(dir * 7.5f, ForceMode.Impulse);
        }

		if (other.gameObject.tag == "Bumper") {
			rb.isKinematic = false;
			Vector3 dir = (transform.position - other.transform.position).normalized;
			dir.y = 0.0f;
			rb.AddForce(dir * 10.0f, ForceMode.Impulse);

			audioSource.pitch = (Random.Range(0.9f, 1.1f));
			audioSource.PlayOneShot(sfxBumper, 1f);
		}

        if (other.gameObject.tag == "Kill Zone") {
			rb.velocity = new Vector3(0f, -0.11f, 0);
			transform.position = startPosition;
        }

        if (other.gameObject.tag == "Target") {
            TargetController tc = other.gameObject.GetComponent<TargetController>();
            tc.Activate();
        }
    }

	private void OnTriggerStay(Collider other){
		if (other.tag == "Tornado") {

			rb.AddForce(new Vector3(0f,7f,0f));
		}
	
	}
}
