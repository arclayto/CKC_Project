using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastellaProjectileController : MonoBehaviour {

	private GameObject target;
	private GameObject player;
	private GameObject castella;
	private Transform targetPosition;
	private float speed;
	private float speedIncrement;

	private Rigidbody rb;

	void Start () {
		player = GameObject.FindWithTag("Player");
		castella = GameObject.FindWithTag("Castella");
		target = player;
		targetPosition = target.transform;

		speed = 15.0f;
		speedIncrement = 1f;

		rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		float step = speed * Time.deltaTime;
		transform.Translate(Vector3.forward * step);
		//transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
	}

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
        	Debug.Log("hit by player");
            target = castella;
            targetPosition = target.transform;
            transform.LookAt(targetPosition);
            speed += speedIncrement;
        }

        if (other.gameObject.tag == "Castella") {
        	Debug.Log("hit by castella");
            target = player;
            targetPosition = target.transform;
            transform.LookAt(targetPosition);
            speed += speedIncrement;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Castella Bounds") {
        	CastellaController castella = GameObject.FindWithTag("Castella").GetComponent<CastellaController>();
	        castella.StartCoroutine("AttackTimer");

	        Destroy(gameObject);
        }
    }
}
