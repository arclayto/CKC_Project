using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonusController : MonoBehaviour {

	public int maxBonus;
	public bool alreadyObtained;

	void Start () {
		// If the player has more bonus health than maxBonus, despawn the bonus to avoid exploits
		if (PlayerController.healthBonus > maxBonus) {
			alreadyObtained = true;
			GetComponent<SpriteRenderer>().color = Color.black;
		}
	}
}
