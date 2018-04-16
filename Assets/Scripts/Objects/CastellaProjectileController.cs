using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastellaProjectileController : MonoBehaviour {

	private GameObject target;
	private GameObject player;
	private GameObject castella;
	public CastellaController castellaC;
	private Transform targetPosition;
	private float speed;
	private float speedIncrement;

	public AudioClip sfxHurt;
	public AudioClip sfxDefeat;

	private Rigidbody rb;

	void Start () {
		player = GameObject.FindWithTag("Player");
		castella = GameObject.FindWithTag("Castella");
		castellaC = castella.GetComponent<CastellaController>();
		target = player;
		targetPosition = target.transform;

		speed = 15.0f;
		speedIncrement = 1.5f;

		rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		float step = speed * Time.deltaTime;
		transform.Translate(Vector3.forward * step);
		transform.LookAt(targetPosition);
		//transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
	}

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
            target = castella;
            targetPosition = target.transform;
            transform.LookAt(targetPosition);
            speed += speedIncrement;
        }

        if (other.gameObject.tag == "Castella") {
        	if (castellaC.projectileVolleys > 0) {
        		// Volley the projectile back to the player
        		if (target == castella) {
	            	castellaC.projectileVolleys--;
	            }

	            target = player;
	            targetPosition = target.transform;
	            transform.LookAt(targetPosition);
	            speed += speedIncrement;
	        } else {
	        	castellaC.health--;

	        	if (castellaC.health == 0) {
	        		// Castella defeated
	        		target.SetActive(false);

			        player.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
        			player.GetComponent<AudioSource>().PlayOneShot(sfxDefeat, 1f);
	        	} else {
		        	castellaC.projectileVolleys = 2 * (4 - castellaC.health);
		        	CastellaController castella = GameObject.FindWithTag("Castella").GetComponent<CastellaController>();
			        castella.StartCoroutine("AttackTimer");
			        Debug.Log("Set attack timer on Castella hurt");

			        player.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
        			player.GetComponent<AudioSource>().PlayOneShot(sfxHurt, 1f);
			    }

			    MakeSmoke();
		        Destroy(gameObject);
	        }
        }
    }

    public void MakeSmoke() {
        // Create smoke effect
        GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
        smoke.transform.position = transform.position;
        smoke.transform.localScale = new Vector3(2f, 2f, 2f); 
    }
}
