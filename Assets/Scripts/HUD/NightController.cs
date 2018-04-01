using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightController : MonoBehaviour {

	public float nightAlpha;
	public float fadeTime;
	public bool isNight;
	public float nightThreshold;
	public float time;
	
	private Color nightColor;
	private Color dayColor;
	CanvasGroup canvasGroup;
	IEnumerator coroutine;

	void Start() {
		canvasGroup = GetComponent<CanvasGroup>();
		isNight = false;
		time = 0;

		if (nightThreshold == 0) {
			nightThreshold = 90f;
		}

		nightColor = new Color(0.4338f, 0.2746254f, 0.2746254f, 1);
		InvokeRepeating ("Timer", 1.0f, 1.0f);
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

	void Timer(){
		time++;
		if (time == nightThreshold) {
			isNight = true;
			coroutine = FadeNight ();
			StartCoroutine (coroutine);
		}
	}
		
	IEnumerator FadeNight() {
		/*while (canvasGroup.alpha < nightAlpha) {
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, nightAlpha, fadeTime);
			yield return new WaitForEndOfFrame();
		}*/

		Color spriteHue = new Vector4 (220f, 220f, 220f, 1);
		GameObject player = GameObject.Find ("Player");
		Skybox cSkybox = GameObject.Find ("Main Camera").GetComponent<Skybox>();

		Light pLight = player.GetComponent<Light> ();
		Material spriteShade = player.GetComponent<Renderer>().material;
		spriteShade.shader = Shader.Find ("Sprites/Default");
			
		while (RenderSettings.ambientLight != nightColor) {
			if (pLight.intensity > 0.999f) {
				pLight.intensity = 1f;
				RenderSettings.ambientLight = nightColor;
				yield break;
			}
				
			pLight.intensity = Mathf.Lerp (pLight.intensity, 1f, fadeTime);
			cSkybox.material.SetFloat ("_Blend", pLight.intensity);
			spriteShade.SetColor("_TintColor", Vector4.Lerp (RenderSettings.ambientLight, spriteHue , fadeTime));
			RenderSettings.ambientLight = Vector4.Lerp (RenderSettings.ambientLight, nightColor, fadeTime);
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator FadeDay() {
		/*while (canvasGroup.alpha > 0.0f) {
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.0f, fadeTime);
			yield return new WaitForEndOfFrame();
		}*/
		GameObject player = GameObject.Find ("Player");
		Skybox cSkybox = GameObject.Find ("Main Camera").GetComponent<Skybox>();

		Light pLight = player.GetComponent<Light> ();
		Material spriteShade = player.GetComponent<Renderer>().material;


		while (pLight.intensity != 0f) {
			if (pLight.intensity < 0.001f) {
				pLight.intensity = 0f;
				RenderSettings.ambientLight = Color.white;
				yield break;
			}
			else if (pLight.intensity < 0.3f) {
				spriteShade.shader = Shader.Find ("Sprites/Diffuse");
			}
			pLight.intensity = Mathf.Lerp (pLight.intensity, 0f, fadeTime);
			cSkybox.material.SetFloat ("_Blend", pLight.intensity);
			RenderSettings.ambientLight = Vector4.Lerp (RenderSettings.ambientLight, Color.white, fadeTime);
			yield return new WaitForEndOfFrame ();
		}
	}
}
