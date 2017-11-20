using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public string levelToLoad;

	private Animator anim;
	private bool loadNext;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if (loadNext) {
			LoadLevel (levelToLoad);
		}
	}

	public void LoadLevel(string scene)
	{
		StartCoroutine (LoadAsynchronously (scene));
	}

	IEnumerator LoadAsynchronously(string scene)
	{
		anim.SetTrigger ("LoadNext");
		yield return new WaitForSeconds (2);
		AsyncOperation operation = SceneManager.LoadSceneAsync("Scenes/" + scene, LoadSceneMode.Single);

		while (operation.isDone == false) {
			Debug.Log (operation.progress);

			yield return null;
		}
	}

	public void LoadScene(string sceneToLoad)
	{
		levelToLoad = sceneToLoad;
		loadNext = true;
	}
}
