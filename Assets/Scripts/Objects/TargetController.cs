using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

	public GameObject target;

	public void Activate() {
		// Create smoke effect
        MakeSmoke();

		target.SetActive(true);
		transform.parent.gameObject.SetActive(false);
	}

	public void MakeSmoke() {
        // Create smoke effect
        GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
        smoke.transform.position = transform.position;
        smoke.transform.localScale = new Vector3(5f, 5f, 5f); 
    }
}
