using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebugController : MonoBehaviour {

	public float jumpSpeed;
	public float jumpHeight;
	public float jumpDelay;
    public float gravityForce;
    public float aggroRange;
    public float maxAggroRange;
    public GameObject target;
    public AudioClip sfxDefeat;

    private Vector3 moveDirection;
    private bool grounded;
    private bool canJump;
    private bool aggro;
    private Vector3 velocity;
    Rigidbody rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CapsuleCollider capsuleCollider;

    void Start() {
        aggro = false;
        canJump = true;

        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.Play("FirebugIdle", -1, 0.0f);
    }
	
	void Update () {
		
	}

	void FixedUpdate() {
        // Flip enemy horizontally based on x relative to player
        if (transform.position.x < target.transform.position.x) {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > target.transform.position.x) {
            spriteRenderer.flipX = true;
        }

        RaycastHit hit;
        if (Physics.SphereCast(transform.position + capsuleCollider.center + new Vector3(0f, -0.2f, 0f), 0.3f, Vector3.down, out hit, 0.1f))
        {
            if (hit.collider.tag == "Untagged")
            {
            	if (grounded == false && canJump == false) {
            		StartCoroutine("JumpTimer");
            	}
            	grounded = true;
            	//Debug.Log("Grounded");
            }
            else
            {
            	grounded = false;
            }
        }

        // Calculate player's position
        Vector3 direction = target.transform.position - transform.position;
        float magnitude = direction.magnitude;
        direction.Normalize();

        velocity = direction * jumpSpeed;

        if (aggro == false && magnitude < aggroRange) {
            // Become aggressive when player is in range
            aggro = true;
        }

        if (aggro == true && magnitude > maxAggroRange) {
            // Become passive when player is out of chasing range
            //aggro = false;
        }

        if (aggro) {
            // Jump towards player when in aggro range
            if (grounded && canJump) {
            	Instantiate(Resources.Load("Firespot"), transform.position, transform.rotation);
	            rb.AddForce(new Vector3(velocity.x, jumpHeight, velocity.z), ForceMode.Impulse);
	            canJump = false;
	            grounded = false;
	        }
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("FirebugIdle") && grounded) {
            animator.Play("FirebugIdle", -1, 0.0f);
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("FirebugJump") && !grounded && rb.velocity.y > 0f) {
            animator.Play("FirebugJump", -1, 0.0f);
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("FirebugFall") && !grounded && rb.velocity.y < 0f) {
            animator.Play("FirebugFall", -1, 0.0f);
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

        if (other.gameObject.tag == "Bubble") {
            // Create smoke effect
            MakeSmoke();

            // Enemy hit by bubble attack, destroy
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

    public void MakeSmoke() {
        // Create smoke effect
        Transform smoke = this.gameObject.transform.GetChild(0);
        smoke.gameObject.SetActive(true);
        smoke.gameObject.GetComponent<SmokeController>().enabled = true;
        smoke.parent = null;

        target.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
        target.GetComponent<AudioSource>().PlayOneShot(sfxDefeat, 1f);
    }

    IEnumerator JumpTimer() {
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
    }
}
