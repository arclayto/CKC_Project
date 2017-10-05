using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpForce;
	public float gravityForce;

	private Vector3 moveDirection;
	CharacterController controller;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Use this for initialization
    void Start () {
		controller = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        // Flip player horizontally based on x direction
        if (Input.GetAxis("Horizontal") > 0.0f) {
            spriteRenderer.flipX = false;
        } else if (Input.GetAxis("Horizontal") < 0.0f) {
            spriteRenderer.flipX = true;
        }

        moveDirection = new Vector3 (Input.GetAxis ("Horizontal") * movementSpeed, moveDirection.y, Input.GetAxis ("Vertical") * movementSpeed);

        // Idle & Walk animation based on x and z movement
        if (controller.isGrounded) {
            if (moveDirection.x != 0.0f && moveDirection.z != 0.0f) {
                // Idle animation when not moving in x or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumIdle")) {
                    animator.Play("PlumIdle", -1, 0.0f);
                }
            }
            else {
                // Walk animation when moving in x and/or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumIdle")) {
                    animator.Play("PlumIdle", -1, 0.0f);
                }
            }
        }

        // Aerial animations based on y movement
        if (!controller.isGrounded) {
            if (moveDirection.y > 1.0f) {
                // Jump animation
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumJump")) {
                    animator.Play("PlumJump", -1, 0.0f);
                }
            }
            else if (moveDirection.y < -1.0f) {
                // Fall animation
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumFall")) {
                    animator.Play("PlumFall", -1, 0.0f);
                }
            }
        }

        if (Input.GetButtonDown ("Jump") && controller.isGrounded) {
			moveDirection.y = jumpForce;
		} 
		else if (controller.isGrounded) {
			moveDirection.y = -4.5f;
		}
			
		moveDirection.y = moveDirection.y + ((Physics.gravity.y * Time.deltaTime) * gravityForce);
		controller.Move (moveDirection * Time.deltaTime);
	}
}
