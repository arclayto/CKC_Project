using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//to use this script, attach This to any object and when you want the next level loading to fire type:
//GetComponent<LevelLoadTrigger>().initiateLoad();



public class LevelLoadTrigger : MonoBehaviour {

	public string levelToLoad;
	public GameObject screenToTrigger;
	public GameObject musicPlayer;
	//public GameObject GM; 
	private bool loadNext;

	// Use this for initialization
	void Start () {
		loadNext = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (loadNext) {
			StartCoroutine(AudioFadeOut.FadeOut(musicPlayer.GetComponent<AudioSource>(), 10f));
			//Destroy(musicPlayer);

			LevelLoader lv = screenToTrigger.GetComponent<LevelLoader>();
			//LevelLoader lv = GetComponent<LevelLoader>();
			lv.LoadLevel (levelToLoad);
		}
	}

	public void initiateLoad()
	{
		loadNext = true;
	}

	public void initiateLoad(string lvlToLoad)
	{
		loadNext = true;
		levelToLoad = lvlToLoad;
	}
}
