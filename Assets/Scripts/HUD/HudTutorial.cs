using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudTutorial : MonoBehaviour {

    public PlayerController playerScript;
    public bool shadow;
    private Text tutorialText;
    private Outline textOutline;
    private Shadow textShadow;
    private Text parentText;
    private CanvasGroup canvasGroup;

    void Start() {
    	tutorialText = GetComponent<Text>();
        textOutline = GetComponent<Outline>();
        textShadow = GetComponent<Shadow>();
        parentText = transform.parent.GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    	
    	//Color tmp = tutorialText.color;
        //tmp.a = 0f;
        //tutorialText.color = tmp;
    }

    void Update() {
        if (shadow) {

            tutorialText.text = parentText.text;

            //Color tmp = tutorialText.color;
            //tmp.a = parentText.color.a;
            //tutorialText.color = tmp;
        }
    }

    public IEnumerator ShowTimer() {
        while (canvasGroup.alpha < 1f) {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        /*
        while (tutorialText.color.a < 1f) {
	        Color tmp = tutorialText.color;
	        tmp.a += 0.1f;
	        tutorialText.color = tmp;
            tmp = textOutline.effectColor;
            tmp.a += 0.1f;
            textOutline.effectColor = tmp;
            tmp = textShadow.effectColor;
            tmp.a += 0.1f;
            textShadow.effectColor = tmp;
	        yield return new WaitForSeconds(0.01f);
	    }
        */
    }

    public IEnumerator HideTimer() {
        while (canvasGroup.alpha > 0f) {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        /*
        while (tutorialText.color.a > 0f) {
	        Color tmp = tutorialText.color;
	        tmp.a -= 0.1f;
	        tutorialText.color = tmp;
            tmp = textOutline.effectColor;
            tmp.a -= 0.1f;
            textOutline.effectColor = tmp;
            tmp = textShadow.effectColor;
            tmp.a -= 0.1f;
            textShadow.effectColor = tmp;
	        yield return new WaitForSeconds(0.01f);
	    }
        */
    }
}
