using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanstalkController : MonoBehaviour {

	public Vector3 teleportCoordinates;

	private GameObject player;
	private PlayerController playerController;
	private bool canTeleport;

	void Start () {
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerController>();

		canTeleport = true;
	}

	void OnTriggerEnter(Collider other) {
		if (teleportCoordinates == Vector3.zero) {
			return;
		}
		else if (other.gameObject.tag == "Player" && canTeleport) {
			// Return point when player goes to bonus stage
			playerController.returnPosition = other.gameObject.transform.position;

			// Teleport player to specified coordinates
			playerController.retryPosition = teleportCoordinates;
			other.gameObject.transform.position = teleportCoordinates;
			playerController.inBonus = true;

			Vector3 cameraCoordinates = teleportCoordinates;
			CameraSmoothFollow cam = Camera.main.GetComponent<CameraSmoothFollow>();
			cameraCoordinates.x -= cam.offset.x;
			cameraCoordinates.y -= cam.offset.y;
			cameraCoordinates.z -= cam.offset.z;
			Camera.main.transform.position = cameraCoordinates;
			//Debug.Log(Camera.main.transform.position);

			GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
			smoke.transform.position = other.gameObject.transform.position;
			smoke.transform.localScale = new Vector3(2f, 2f, 2f);

			canTeleport = false;
		}
	}
}
