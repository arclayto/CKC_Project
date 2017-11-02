using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour {

	public float rotateSpeed = 100000.0f;

	public GameObject PullOBJ;
	public float ForceSpeed;

	void Start () {
		
	}
	
	void Update () {
		transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
	}
		
	public void OnTriggerStay (Collider coll) {

		if (coll.gameObject.tag == ("Player")){
			PullOBJ = coll.gameObject;

			PullOBJ.transform.position = Vector3.MoveTowards
				(PullOBJ.transform.position,
					transform.position,
					ForceSpeed * Time.deltaTime);
		} 
	}
}
