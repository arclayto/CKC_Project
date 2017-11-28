using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour {

	public AudioClip sfxPop;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		AudioSource audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
		audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.PlayOneShot(sfxPop, 0.5f);
	}
}
