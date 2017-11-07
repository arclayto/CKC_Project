using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFocusTriggerController : MonoBehaviour {

	public Transform focusOn;
	public GameObject mainCamera;
	public float focusTime;

	public bool hasBeenTriggered;
	// Use this for initialization
	void Start () {
		hasBeenTriggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider Coll)
	{
		if ( Coll.gameObject.tag == "Player" && !hasBeenTriggered) {
			hasBeenTriggered = true;
			mainCamera.GetComponent<CameraSmoothFollow>().setFocus(focusOn, focusTime);
		}
	}

	/*IEnumerator focus()
	{
		Transform old = mainCamera.GetComponentInChildren<CameraSmoothFollow> ().target;
		mainCamera.GetComponent<CameraSmoothFollow> ().setFocus(focusOn);
		yield return new WaitForSeconds (2);
		mainCamera.GetComponent<CameraSmoothFollow> ().setFocus(old);
	}*/
}
