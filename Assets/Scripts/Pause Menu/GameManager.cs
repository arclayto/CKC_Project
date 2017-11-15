using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public UIManager UI;
	public Canvas PauseCanvas;
	public Canvas HUDCanvas;

	private int isPaused;

	// Use this for initialization
	void Start () {
		PauseCanvas.enabled = false;
		HUDCanvas.enabled = true;
		Time.timeScale = 1.0f;
		isPaused = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TogglePauseMenu()
	{
		
		if (isPaused == 0) 
		{
			PauseCanvas.enabled = true;
			HUDCanvas.enabled = false;
			Time.timeScale = 0.0f;
			isPaused = 1;
		}
		else if (isPaused == 1) 
		{
			PauseCanvas.enabled = false;
			HUDCanvas.enabled = true;
			Time.timeScale = 1.0f;
			isPaused = 0;
		}
	}

	public void QuitToTitle(int code)
	{
		if (code == 1) {
			GetComponent<LevelLoadTrigger>().initiateLoad("TitleScreen");
		} else if (code == 2) {
			
			string scene = "TitleScreen";
			AsyncOperation operation = SceneManager.LoadSceneAsync("Scenes/" + scene, LoadSceneMode.Single);
			/*
			while (operation.isDone == false) {
				Debug.Log (operation.progress);
				yield return null;
			}
			*/
		}

	}

	public int getIsPaused()
	{
		return isPaused;
	}
}
