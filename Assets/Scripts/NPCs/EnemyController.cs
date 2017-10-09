using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public GameObject player;


	private Vector3 distToPlayer;
	private Vector3 proximityRangeIdle;
	private Vector3 proximityRangeAngery;
	private Vector3 proximityRangeContact;
	private float speed;
	private float contactTime;


	//debug text vars
	public Text DistText;
	private string mDistString;
	enum mState {Idle, Angery, Stunned, Dying};
	private mState EnemyState;

	// Use this for initialization
	void Start () {

		EnemyState = mState.Idle;
		contactTime = Time.time;
		proximityRangeIdle = new Vector3 (6, 4, 6);
		proximityRangeAngery = new Vector3 (10,4,10);
		proximityRangeContact = new Vector3 (1,2,1);
		speed = .5f;

		distToPlayer = transform.position - player.transform.position;
		SetDistText ();

	}

	// Update is called once per frame
	void Update () {


		switch (EnemyState) {
		case mState.Angery:
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, speed); //move towards player
			if (!isNearPlayer (proximityRangeAngery)) {//check if player is within angery range
				EnemyState = mState.Idle;
			}
			if(isNearPlayer(proximityRangeContact)){
				

					contactTime = Time.time;
					EnemyState = mState.Stunned;	


			}
			break;
		case mState.Idle:
			if (isNearPlayer (proximityRangeIdle)) {//check if player is within idle range
				EnemyState = mState.Angery;
			}

			break;
		case mState.Stunned:
			
			if (getElapsedTime(contactTime) > 1.5) {
				
					EnemyState = mState.Angery;
				}

			//freeze for 3 seconds,  then go back to being angery

			break;
		case mState.Dying:
			//destroy this object with a dying animation
			gameObject.SetActive(false);
			break;
		}

	}

	float getElapsedTime(float StartTime)
	{
		return (Time.time - StartTime);
	}

	bool isNearPlayer(Vector3 proximityRange)
	{
		bool isNear = false;
		distToPlayer = transform.position - player.transform.position;
		if (distToPlayer.x < proximityRange.x && distToPlayer.y < proximityRange.y && distToPlayer.z < proximityRange.z) 
		{
			isNear = true;
		}

		return isNear;
	}

	void SetDistText() //for debugging
	{
		distToPlayer = transform.position - player.transform.position;

		mDistString  = "Distance X:" + distToPlayer.x.ToString (); 
		mDistString += "\nDistance Y:" + distToPlayer.y.ToString (); 
		mDistString += "\nDistance Z:" + distToPlayer.z.ToString (); 

		//DistText.text = mDistString;
		DistText.text = Time.time.ToString ();


	}
}
