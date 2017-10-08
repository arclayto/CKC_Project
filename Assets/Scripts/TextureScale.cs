using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScale : MonoBehaviour {

	public float tileX = 1.0f;
    public float tileY = 1.0f;
	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		rend.material.mainTextureScale = new Vector2(tileX, tileY);
	}
}
