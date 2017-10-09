using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour {

    public float movementSpeed;
    public float gravityForce;
    public float aggroRange;
    public GameObject target;

    private Vector3 moveDirection;
    Rigidbody rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start() {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        // Flip enemy horizontally based on x relative to player
        if (transform.position.x < target.transform.position.x) {
            spriteRenderer.flipX = false;
        }
        else if (transform.position.x > target.transform.position.x) {
            spriteRenderer.flipX = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < aggroRange) {
            
        }

        // Idle & Walk animation based on x and z movement
        /*
        if (controller.isGrounded) {
            if (moveDirection.x == 0.0f && moveDirection.z == 0.0f) {
                // Idle animation when not moving in x or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BellIdle")) {
                    animator.Play("BellIdle", -1, 0.0f);
                }
            } else {
                // Walk animation when moving in x and/or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BellWalk")) {
                    animator.Play("BellWalk", -1, 0.0f);
                }
            }
        }
        */
    }
}
