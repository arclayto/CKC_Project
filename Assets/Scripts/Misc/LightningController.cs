using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour {

	public GameObject target;
	public GameObject hitbox;
    public AudioClip sfxSpark;
    public AudioClip sfxStrike;

	Animator animator;
	SpriteRenderer spriteRenderer;

	void Start () {
		animator = GetComponent<Animator>();
		animator.Play("LightningWarn", -1, 0.0f);

        StartCoroutine("SparkSounds");
	}
	
	void Update () {
		transform.position = target.transform.position;

		if (animator.GetCurrentAnimatorStateInfo(0).IsName("LightningWarn")) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                // Play lightning bolt animation after warning animation finishes
                animator.Play("LightningBolt", -1, 0.0f);

                target.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
                target.GetComponent<AudioSource>().PlayOneShot(sfxStrike, 1.5f);

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

    public IEnumerator SparkSounds() {
        float delay = 0.5f;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f) {
            target.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1.1f));
            target.GetComponent<AudioSource>().PlayOneShot(sfxSpark, 0.5f);
            yield return new WaitForSeconds(delay);
            delay -= 0.05f;
            if (delay < 0.15f) {delay = 0.15f;}
        }
    }

    public IEnumerator HitboxTimer() {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitbox.SetActive(false);
    }
}
