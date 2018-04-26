using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Advance : MonoBehaviour {

	public Text storyText0;
	public Text storyText1;
	public Text storyText2;
	int progress;

	// Use this for initialization
	void Start () {
		progress = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			if (progress == 0) {
				storyText0.gameObject.SetActive(true);
			} else if (progress == 1) {
				storyText1.gameObject.SetActive(true);
			} else if (progress == 2) {
				storyText2.gameObject.SetActive(true);
			} else {
				GetComponent<LevelLoadTrigger>().initiateLoad("PhysicsTEST");
			}
			progress++;
        }
	}
}
