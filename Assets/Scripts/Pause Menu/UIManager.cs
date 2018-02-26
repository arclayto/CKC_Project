using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public GameManager GM;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ScanForKeyStroke ();
	}

	void ScanForKeyStroke()
	{
		if (Input.GetButtonDown ("Pause")) {
			//GM.TogglePauseMenu ();
		} else if (Input.GetButtonDown ("Quit")) {
			if (GM.getIsPaused () == 1) {
				GM.QuitToTitle (2);
			}

		} else if (Input.GetButtonDown ("Cancel")) {
			if (GM.getIsPaused () == 1) {
			GM.QuitToTitle (1);
			}

		}
	}
}
