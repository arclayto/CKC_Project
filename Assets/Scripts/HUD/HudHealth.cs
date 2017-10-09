using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHealth : MonoBehaviour {

    public PlayerController playerScript;
    public Image healthImage;

    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;

    void Start() {
        healthImage = GetComponent<Image>();
    }

    void Update() {
        if (playerScript.GetHealth() == 0) {
            // If the player has no health, show the empty heart icon
            healthImage.sprite = emptySprite;
        }
        else if (playerScript.GetHealth() == 1) {
            // If the player has 1 health, show the half heart icon
            healthImage.sprite = halfSprite;
        }
        else if (playerScript.GetHealth() == 2) {
            // If the player has 2 health, show the full heart icon
            healthImage.sprite = fullSprite;
        }
    }
}
