using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

	public GameObject target;

	public void Activate() {
		target.SetActive(true);
		transform.parent.gameObject.SetActive(false);
	}
}
