using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour {

    public float maxSpeed;
    public float gravityForce;
    public float aggroRange;
    public float maxAggroRange;
    public GameObject target;
    public AudioClip sfxDefeat;

    private Vector3 moveDirection;
    private bool aggro;
    private Vector3 velocity;
    Rigidbody rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start() {
        aggro = false;

        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate() {
        // Flip enemy horizontally based on x relative to player
        if (transform.position.x < target.transform.position.x) {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > target.transform.position.x) {
            spriteRenderer.flipX = true;
        }

        // Calculate player's position
        Vector3 direction = target.transform.position - transform.position;
        float magnitude = direction.magnitude;
        direction.Normalize();

        velocity = direction * maxSpeed;

        if (aggro == false && magnitude < aggroRange) {
            // Become aggressive when player is in range
            aggro = true;
        }

        if (aggro == true && magnitude > maxAggroRange) {
            // Become passive when player is out of chasing range
            aggro = false;
        }

        if (aggro) {
            // Move towards player when in aggro range
            rb.AddForce(new Vector3(velocity.x, rb.velocity.y, velocity.z), ForceMode.Force);
        }

        // Idle & Walk animation aggro
        if (aggro == false) {
            // Idle animation when not aggro'd, standing around
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BellIdle")) {
                animator.Play("BellIdle", -1, 0.0f);
            }
        } else {
            // Walk animation when aggro'd, chasing player
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BellWalk")) {
                animator.Play("BellWalk", -1, 0.0f);
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            // Rebound off player when colliding with them
    		rb.AddForce(new Vector3(-velocity.x / 2, rb.velocity.y, -velocity.z / 2), ForceMode.Impulse);
        }

        if (collision.gameObject.tag == "Umbrella Attack") {
            // Rebound further while player is blocking
            rb.AddForce(new Vector3(-velocity.x, rb.velocity.y, -velocity.z), ForceMode.Impulse);

            //target.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
            target.GetComponent<AudioSource>().PlayOneShot(target.GetComponent<PlayerController>().sfxUmbrella, 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
            // Create smoke effect
            MakeSmoke();

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Feet Attack") {
            // Create smoke effect
            MakeSmoke();

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }

        if (other.tag == "Cactus") {
            // Create smoke effect
            MakeSmoke();

            // Enemy hit by cactus, destroy
            Destroy(gameObject);
        }
    }

    /*
    private void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Player Attack") {
			// Create smoke effect
            MakeSmoke();

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
		}

        if (other.tag == "Cactus") {
            // Create smoke effect
            MakeSmoke();

            // Enemy hit by key attack, destroy
            Destroy(gameObject);
        }
    }
    */

    public void MakeSmoke() {
        // Create smoke effect
        Transform smoke = this.gameObject.transform.GetChild(0);
        smoke.gameObject.SetActive(true);
        smoke.gameObject.GetComponent<SmokeController>().enabled = true;
        smoke.parent = null;

        target.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
        target.GetComponent<AudioSource>().PlayOneShot(sfxDefeat, 1f);
    }
}
