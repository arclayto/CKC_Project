using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMageController : MonoBehaviour {

	public float aggroRange;
	public float teleportRange;
	public GameObject target;
    public GameObject nextMage;

	private GameObject lightningSpell;
	private bool aggro;
	Rigidbody rb;
	SpriteRenderer spriteRenderer;
    Animator animator;

	void Start() {
		lightningSpell = null;
		aggro = false;

        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
		animator.Play("LightningMageCast", -1, 0.0f);
	}
	
	void FixedUpdate() {
		// Calculate player's position
        Vector3 direction = target.transform.position - transform.position;
        float magnitude = direction.magnitude;
        direction.Normalize();

        if (aggro == false && magnitude < aggroRange) {
            // Become aggressive when player is in range
            aggro = true;
        }

        if (aggro == true && magnitude > aggroRange) {
            // Become passive when player is out of range
            aggro = false;
        }

        if (aggro) {
        	// Cast lightning spell on target if not already cast
        	if (lightningSpell == null) {
	        	lightningSpell = (GameObject)Instantiate(Resources.Load("Lightning"));
	        	lightningSpell.GetComponent<LightningController>().target = target;

	        	if (!animator.GetCurrentAnimatorStateInfo(0).IsName("LightningMageCast")) {
	                animator.Play("LightningMageCast", -1, 0.0f);
	            }
	        }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("LightningMageCast")) {
        	if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                animator.Play("LightningMageCast", -1, 0.0f);
            }
        }

        if (magnitude < teleportRange) {
            // Disappear when player gets too close
            if (lightningSpell != null) {
	            Destroy(lightningSpell);
	        }

            if (nextMage != null) {
                // "Teleport" to the next location if it
                nextMage.SetActive(true);

                // Create smoke effect
                GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
                smoke.transform.position = nextMage.transform.position;
            }

            Destroy(gameObject);

            // Create smoke effect
            MakeSmoke();
        }
	}

	public void MakeSmoke() {
        // Create smoke effect
        GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
        smoke.transform.position = transform.position;
    }
}
