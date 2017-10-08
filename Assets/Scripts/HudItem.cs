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
            itemImage.enabled = false; 
        }
        else {
            if (playerScript.GetEquippedItem() == 1) {
                itemImage.sprite = keySprite;
            }

            itemImage.enabled = true;
        }
    }
}
