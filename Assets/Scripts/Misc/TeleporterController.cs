using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour {

	public Vector3 teleportCoordinates;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(Camera.main.transform.position);
	}

	void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // Teleport player to specified coordinates
            other.gameObject.transform.position = teleportCoordinates;

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
        }
    }
}
