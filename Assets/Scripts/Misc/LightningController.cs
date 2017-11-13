using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour {

	public GameObject target;
	public GameObject hitbox;

	Animator animator;
	SpriteRenderer spriteRenderer;

	void Start () {
		animator = GetComponent<Animator>();
		animator.Play("LightningWarn", -1, 0.0f);
	}
	
	void Update () {
		transform.position = target.transform.position;

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("LightningWarn")) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                // Play lightning bolt animation after warning animation finishes
                animator.Play("LightningBolt", -1, 0.0f);

                StartCoroutine("HitboxTimer");
            }
        }

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("LightningBolt")) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                // Destroy once lightning bolt animation finishes
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator HitboxTimer() {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitbox.SetActive(false);
    }
}
