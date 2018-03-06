using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStick : MonoBehaviour {


	private GameObject target = null;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		target = null;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.transform.SetParent (gameObject.transform);
		}
	}

	void OnTriggerExit(Collider col){
		col.gameObject.transform.SetParent(null);
	}
		
}
