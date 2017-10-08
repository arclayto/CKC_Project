using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour {

    public float rotateSpeed = 100.0f;

	void Start() {
		
	}
	
	void Update() {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
	}
}
