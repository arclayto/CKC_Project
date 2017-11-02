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

    private Vector3 moveDirection;
    private int health;
    private int equippedItem;
    private int beans;
    private bool invulnerable;
	private bool inTornado;
	private bool canMove;
	private Vector3 otherTornado;
	private float lastSwitchedItem;

    private Coroutine showTutorialText = null;
    private Coroutine hideTutorialText = null;
	private Coroutine beanFade = null;

	CharacterController controller;
    Collider coll;
	SpriteRenderer spriteRenderer;
	Animator animator;

    // Use this for initialization
    void Start() {
		//hide the cursor
		Cursor.visible = false;
		canMove = true;

        health = 2;
        equippedItem = 0;
        beans = 0;
		lastSwitchedItem = Time.time;

		controller = GetComponent<CharacterController>();
        coll = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
		inTornado = false;
    }
	
	// Update is called once per frame
	void Update() {
        // Flip player horizontally based on x direction
      	if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
            if (Input.GetAxis("Horizontal") > 0.0f) {
                spriteRenderer.flipX = false;
            } else if (Input.GetAxis("Horizontal") < 0.0f) {
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
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumIdle") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                    animator.Play("PlumIdle", -1, 0.0f);
                }
            }
            else {
                // Walk animation when moving in x and/or z
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumWalk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                    animator.Play("PlumWalk", -1, 0.0f);
                }
            }
        }

        // Aerial animations based on y movement
		if (!controller.isGrounded) {
            if (moveDirection.y > 1.0f) {
                // Jump animation when moving up in y
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumJump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                    animator.Play("PlumJump", -1, 0.0f);
                }
            }
            else if (moveDirection.y < -1.0f) {
                // Fall animation when moving down in y
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlumFall") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                    animator.Play("PlumFall", -1, 0.0f);
                }
            }
        }
			
        if (Input.GetButtonDown ("Jump") && controller.isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {

            // Jump if on ground
			Jump();
			//jumpCheck = true;
		} 
		else if (controller.isGrounded) {
            // Cap vertical speed on ground (fixes terminal velocity fall bug)
			moveDirection.y = -1f;
		}

		if (Input.GetButtonUp ("Jump") && !controller.isGrounded && moveDirection.y > 0f) {
            // Decrease jump height
			ShortenJump();
			//jumpCheck = true;
		} 

        if (Input.GetButtonDown("Ability")) {
            // Use ability
			if (equippedItem == 1) {
				if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("PlumKey")) {
					animator.Play ("PlumKey", -1, 0.0f);
				}
			} 
			else if (equippedItem == 2) {
				if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("PlumBlock") && controller.isGrounded) {
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
                // Key ability complete, return to idle state
                animator.Play("PlumIdle", -1, 0.0f);
            }
        }
        else {
            // Deactivate hitbox
            umbrellaAttack.SetActive(false);
        }

		//gravity contingent upon use of umbrella
		if (Input.GetButton("Jump") && equippedItem == 2) {
			RaycastHit hit;
			float reach = 7.0f;
			float reach2 = 1.2f * reach;
			Vector3 down = transform.TransformDirection (Vector3.down);
			Debug.DrawRay (transform.position, down * reach, Color.red);
			
			if (Physics.Raycast (transform.position, down, out hit, reach) && hit.transform.tag == "Fan") {
				moveDirection.y += 1.5f * (gravityForce * Time.deltaTime);

				if (!controller.isGrounded) {
					animator.Play("PlumFloat", -1, 0.0f);
				}
			}
			else if (Physics.Raycast (transform.position, down, out hit, reach2) && hit.transform.tag == "Fan") {
				moveDirection.y -= 1.5f * (gravityForce * Time.deltaTime);

				if (!controller.isGrounded) {
					animator.Play("PlumFloat", -1, 0.0f);
				}
			}
			else if (moveDirection.y < 0) {
				//moveDirection.y -= (gravityForce / 3 * Time.deltaTime);
				moveDirection.y = -gravityForce / 15;

				if (!controller.isGrounded) {
					animator.Play("PlumFloat", -1, 0.0f);
				}
			}
			else {
				moveDirection.y -= gravityForce * Time.deltaTime;
			}
		} 
		else {
			moveDirection.y -= gravityForce * Time.deltaTime;
		}

		if (canMove) {
			controller.Move (moveDirection * Time.deltaTime);
		}
		//ESC key quits the application
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.Quit ();
		}
	}

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                // Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
                if (health > 1) {
                    health--;
                }
                else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                StartCoroutine("InvulnerabilityTimer");
            }

            // Ignore collision with enemy
            Physics.IgnoreCollision(collision.collider, coll);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumKey") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
            // Deal damage to player if not invulnerable, then make the player invulnerable for a short time
            if (health > 1) {
                health--;
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            StartCoroutine("InvulnerabilityTimer");
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
		}

        if (other.tag == "Healthup") {
            // Heal and destroy item when colliding with it
			if (health < 2) {
				Destroy (other.gameObject);
				health = 2;
			}
        }

        if (other.tag == "Bean") {
            // Increment bean counter and destroy item when colliding with it
			Destroy (other.gameObject);
			beans++;
			if (beanFade != null) {
				StopCoroutine(beanFade);
			}
			beanFade = StartCoroutine(beanCount.GetComponent<HudBeans>().VisibilityTimer());
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (other.tag == "Cactus") {
            if (invulnerable == false) {
                // Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
                if (health > 1) {
                    health--;
                }
                else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                StartCoroutine("InvulnerabilityTimer");
            }
        }

		/*if (other.tag == "Tornado") {
			inTornado = true;
			otherTornado.x = other.GetComponentInParent<Transform>().position.x;
			otherTornado.y = other.GetComponentInParent<Transform>().position.y;
			otherTornado.z = other.GetComponentInParent<Transform>().position.z;
		}*/

        if (other.tag == "Lightning") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                // Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
                if (health > 1) {
                    health--;
                }
                else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                StartCoroutine("InvulnerabilityTimer");
            }
        }
    }

    private void OnTriggerStay(Collider other) {
    	if (other.tag == "Cactus") {
			if (invulnerable == false) {
				// Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
				if (health > 1) {
					health--;
				} else {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
				}

				StartCoroutine ("InvulnerabilityTimer");
			} 
        }
	 	
		if (other.tag == "Tornado") {
			inTornado = true;
			//otherTornado.y = other.transform.position.y;
			//otherTornado.x = other.transform.position.x;
			//otherTornado.z = other.transform.position.z;
			moveDirection.y += 1;
			//moveDirection.z += other.transform.position.z * 10;
			//moveDirection.x += other.transform.position.x * 10;
		}

        if (other.tag == "Lightning") {
            if (invulnerable == false && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlumBlock")) {
                // Deal damage to player if not invulnerable and not spinning, then make the player invulnerable for a short time
                if (health > 1) {
                    health--;
                }
                else {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                StartCoroutine("InvulnerabilityTimer");
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

		if (other.tag == "Torando") {
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
	}

	public void ShortenJump() {
		moveDirection.y /= 1.5f;
	}

	public void AllowMovement(bool b)
	{
		canMove = b;
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
}
