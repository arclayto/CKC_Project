using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

	void OnEnable() {
    	StartCoroutine("ActiveTimer");
    }

	public IEnumerator ActiveTimer() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
