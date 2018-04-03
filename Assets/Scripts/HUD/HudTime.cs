using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudTime : MonoBehaviour {

	float timeUntilNight;

	Text timeText;
	NightController night;
	CanvasGroup group;

	// Use this for initialization
	void Start () {
		timeText = GetComponent<Text>();
		night = GameObject.FindWithTag("Night").GetComponent<NightController>();
		group = transform.parent.gameObject.GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilNight = night.nightThreshold - night.time;
		timeText.text = System.String.Format("{0:0}:{1:00}", Mathf.Floor(timeUntilNight/60), timeUntilNight % 60);

		if (timeUntilNight <= 0) {
			group.alpha = 0;
		}
	}
}
