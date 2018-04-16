using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastellaController : MonoBehaviour {

	public GameObject music;
	public GameObject bossText;
	private GameObject target;
	private GameObject smoke;
	private float startY;
	public int health;
	private bool aggro;
	private float aggroRange;
	private bool cutscene;
	private int attackType;
	public int projectileVolleys;
	private int laughs;
	private AudioSource audio;

	private IEnumerator cutsceneCoroutine;

	public AudioClip sfxLaugh;

	SpriteRenderer spriteRenderer;
    Animator animator;

    void Start() {
    	target = GameObject.FindWithTag("Player");
    	smoke = transform.GetChild(0).gameObject;
    	startY = transform.position.y;
    	health = 3;
        aggro = false;
		aggroRange = 30;
		cutscene = false;
		attackType = 0;
		projectileVolleys = 2 * (4 - health);
		laughs = 5;

		audio = target.GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        animator = GetComponent<Animator>();
    }

    void Update() {
    	transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(startY, startY + 1.0f, Mathf.PingPong(Time.time, 1.0f)), transform.position.z);
    }

	void FixedUpdate() {
		// Calculate player's position
        Vector3 direction = target.transform.position - transform.position;
        float magnitude = direction.magnitude;
        direction.Normalize();

        if (aggro == false && cutscene == false && magnitude < aggroRange) {
        	StartCoroutine("AppearTimer");
        	aggro = true;
        	cutscene = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CastellaLaugh")) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            	// Laugh loop
                if (laughs == 0) {
                	// Start fight
                	animator.Play("CastellaIdle", -1, 0.0f);
                	cutscene = false;
			        music.SetActive(true);
                } else {
                	// Keep laughing
                	animator.Play("CastellaLaugh", -1, 0.0f);
                	audio.PlayOneShot(sfxLaugh, 1.0f);
                	laughs--;
                }
            }
        }
    }

    IEnumerator AppearTimer() {
        yield return new WaitForSeconds(0.5f);

        // Create smoke effect
        smoke.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(bossText.GetComponent<HudBossText>().ShowTimer());

        // Laughing animation
        animator.Play("CastellaLaugh", -1, 0.0f);
        audio.PlayOneShot(sfxLaugh, 1.0f);
        laughs--;

        yield return new WaitForSeconds(3f);
        StartCoroutine(bossText.GetComponent<HudBossText>().HideTimer());
        StartCoroutine("AttackTimer");
        Debug.Log("Set attack timer on cutscene end");
    }

    IEnumerator AttackTimer() {
        yield return new WaitForSeconds(2.5f);

        if (attackType == 0) {
	    	// First attack type: dark pillar
	        for (int i = 4 - health; i > 0; i--) {
	        	GameObject pillar = (GameObject)Instantiate(Resources.Load("Castella Pillar Light"));
	        	pillar.transform.position = new Vector3(target.transform.position.x, 5.02f, target.transform.position.z);

	        	if (i == 1) {
	        		pillar.GetComponent<CastellaPillarController>().finalPillarInAttack = true;
        		} else {
        			yield return new WaitForSeconds(1.0f);
        		}
	        }

	        attackType = 1;
	    } else if (attackType == 1) {
        	// Second attack type: dead man's volley
	        GameObject projectile = (GameObject)Instantiate(Resources.Load("Castella Projectile"));
	        projectile.transform.position = transform.position;

	        attackType = 0;
	    }
    }
}
