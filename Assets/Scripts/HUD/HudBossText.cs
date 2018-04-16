using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudBossText : MonoBehaviour {

	private CanvasGroup canvasGroup;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator ShowTimer() {
        while (canvasGroup.alpha < 1f) {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator HideTimer() {
        while (canvasGroup.alpha > 0f) {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
