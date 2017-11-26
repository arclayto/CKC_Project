using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudItem : MonoBehaviour {

    public PlayerController playerScript;
    public Image itemImage;

    public Sprite keySprite;
    public Sprite umbrellaSprite;
    public Sprite bubblewandSprite;

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
            else if (playerScript.GetEquippedItem() == 2) {
                // If the player has an umbrella, show the umbrella item icon
                itemImage.sprite = umbrellaSprite;
            }
            else if (playerScript.GetEquippedItem() == 3) {
                // If the player has a bubblewand, show the bubblewand item icon
                itemImage.sprite = bubblewandSprite;
            }

            // If the player has any item, show the item icon
            itemImage.enabled = true;
        }
    }
}
