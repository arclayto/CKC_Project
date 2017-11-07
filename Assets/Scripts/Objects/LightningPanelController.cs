using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPanelController : MonoBehaviour {

	public GameObject target;
	public Texture disabledMat;

	private bool activated;
	private Renderer renderer;

	void Start () {
		activated = false;
		renderer = GetComponent<Renderer>();
	}

	private void OnTriggerEnter(Collider other) {
        if (other.tag == "Lightning") {
            if (activated == false) {
                // Activate
                activated = true;
                target.SetActive(true);

                renderer.material.mainTexture = disabledMat;
            }
        }
    }
}
