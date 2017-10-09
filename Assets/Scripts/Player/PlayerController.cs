using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpForce;
	public float gravityForce;
    public int invulnerableTime;
    public GameObject keyAttack;

    private Vector3 moveDirection;
    private int health;
    private int equippedItem;
    private bool invulnerable;
	CharacterController controller;
    Collider collider;
	SpriteRenderer spriteRenderer;
    Animator animator;

    // Use this for initialization
    void Start() {
        health = 2;
        equippedItem = 0;

		controller = GetComponent<CharacterController>();
        collider = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update() {
        // Flip player horizontally based on x direction
      	if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
            if (Input.GetAxis("Horizontal") > 0.0f) {
                spriteRenderer.flipX = false;
            } else if (Input.GetAxis("Horizontal") < 0.0f) {
                spriteRenderer.flipX = true;
            }
        }

        moveDirection = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, moveDirection.y, Input.GetAxis("Vertical") * movementSpeed);

        // Idle & Walk animation based on x and z movement
        if (controller.isGrounded) {
            if (moveDirection.x == 0.0f && moveDirection.z == 0.0f) {
                // Idle animation when not moving in x or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumIdle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
                    animator.Play("PlumIdle", -1, 0.0f);
                }
            }
            else {
                // Walk animation when moving in x and/or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumWalk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
                    animator.Play("PlumWalk", -1, 0.0f);
                }
            }
        }

        // Aerial animations based on y movement
		if (!controller.isGrounded) {
            if (moveDirection.y > 1.0f) {
                // Jump animation when moving up in y
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumJump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
                    animator.Play("PlumJump", -1, 0.0f);
                }
            }
            else if (moveDirection.y < -1.0f) {
                // Fall animation when moving down in y
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumFall") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
                    animator.Play("PlumFall", -1, 0.0f);
                }
            }
        }

        if (Input.GetButtonDown ("Jump") && controller.isGrounded) {
            // Jump if on ground
			moveDirection.y = jumpForce;
		} 
		else if (controller.isGrounded) {
            // Cap vertical speed on ground (fixes terminal velocity fall bug)
			moveDirection.y = -4.5f;
		}

        if (Input.GetButtonDown("Ability")) {
            // Use ability
            if (equippedItem == 1) {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
                    animator.Play("PlumKey", -1, 0.0f);
                }
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
            // Slow movement while using key ability
            moveDirection.x /= 2;
            moveDirection.z /= 2;

            // Activate hitbox
            keyAttack.SetActive(true);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                // Key ability complete, return to idle state
                animator.Play("PlumIdle", -1, 0.0f);
            }
        }
        else {
            // Deactivate hitbox
            keyAttack.SetActive(false);
        }

        moveDirection.y = moveDirection.y + ((Physics.gravity.y * Time.deltaTime) * gravityForce);
		controller.Move(moveDirection * Time.deltaTime);
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (invulnerable == false) {
                // Deal damage to player if not invulnerable, then make the player invulnerable for a short time
                if (health > 0) {
                    health--;
                }

                StartCoroutine("InvulnerabilityTimer");
            }

            // Ignore collision with enemy
            Physics.IgnoreCollision(collision.collider, collider);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (invulnerable == false) {
            // Deal damage to player if not invulnerable, then make the player invulnerable for a short time
            if (health > 0) {
                health--;
            }

            StartCoroutine("InvulnerabilityTimer");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Item") {
            // Equip the item and destroy it when colliding with it
            Destroy(other.gameObject);
            equippedItem = 1;
        }

        if (other.tag == "Kill Zone") {
            // Restart the scene when colliding with a kill zone
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public int GetHealth() {
        return health;
    }

    public int GetEquippedItem() {
        return equippedItem;
    }

	public void SetEquippedItem(int i){
		equippedItem = i;
	}

    IEnumerator InvulnerabilityTimer() {
        invulnerable = true;

        Color tmp = spriteRenderer.color;
        tmp.a = 0.5f;
        spriteRenderer.color = tmp;

        yield return new WaitForSeconds(invulnerableTime);

        invulnerable = false;

        tmp = spriteRenderer.color;
        tmp.a = 1.0f;
        spriteRenderer.color = tmp;
    }
}
