using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour {

	public AudioClip sfxBurning;

	void OnEnable() {
		AudioSource audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
		audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.PlayOneShot(sfxBurning, 1f);

    	StartCoroutine("ActiveTimer");
    }

	public IEnumerator ActiveTimer() {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
