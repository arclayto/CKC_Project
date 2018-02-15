using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHealth : MonoBehaviour {

    public PlayerController playerScript;
    public Image healthImage;

    public Sprite[] spriteList;

    void Start() {
        healthImage = GetComponent<Image>();
    }

    void Update() {
    	healthImage.sprite = spriteList[playerScript.GetHealth()];
    	/*
        if (playerScript.GetHealth() == 0) {
            // If the player has no health, show the empty heart icon
            //healthImage.sprite = sprite0;
        }
        else if (playerScript.GetHealth() == 1) {
            // If the player has 1 health, show the half heart icon
            //healthImage.sprite = sprite1;
        }
        else if (playerScript.GetHealth() == 2) {
            // If the player has 2 health, show the full heart icon
            //healthImage.sprite = sprite2;
        }
        */
    }
}
