﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellController : MonoBehaviour {

    public float maxSpeed;
    public float acceleration;
    public float gravityForce;
    public float aggroRange;
    public GameObject target;

    private Vector3 moveDirection;
    private bool aggro;
    Rigidbody rb;
    Collider collider;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start() {
        aggro = false;

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
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

        Vector3 velocity = direction * maxSpeed;

        if (magnitude < aggroRange) {
            // Move towards player when in aggro range
            aggro = true;
            rb.AddForce(new Vector3(velocity.x, rb.velocity.y, velocity.z), ForceMode.Acceleration);
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
            // Ignore collision with player
            //Physics.IgnoreCollision(collision.collider, collider);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player Attack") {
            Destroy(gameObject);
        }
    }
}