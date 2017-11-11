using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudTutorial : MonoBehaviour {

    public PlayerController playerScript;
    public bool shadow;
    private Text tutorialText;
    private Text parentText;

    void Start() {
    	tutorialText = GetComponent<Text>();
        parentText = transform.parent.GetComponent<Text>();
    	
    	Color tmp = tutorialText.color;
        tmp.a = 0f;
        tutorialText.color = tmp;
    }

    void Update() {
        if (shadow) {

            tutorialText.text = parentText.text;

            Color tmp = tutorialText.color;
            tmp.a = parentText.color.a;
            tutorialText.color = tmp;
        }
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
