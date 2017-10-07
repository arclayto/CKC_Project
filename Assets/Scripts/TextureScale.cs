using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScale : MonoBehaviour {

	public float scale = 1;
	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		rend.material.mainTextureScale = new Vector2(transform.localScale.x * scale, transform.localScale.z * scale);
	}
}
