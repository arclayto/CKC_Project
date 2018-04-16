using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastellaController : MonoBehaviour {

	private GameObject target;
	private GameObject smoke;
	private float startY;
	private int health;
	private bool aggro;
	private float aggroRange;
	private bool cutscene;
	private int attackType;

	SpriteRenderer spriteRenderer;
    Animator animator;

    void Start() {
    	target = GameObject.FindWithTag("Player");
    	smoke = transform.GetChild(0).gameObject;
    	startY = transform.position.y;
    	health = 3;
        aggro = false;
		aggroRange = 30;
		cutscene = false;
		attackType = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        animator = GetComponent<Animator>();
    }

    void Update() {
    	transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(startY, startY + 1.0f, Mathf.PingPong(Time.time, 1.0f)), transform.position.z);
    }

	void FixedUpdate() {
		// Calculate player's position
        Vector3 direction = target.transform.position - transform.position;
        float magnitude = direction.magnitude;
        direction.Normalize();

        if (aggro == false && cutscene == false && magnitude < aggroRange) {
        	StartCoroutine("AppearTimer");
        	aggro = true;
        	cutscene = true;
        }
    }

    IEnumerator AppearTimer() {
        yield return new WaitForSeconds(0.5f);

        // Create smoke effect
        smoke.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(0.5f);

        // Laughing animation

        yield return new WaitForSeconds(3f);
        cutscene = false;

        StartCoroutine("AttackTimer");
    }

    IEnumerator AttackTimer() {
        yield return new WaitForSeconds(2.5f);

        if (attackType == 0) {
        	// First attack type: dead man's volley
	        GameObject projectile = (GameObject)Instantiate(Resources.Load("Castella Projectile"));
	        projectile.transform.position = transform.position;
	    } else if (attackType == 1) {
	    	// Second attack type: dark pillar
	    	GameObject pillar = (GameObject)Instantiate(Resources.Load("Castella Pillar Light"));
	        pillar.transform.position = target.transform.position + new Vector3(0f, -1f, 0f);
	    }

    	// After every attack, cycle to the next attack type
        if (attackType == 1) {
        	attackType = 0;
        } else {
        	attackType++;
        }
    }
}
