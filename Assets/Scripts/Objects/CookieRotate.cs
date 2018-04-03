using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieRotate : MonoBehaviour {

    public float rotateSpeed = 100.0f;

	void Start() {
		
	}
	
	void Update() {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
	}
}
