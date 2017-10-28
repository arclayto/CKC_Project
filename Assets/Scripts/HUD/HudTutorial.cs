using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudTutorial : MonoBehaviour {

    public PlayerController playerScript;
    private Text tutorialText;

    void Start() {
    	tutorialText = GetComponent<Text>();
    	
    	Color tmp = tutorialText.color;
        tmp.a = 0f;
        tutorialText.color = tmp;
    }

    public IEnumerator ShowTimer() {
        while (tutorialText.color.a < 1f) {
	        Color tmp = tutorialText.color;
	        tmp.a += 0.1f;
	        tutorialText.color = tmp;
	        yield return new WaitForSeconds(0.01f);
	    }
    }

    public IEnumerator HideTimer() {
        while (tutorialText.color.a > 0f) {
	        Color tmp = tutorialText.color;
	        tmp.a -= 0.1f;
	        tutorialText.color = tmp;
	        yield return new WaitForSeconds(0.01f);
	    }
    }
}
