using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudItem : MonoBehaviour {

    public PlayerController playerScript;
    public Image itemImage;

    public Sprite keySprite;

	void Start() {
        itemImage = GetComponent<Image>();
	}
	
	void Update() {
		if (playerScript.GetEquippedItem() == 0) {
            // If the player has no item, hide the item icon
            itemImage.enabled = false; 
        }
        else {
            if (playerScript.GetEquippedItem() == 1) {
                // If the player has a key, show the key item icon
                itemImage.sprite = keySprite;
            }

            // If the player has any item, show the item icon
            itemImage.enabled = true;
        }
    }
}
