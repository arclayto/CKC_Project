using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumballSwitchController : MonoBehaviour {

	public GameObject target;
	public bool isActivated;
	public Texture pressedTexture;

	Renderer rend;

	void Awake() {
		rend = gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>();
	}

	public void ActivateTarget() {
		target.SetActive(true);
		rend.material.mainTexture = pressedTexture;
	}
}
