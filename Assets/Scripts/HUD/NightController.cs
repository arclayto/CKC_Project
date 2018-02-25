using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightController : MonoBehaviour {

	public float nightAlpha;
	public float fadeTime;
	public bool isNight;

	CanvasGroup canvasGroup;
	IEnumerator coroutine;

	void Start() {
		canvasGroup = GetComponent<CanvasGroup>();
		isNight = false;
	}

	void Update() {
		// Debug force screen transitions
		if (Input.GetKeyDown(KeyCode.Home)) {
			if (coroutine != null) {
				StopCoroutine(coroutine);
			}
			coroutine = FadeNight();
			StartCoroutine(coroutine);

			isNight = true;
		}

		if (Input.GetKeyDown(KeyCode.End)) {
			if (coroutine != null) {
				StopCoroutine(coroutine);
			}
			coroutine = FadeDay();
			StartCoroutine(coroutine);

			isNight = false;
		}
	}

	IEnumerator FadeNight() {
		while (canvasGroup.alpha < nightAlpha) {
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, nightAlpha, fadeTime);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator FadeDay() {
		while (canvasGroup.alpha > 0.0f) {
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.0f, fadeTime);
			yield return new WaitForEndOfFrame();
		}
	}
}
