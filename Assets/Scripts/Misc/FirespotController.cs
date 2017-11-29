using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirespotController : MonoBehaviour {

	public float disappearTime;

	void Start () {
		StartCoroutine("DisappearTimer");
	}

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Bubble") {
            // Enemy hit by bubble attack, destroy
            Destroy(gameObject);
        }
    }

	IEnumerator DisappearTimer() {
        yield return new WaitForSeconds(disappearTime);
        GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
