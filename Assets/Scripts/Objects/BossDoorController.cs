using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorController : MonoBehaviour {

	public PlayerController player;
	public GameObject redGem;
	public GameObject greenGem;

	public int gems;

	//opening door by contact with player while holding key
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && gems == 2) {
			Animator animator = GetComponentInParent<Animator>();
			animator.SetTrigger("Open");
			redGem.SetActive(true);
			greenGem.SetActive(true);

			//StartCoroutine("FadeoutTimer");
		}
	}

	IEnumerator FadeoutTimer() {
        yield return new WaitForSeconds(1f);
        GetComponent<LevelLoadTrigger>().initiateLoad();
    }
}