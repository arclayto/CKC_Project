using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastellaPillarController : MonoBehaviour {

	private GameObject pillar;
	private GameObject innerPillar;
	private Renderer pillarRenderer;
	private Renderer innerPillarRenderer;
	private float delay;

	private Renderer renderer;

	void Start () {
		pillar = gameObject.transform.GetChild(0).gameObject;
		innerPillar = pillar.transform.GetChild(0).gameObject;
		pillarRenderer = pillar.GetComponent<Renderer>();
		innerPillarRenderer = innerPillar.GetComponent<Renderer>();
		delay = 1.0f;
		renderer = GetComponent<Renderer>();
		StartCoroutine("DamageTimer");
	}
	
	IEnumerator DamageTimer() {
        yield return new WaitForSeconds(delay);

        // Enable pillar object
        pillar.SetActive(true);

        while (renderer.material.color.a > 0.05f) {
	        renderer.material.color -= new Color(0, 0, 0, 0.05f);
	        pillarRenderer.material.color -= new Color(0, 0, 0, 0.05f);
	        innerPillarRenderer.material.color -= new Color(0, 0, 0, 0.05f);
	        yield return new WaitForSeconds(0.05f);
	    }

	    CastellaController castella = GameObject.FindWithTag("Castella").GetComponent<CastellaController>();
        castella.StartCoroutine("AttackTimer");

        Destroy(gameObject);
    }
}
