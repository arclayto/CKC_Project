using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour {

	public string levelToLoad;
	public GameObject screenToTrigger;

	private bool loadNext;

	// Use this for initialization
	void Start () {
		loadNext = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (loadNext) {
			LevelLoader lv = screenToTrigger.GetComponent<LevelLoader>();
			lv.LoadLevel (levelToLoad);
		}
	}

	public void initiateLoad()
	{
		loadNext = true;
	}
}
