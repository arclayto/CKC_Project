using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpForce;
	public float gravityForce;

	private Vector3 moveDirection;
	public CharacterController controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

		moveDirection = new Vector3 (Input.GetAxis ("Horizontal") * movementSpeed, moveDirection.y, Input.GetAxis ("Vertical") * movementSpeed);

		if (Input.GetButtonDown ("Jump") && controller.isGrounded) {
			moveDirection.y = jumpForce;
		} 
		else if (controller.isGrounded) {
			moveDirection.y = 0;
		}

			
		moveDirection.y = moveDirection.y + ((Physics.gravity.y * Time.deltaTime) * gravityForce);
		controller.Move (moveDirection * Time.deltaTime);
	}
}
