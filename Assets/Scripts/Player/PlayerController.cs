using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpForce;
	public float gravityForce;
    public int invulnerableTime;
    public GameObject keyAttack;
	public GameObject feetAttack;
	public GameObject umbrellaAttack;
	public GameObject beanCount;
    public GameObject tutorialText;
    public AudioClip sfxJump;
    public AudioClip sfxHurt;
    public AudioClip sfxHeal;
    public AudioClip sfxBean;
    public AudioClip sfxKeyswing;
    public AudioClip sfxUmbrella;
    public AudioClip sfxTornado;
    public Vector3 returnPosition;
    public Vector3 retryPosition;
    public bool inBonus;

    private Vector3 moveDirection;
    private int health;
    public static int healthBonus = 0;
    private float restartTime = 1.0f;
    private int equippedItem;
    private int beans;
    private bool invulnerable;
    private bool isFloating;
    private GameObject bubble;
	private bool inTornado;
	private bool canMove;
    private bool canBubblewand;
	private Vector3 otherTornado;
	private float lastSwitchedItem;

    private Coroutine showTutorialText = null;
    private Coroutine hideTutorialText = null;
	private Coroutine beanFade = null;

	CharacterController controller;
    Collider coll;
	SpriteRenderer spriteRenderer;
	Animator animator;
    AudioSource audioSource;

    NightController night;

    // Use this for initialization
    void Start() {
		//hide the cursor
		Cursor.visible = false;
		canMove = true;
        health = 2 + healthBonus;
        equippedItem = 0;
        beans = 0;
        isFloating = false;
        bubble = null;
		lastSwitchedItem = Time.time;
        canBubblewand = true;

		controller = GetComponent<CharacterController>();
        coll = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
		inTornado = false;

        night = GameObject.FindWithTag("Night").GetComponent<NightController>();
    }
	
	// Update is called once per frame
	void Update() {
        // Flip player horizontally based on x direction
      	if (canMove && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
            if (Input.GetAxisRaw("Horizontal") > 0.0f) {
                spriteRenderer.flipX = false;
            } else if (Input.GetAxisRaw("Horizontal") < 0.0f) {
                spriteRenderer.flipX = true;
            }
        }

		moveDirection = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, moveDirection.y, Input.GetAxis("Vertical") * movementSpeed);

		//broken code for tornados, just leave it for now
		/*if (inTornado == true) {
			Vector3 velocity = Vector3.zero;
										moveDirection = new Vector3((Input.GetAxis("Horizontal") * movementSpeed) + Vector3.Lerp(transform.position, otherTornado, 0.5f).x,
										moveDirection.y, 
										(Input.GetAxis("Vertical") * movementSpeed) + Vector3.Lerp(transform.position, otherTornado, 0.5f).z);
		}*/

        // Idle & Walk animation based on x and z movement
        if (controller.isGrounded) {
            if (moveDirection.x == 0.0f && moveDirection.z == 0.0f) {
                // Idle animation when not moving in x or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumIdle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")
                	&& !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleSwing") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                    animator.Play("PlumIdle", -1, 0.0f);
                }
            }
            else {
                // Walk animation when moving in x and/or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumWalk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")
                	&& !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleSwing") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                    animator.Play("PlumWalk", -1, 0.0f);
                }
            }

            // Stop floating
            isFloating = false;

            // Reset bubblewand when grounded
            canBubblewand = true;
        }

        // Aerial animations based on y movement
		if (!controller.isGrounded) {
            if (moveDirection.y > 1.0f) {
                // Jump animation when moving up in y
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumJump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim")
                     && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleSwing") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                    animator.Play("PlumJump", -1, 0.0f);
                }
            }
            else if (moveDirection.y < -1.0f) {
                // Fall animation when moving down in y
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumFall") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")
                	&& !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleSwing") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                    animator.Play("PlumFall", -1, 0.0f);
                }
            }
        }
			
        if (Input.GetButtonDown("Jump") && controller.isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
            // Jump if on ground
			Jump();
		} 
		else if (controller.isGrounded && !inTornado) {
            // Cap vertical speed on ground (fixes terminal velocity fall bug)
			moveDirection.y = -1f;
		}

		if (Input.GetButtonUp ("Jump") && !controller.isGrounded) {
			if (moveDirection.y > 0f) {
				// Decrease jump height
				ShortenJump();
			}

			if (isFloating) {
				// Stop floating
				isFloating = false;
			}
		}

        if (Input.GetButtonDown("Ability") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
            // Use ability
			if (equippedItem == 1) {
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
					animator.Play ("PlumKey", -1, 0.0f);

                    audioSource.pitch = (Random.Range(0.9f, 1.1f));
                    audioSource.PlayOneShot(sfxKeyswing, 1f);
				}
			} 
			else if (equippedItem == 2) {
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && controller.isGrounded) {
					animator.Play ("PlumBlock", -1, 0.0f);
				}
			}
            else if (equippedItem == 3) {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleSwing") && canBubblewand) {
                    animator.Play("PlumBubbleAim", -1, 0.0f);
                }
            }
        }

        if (Input.GetButton("Ability")) {
            // Use ability
            if (equippedItem == 2) {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && controller.isGrounded) {
                    animator.Play ("PlumBlock", -1, 0.0f);
                }
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey")) {
            // Slow movement while using key ability
            moveDirection.x /= 2;
            moveDirection.z /= 2;

            // Activate hitbox
            keyAttack.SetActive(true);
			feetAttack.SetActive(true);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                // Key ability complete, return to idle state
                animator.Play("PlumIdle", -1, 0.0f);
            }
        }
        else {
            // Deactivate hitbox
            keyAttack.SetActive(false);
			feetAttack.SetActive(false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
            // Stop movement while using umbrella block ability
            moveDirection.x = 0;
            moveDirection.z = 0;

            // Activate hitbox
            umbrellaAttack.SetActive(true);

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                // Hold button to block longer
                if (Input.GetButton("Ability")) {
	                animator.Play("PlumBlock", -1, 0.0f);
	            }
	            else {
	            	animator.Play("PlumIdle", -1, 0.0f);
	            }
            }
        }
        else {
            // Deactivate hitbox
            umbrellaAttack.SetActive(false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleAim")) {
            // Stop movement while using bubblewand aim ability
            moveDirection.x = 0;
            moveDirection.z = 0;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                if (Input.GetButton("Ability")) {
                    // Hold button to aim longer
                    animator.Play("PlumBubbleAim", -1, 0.0f);
                }
            }

            if (Input.GetButtonUp("Ability")) {
                // Shoot bubble where aiming
                Vector3 aimDirection = new Vector3(Mathf.Round(Input.GetAxis("Horizontal")), 0f, Mathf.Round(Input.GetAxis("Vertical")));
                if (aimDirection.x == 0f && aimDirection.z == 0f) {
                    if (spriteRenderer.flipX == true) {
                        aimDirection.x = -1f;
                    } else {
                        aimDirection.x = 1f;
                    }
                }
                if (bubble != null) {
                    Destroy(bubble);
                }
                bubble = (GameObject)Instantiate(Resources.Load("Bubble"), transform.position + aimDirection * 1.75f, transform.rotation);
                bubble.GetComponent<Rigidbody>().AddForce(aimDirection * 10f, ForceMode.Impulse);
                animator.Play("PlumBubbleSwing", -1, 0.0f);
                canBubblewand = false;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBubbleSwing")) {
            // Slow movement while using bubble swing ability
            moveDirection.x /= 2;
            moveDirection.z /= 2;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f) {
                // Key ability complete, return to idle state
                animator.Play("PlumIdle", -1, 0.0f);
            }
        }

        if (Input.GetButton("Jump") && equippedItem == 2 && !isFloating && moveDirection.y < 0f) {
    		// Umbrella reduces gravity
    		isFloating = true;
    	}

		if (isFloating) {
			moveDirection.y -= (gravityForce * Time.deltaTime);
			
			if (moveDirection.y < -3f) {
				moveDirection.y = -3f;
			}

			if (!controller.isGrounded) {
				animator.Play("PlumFloat", -1, 0.0f);
			}
		}
        else {
        	// Normal gravity
        	moveDirection.y -= gravityForce * Time.deltaTime;
        }

		if (canMove) {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
    			controller.Move (moveDirection * Time.deltaTime);
            } else {
                controller.Move (new Vector3(0.0f, moveDirection.y, 0.0f) * Time.deltaTime);
            }

            animator.enabled = true;
		} else {
            animator.enabled = false;
        }

		//ESC key quits the application
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit ();
		}
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                TakeDamage();
            }

            // Ignore collision with enemy
            Physics.IgnoreCollision(collision.collider, coll);
        }

        if (collision.gameObject.tag == "Bubble") {
            if (controller.isGrounded) {
                transform.Translate(0f, .1f, 0f);
                moveDirection.y = jumpForce;
            } else {
                moveDirection.y = jumpForce;
                Destroy (collision.gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                TakeDamage();
            }
        }

        if (collision.gameObject.tag == "Bubble") {
            if (controller.isGrounded) {
                transform.Translate(0f, .1f, 0f);
                moveDirection.y = jumpForce;
            } else {
                moveDirection.y = jumpForce;
                Destroy (collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
		if (other.tag == "Key") {
			// Equip the item and destroy it when colliding with it
			if (equippedItem == 0) {
				Destroy (other.gameObject);
				equippedItem = 1;
			}
			else if (equippedItem == 2 && (Time.time - lastSwitchedItem) > 1) {
				Destroy (other.gameObject);
				Instantiate (Resources.Load("Umbrella"), other.transform.position, Quaternion.identity);
				lastSwitchedItem = Time.time;
				equippedItem = 1;
				Destroy (other.gameObject);
			}
            else if (equippedItem == 3 && (Time.time - lastSwitchedItem) > 1) {
                Destroy (other.gameObject);
                Instantiate (Resources.Load("Bubblewand"), other.transform.position, Quaternion.identity);
                lastSwitchedItem = Time.time;
                equippedItem = 1;
                Destroy (other.gameObject);
            }

            audioSource.pitch = (Random.Range(0.9f, 1.1f));
            audioSource.PlayOneShot(sfxBean, 1f);
        }

		if (other.tag == "Umbrella") {
			if (equippedItem == 0) {
				Destroy (other.gameObject);
				equippedItem = 2;
			}
			else if (equippedItem == 1 && (Time.time - lastSwitchedItem) > 1.0f) {
				Instantiate (Resources.Load("Key"), other.transform.position, Quaternion.identity);
				lastSwitchedItem = Time.time;
				equippedItem = 2;
				Destroy (other.gameObject);
			}
            else if (equippedItem == 3 && (Time.time - lastSwitchedItem) > 1) {
                Destroy (other.gameObject);
                Instantiate (Resources.Load("Bubblewand"), other.transform.position, Quaternion.identity);
                lastSwitchedItem = Time.time;
                equippedItem = 2;
                Destroy (other.gameObject);
            }

            audioSource.pitch = (Random.Range(0.9f, 1.1f));
            audioSource.PlayOneShot(sfxBean, 1f);
		}

        if (other.tag == "Bubblewand") {
            if (equippedItem == 0) {
                Destroy (other.gameObject);
                equippedItem = 3;
            }
            else if (equippedItem == 1 && (Time.time - lastSwitchedItem) > 1.0f) {
                Instantiate (Resources.Load("Key"), other.transform.position, Quaternion.identity);
                lastSwitchedItem = Time.time;
                equippedItem = 3;
                Destroy (other.gameObject);
            }
            else if (equippedItem == 2 && (Time.time - lastSwitchedItem) > 1) {
                Destroy (other.gameObject);
                Instantiate (Resources.Load("Umbrella"), other.transform.position, Quaternion.identity);
                lastSwitchedItem = Time.time;
                equippedItem = 3;
                Destroy (other.gameObject);
            }

            audioSource.pitch = (Random.Range(0.9f, 1.1f));
            audioSource.PlayOneShot(sfxBean, 1f);
        }

        if (other.tag == "Healthup") {
            // Heal and destroy item when colliding with it
			if (health < (2 + healthBonus)) {
				Destroy (other.gameObject);
				health = 2 + healthBonus;

                audioSource.pitch = (Random.Range(0.9f, 1.1f));
                audioSource.PlayOneShot(sfxHeal, 0.3f);
			}
        }

        if (other.tag == "Bean") {
            // Increment bean counter and destroy item when colliding with it
            HudBeans hudBeans = beanCount.GetComponent<HudBeans>();
			Destroy (other.gameObject);
			beans++;
			//beans = 50;
			if (beans == hudBeans.beansTotal){
				GameObject Beanstalk = GameObject.Find ("Beanstalk");
				Animator BeanstalkAnim = Beanstalk.GetComponent<Animator> ();
				GameObject Camera = GameObject.Find ("Main Camera");
				Camera.GetComponent<CameraSmoothFollow> ().setInstantFocus(Beanstalk.transform, new Vector3(0, -6, 14));
				BeanstalkAnim.SetTrigger ("Rise");
			}

			audioSource.pitch = (Random.Range(0.9f, 1.1f));
            audioSource.PlayOneShot(sfxBean, 1f);

			if (beanFade != null) {
				StopCoroutine(beanFade);
			}
			beanFade = StartCoroutine(hudBeans.VisibilityTimer());
        }

		if (other.tag == "Bean50") {

			HudBeans hudBeans = beanCount.GetComponent<HudBeans>();
			beans = hudBeans.beansTotal;
			Destroy (other.gameObject);

			GameObject Beanstalk = GameObject.Find ("Beanstalk");
			Animator BeanstalkAnim = Beanstalk.GetComponent<Animator> ();
			GameObject Camera = GameObject.Find ("Main Camera");
			Camera.GetComponent<CameraSmoothFollow> ().setInstantFocus(Beanstalk.transform, new Vector3(0, -6, 14));
			BeanstalkAnim.SetTrigger ("Rise");


			audioSource.pitch = (Random.Range(0.9f, 1.1f));
			audioSource.PlayOneShot(sfxBean, 1f);

			if (beanFade != null) {
				StopCoroutine(beanFade);
			}
			beanFade = StartCoroutine(hudBeans.VisibilityTimer());
		}

        if (other.tag == "HealthBonus") {
            // Increase maximum health by 1 and destroy item when colliding with it
            Destroy (other.gameObject);
            HealthBonusController healthBonusController = other.GetComponent<HealthBonusController>();

            if (healthBonusController.alreadyObtained == false) {
	            healthBonus++;
                HealthBonusController.obtainedBonuses[healthBonusController.stageIndex] = true;
            }
            health = 2 + healthBonus;

            audioSource.pitch = (Random.Range(0.9f, 1.1f));
            audioSource.PlayOneShot(sfxHeal, 0.3f);

            StartCoroutine("ReturnTimer");
        }

        if (other.tag == "Tutorial") {
            // Change tutorial text to collider's string
            tutorialText.GetComponent<Text>().text = other.gameObject.GetComponent<TutorialTextController>().tutorialString;
            if (hideTutorialText != null) {
                StopCoroutine(hideTutorialText);
            }
            showTutorialText = StartCoroutine(tutorialText.GetComponent<HudTutorial>().ShowTimer());
        }

        if (other.tag == "Kill Zone") {
            // Restart the scene when colliding with a kill zone
            if (inBonus) {
            	transform.position = retryPosition;

				Vector3 cameraCoordinates = retryPosition;
				CameraSmoothFollow cam = Camera.main.GetComponent<CameraSmoothFollow>();
				cameraCoordinates.x -= cam.offset.x;
				cameraCoordinates.y -= cam.offset.y;
				cameraCoordinates.z -= cam.offset.z;
				Camera.main.transform.position = cameraCoordinates;
				//Debug.Log(Camera.main.transform.position);

				GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
				smoke.transform.position = transform.position;
				smoke.transform.localScale = new Vector3(2f, 2f, 2f);

                health = 2 + healthBonus;
            } else {
	            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	        }
        }

        if (other.tag == "Cactus") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                TakeDamage();
            }
        }

        if (other.tag == "Firespot" && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
            if (invulnerable == false) {
                TakeDamage();
            }
        }

        if (other.tag == "Castella Projectile") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                TakeDamage();

                CastellaProjectileController castellaProjectile = other.gameObject.GetComponent<CastellaProjectileController>();
                castellaProjectile.castellaC.projectileVolleys = 2 * (4 - castellaProjectile.castellaC.health);

                castellaProjectile.MakeSmoke();
                Destroy(other.gameObject);

                CastellaController castella = GameObject.FindWithTag("Castella").GetComponent<CastellaController>();
                castella.StartCoroutine("AttackTimer");
                Debug.Log("Set attack timer on player hurt");
            }
        }

        if (other.tag == "Castella Pillar" && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
            if (invulnerable == false) {
                TakeDamage();
            }
        }

		if (other.tag == "Tornado") {
			if (moveDirection.y > 2f) {
				moveDirection.y = 2f;
			}

            audioSource.pitch = (Random.Range(0.9f, 1.1f));
            audioSource.PlayOneShot(sfxTornado, 1f);
		}

        if (other.tag == "Lightning") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                // Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
                TakeDamage();
            }
        }

        if (other.tag == "Bubble") {
            if (controller.isGrounded) {
                transform.Translate(0f, .1f, 0f);
                moveDirection.y = jumpForce;
            } else {
                moveDirection.y = jumpForce;
                Destroy (other.gameObject);
            }
        }

        if (other.tag == "Gem") {
        	other.gameObject.GetComponent<GemController>().bossDoor.gems++;
            Destroy (other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
    	if (other.tag == "Cactus") {
			if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
				TakeDamage();
			} 
        }

        if (other.tag == "Firespot") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                TakeDamage();
            } 
        }
	 	
		if (other.tag == "Tornado") {
			inTornado = true;

			if (controller.isGrounded) {
				moveDirection.y = 2f;
			} else {
				moveDirection.y += 2f;
			} 
		}

        if (other.tag == "Lightning") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumCry")) {
                TakeDamage();
            }
        }

        if (other.tag == "Bubble") {
            if (controller.isGrounded) {
                transform.Translate(0f, .1f, 0f);
                moveDirection.y = jumpForce;
            } else {
                moveDirection.y = jumpForce;
                Destroy (other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
		if (other.tag == "Tutorial") {
			// Fade out tutorial text
			if (showTutorialText != null) {
				StopCoroutine (showTutorialText);
			}
			hideTutorialText = StartCoroutine (tutorialText.GetComponent<HudTutorial> ().HideTimer ());
		} 

		if (other.tag == "Tornado") {
			inTornado = false;
		}
    }

    public int GetHealth() {
        return health;
    }

    public int GetEquippedItem() {
        return equippedItem;
    }

    public int GetBeans() {
        return beans;
    }

	public void SetEquippedItem(int i){
		equippedItem = i;
	}

	public void Jump() {
		moveDirection.y = jumpForce;

		if (canMove) {
	        audioSource.pitch = (Random.Range(0.9f, 1.1f));
	        audioSource.PlayOneShot(sfxJump, 2.0f);
	    }
	}

	public void Bounce() {
		moveDirection.y = jumpForce + 2.5f;

		if (canMove) {
	        audioSource.pitch = (Random.Range(0.9f, 1.1f));
	        audioSource.PlayOneShot(sfxJump, 2.0f);
	    }
	}

	public void ShortenJump() {
		moveDirection.y /= 1.5f;
	}

	public void AllowMovement(bool b)
	{
		canMove = b;
	}

    public void TakeDamage() {
        int damage = 1;
        if (night != null) {
            if (night.isNight) {
                damage *= 2;
            }
        }
        // Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
        if (health > damage) {
            health -= damage;
            StartCoroutine("InvulnerabilityTimer");
        }
        else {
            health = 0;
            animator.Play("PlumCry", -1, 0.0f);
            StartCoroutine("RestartTimer");
        }

        audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.PlayOneShot(sfxHurt, 1.5f);
    }

    IEnumerator InvulnerabilityTimer() {
        invulnerable = true;

        Color tmp = spriteRenderer.color;
        tmp.a = 0.5f;
        spriteRenderer.color = tmp;

        yield return new WaitForSeconds(invulnerableTime);

        invulnerable = false;

        tmp = spriteRenderer.color;
        tmp.a = 1.0f;
        spriteRenderer.color = tmp;
    }

    IEnumerator RestartTimer() {
        yield return new WaitForSeconds(invulnerableTime);

        if (inBonus) {
        	transform.position = retryPosition;

			Vector3 cameraCoordinates = retryPosition;
			CameraSmoothFollow cam = Camera.main.GetComponent<CameraSmoothFollow>();
			cameraCoordinates.x -= cam.offset.x;
			cameraCoordinates.y -= cam.offset.y;
			cameraCoordinates.z -= cam.offset.z;
			Camera.main.transform.position = cameraCoordinates;
			//Debug.Log(Camera.main.transform.position);

			GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
			smoke.transform.position = transform.position;
			smoke.transform.localScale = new Vector3(2f, 2f, 2f);

            health = 2 + healthBonus;
            animator.Play("PlumIdle", -1, 0.0f);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator ReturnTimer() {
        yield return new WaitForSeconds(invulnerableTime);

        transform.position = returnPosition;

		Vector3 cameraCoordinates = returnPosition;
		CameraSmoothFollow cam = Camera.main.GetComponent<CameraSmoothFollow>();
		cameraCoordinates.x -= cam.offset.x;
		cameraCoordinates.y -= cam.offset.y;
		cameraCoordinates.z -= cam.offset.z;
		Camera.main.transform.position = cameraCoordinates;
		//Debug.Log(Camera.main.transform.position);

		GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"));
		smoke.transform.position = transform.position;
		smoke.transform.localScale = new Vector3(2f, 2f, 2f);

		inBonus = false;
    }
}
