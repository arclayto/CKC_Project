using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonusController : MonoBehaviour {

	public int stageIndex;
	public static bool[] obtainedBonuses = new bool[3];
	public bool alreadyObtained;

	void Start () {
		// If the player has more bonus health than maxBonus, despawn the bonus to avoid exploits
		if (obtainedBonuses[stageIndex] == true) {
			alreadyObtained = true;
			GetComponent<SpriteRenderer>().color = Color.black;
		}
	}
}
