using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleDoorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player"){

			StartCoroutine("FadeoutTimer");
		}
	}

	IEnumerator FadeoutTimer() {
		yield return new WaitForSeconds(1f);
		GetComponent<LevelLoadTrigger>().initiateLoad();
	}
}
