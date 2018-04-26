using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastellaPillarController : MonoBehaviour {

	private GameObject pillar;
	private GameObject innerPillar;
	private Renderer pillarRenderer;
	private Renderer innerPillarRenderer;
	private float delay;
	private float fadeAmount;
	public bool finalPillarInAttack;

	private Renderer renderer;

	void Start () {
		pillar = gameObject.transform.GetChild(0).gameObject;
		innerPillar = pillar.transform.GetChild(0).gameObject;
		pillarRenderer = pillar.GetComponent<Renderer>();
		innerPillarRenderer = innerPillar.GetComponent<Renderer>();
		delay = 1.0f;
		fadeAmount = 0.05f;

		renderer = GetComponent<Renderer>();
		StartCoroutine("DamageTimer");
	}
	
	IEnumerator DamageTimer() {
        yield return new WaitForSeconds(delay);

        // Enable pillar object
        pillar.SetActive(true);

        while (renderer.material.color.a > fadeAmount) {
	        renderer.material.color -= new Color(0, 0, 0, fadeAmount);
	        pillarRenderer.material.color -= new Color(0, 0, 0, fadeAmount);
	        innerPillarRenderer.material.color -= new Color(0, 0, 0, fadeAmount);
	        
	        yield return new WaitForSeconds(fadeAmount);

	        pillar.GetComponent<CapsuleCollider>().enabled = false;
        	innerPillar.GetComponent<CapsuleCollider>().enabled = false;
	    }

	    if (finalPillarInAttack == true) {
	    	// Reset Castella's state
		    CastellaController castella = GameObject.FindWithTag("Castella").GetComponent<CastellaController>();
	        castella.StartCoroutine("AttackTimer");
	        Debug.Log("Set attack timer on final pillar");
	    }

        Destroy(gameObject);
    }
}
