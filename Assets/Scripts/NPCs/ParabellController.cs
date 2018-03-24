using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabellController : MonoBehaviour {

    public float maxSpeed;
    public float gravityForce;
    public float swoopTime;
    public float aggroRange;
    public float maxAggroRange;
    public GameObject target;
    public AudioClip sfxDefeat;

    private Vector3 moveDirection;
    private bool aggro;
    private Vector3 velocity;
    private bool canSwoop;
    private Vector3 startPosition;
    Rigidbody rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;

    void Start() {
        aggro = false;
        canSwoop = true;

        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
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

        if (aggro && canSwoop) {
            // Move towards player when in aggro range
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            rb.AddForce(new Vector3(velocity.x * 2, -4.0f, velocity.z * 2), ForceMode.Impulse);
            canSwoop = false;
            StartCoroutine("SwoopTimer");
        }

        // Return to starting height
        if (transform.position.y < startPosition.y) {
            rb.AddForce(new Vector3(0.0f, 3.5f, 0.0f), ForceMode.Force);
        } else {
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        }

        // Idle & Fly animation aggro
        if (aggro == false) {
            // Idle animation when not aggro'd, standing around
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ParabellIdle")) {
                animator.Play("ParabellIdle", -1, 0.0f);
            }
        } else {
            // Fly animation when aggro'd, chasing player
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ParabellFly")) {
                animator.Play("ParabellFly", -1, 0.0f);
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
            rb.AddForce(new Vector3(-velocity.x * 1.3f, rb.velocity.y, -velocity.z * 1.3f), ForceMode.Impulse);
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

        if (other.tag == "Gumball") {
            if (other.transform.parent.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.0f) {
                // Create smoke effect
                MakeSmoke();

                // Enemy hit by key attack, destroy
                Destroy(gameObject);
            }
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

    IEnumerator SwoopTimer() {
        yield return new WaitForSeconds(swoopTime);

        canSwoop = true;
    }
}
