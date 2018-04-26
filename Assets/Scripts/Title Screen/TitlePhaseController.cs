using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitlePhaseController : MonoBehaviour {

	//public float speed;
	public Text countText; 
	public Text winText;
	public Text mangleText;
	public Transform target;
	public float radius;
	public int yVal;

	//private Rigidbody rb;
	//private int count;

	private float angle;
	private float cspeed;

	private int mSeconds;
	private int numPhases;
	private int currentPhase;
	private float mAngle;
	private float horizontalInput;
	private bool leftPress;
	private bool rightPress;
	private int mDebug;
	private bool canPress;
	private int moveStatus;
	private int moveOn;
	private string sOption;
	void Start ()
	{
		mDebug = 1;
		moveStatus = 0;
		moveOn = 0;
		sOption = "Start Game";
		canPress = true;
		//rb = GetComponent<Rigidbody>();
		horizontalInput = 0;
		currentPhase = 0;
		SetCountText (currentPhase);
		//winText.text = "";

		Cursor.visible = false;

		angle = 0;
		// radius = 3;
		mSeconds = 5; //num of seconds to complete a circle;
		cspeed = (2 * Mathf.PI) / mSeconds;
		mAngle = 2 * Mathf.PI;
		transform.LookAt(target);
	}


	void Update()
	{
		horizontalInput = Input.GetAxis ("Horizontal");
		leftPress = ( horizontalInput < 0);
		rightPress = (horizontalInput  > 0);

		//bool isAnimating = false;

		RenderSettings.skybox.SetFloat ("_Rotation", Time.time * 2f);

		if (leftPress == true && moveStatus == 0 && canPress == true) {
			currentPhase -= 1;
			moveStatus = -1;
			if (currentPhase < 0) {
				currentPhase = 5;
			}
			canPress = false;
		} else if (rightPress == true && moveStatus == 0 &&canPress == true) {
			currentPhase += 1;
			moveStatus = 1;
			if (currentPhase > 5) {
				currentPhase = 0;
			}
			canPress = false;
		} else if (Input.GetButtonDown ("Jump") && moveOn == 0) {

			moveOn = 1;
			canPress = false;
		}
		//if (moveOn == 1) {GetComponent<LevelLoadTrigger>().initiateLoad();}
		switch (currentPhase) 
		{
		case 0:	mAngle = 0 * Mathf.PI; mangleText.text = mAngle.ToString () + "\n0" ;
			if (moveOn == 1) {GetComponent<LevelLoadTrigger>().initiateLoad("StoryScreen"); moveOn = 2;}
				break;
		case 1:	mAngle =  Mathf.PI / 3; mangleText.text = mAngle.ToString ()  + "\n1/3";
			if (moveOn == 1) {GetComponent<LevelLoadTrigger>().initiateLoad("Desert"); moveOn = 2;}
				break;
		case 2: mAngle =  2 * (Mathf.PI / 3); mangleText.text = mAngle.ToString () + "\n2/3";
			if (moveOn == 1) {GetComponent<LevelLoadTrigger>().initiateLoad("Temple"); moveOn = 2;}
				break;
		case 3: mAngle = (1) * Mathf.PI; mangleText.text = mAngle.ToString () + "\n1";
			if (moveOn == 1) {GetComponent<LevelLoadTrigger>().initiateLoad("Candyland"); moveOn = 2;}
				break;
		case 4: mAngle = (4 * Mathf.PI) / 3; mangleText.text = mAngle.ToString () + "\n4/3";
			if (moveOn == 1) {GetComponent<LevelLoadTrigger>().initiateLoad("Candyboss"); moveOn = 2;}
				break;
		case 5: mAngle = (5) * Mathf.PI / 3; mangleText.text = mAngle.ToString () + "\n5/3";
			if (moveOn == 1) {Application.Quit();}
				break;
		default: mAngle = 0 * Mathf.PI; mangleText.text = mAngle.ToString () + "\n0";
				break;
		}

		if (horizontalInput == 0) 
		{
			canPress = true;
		}

		//float vAngle = mAngle / mSeconds;
		float cx, cz, ccx, ccz;
		cx = Mathf.Cos (angle) * radius;
		cz = Mathf.Sin (angle) * radius;
		ccx = Mathf.Cos (mAngle) * radius;
		ccz = Mathf.Sin (mAngle) * radius;

		if (moveStatus == 1) 
		{
			angle += cspeed * Time.deltaTime;
			angle %= (2 * Mathf.PI);
			if(isInRange(angle, mAngle))
			{
				moveStatus = 0;
			}
		}
		if (moveStatus == -1) 
		{
			angle -= cspeed * Time.deltaTime;

			if (angle == 0) {
				//angle = (2 * Mathf.PI);
				/*
				angle *= -1;
				angle %= (2 * Mathf.PI);
				*/
			} 
			else if (angle < 0) 
			{
				angle = (2 * Mathf.PI) + angle;
				if (currentPhase == 0) {
					moveStatus = 0;
					angle = 0;
				}

			}
			else 
			{
				angle %= (2 * Mathf.PI);
			}
				


			if(isInRange(angle, mAngle))
			{
				moveStatus = 0;
			}
		}

		if(moveStatus != 0)
		{
			
			if (cx != ccx || cz != ccz) {
				transform.position = new Vector3 (cx, yVal, cz);
				// Rotate the camera every frame so it keeps looking at the target

				transform.LookAt(target);
			}




		}

		if (mDebug == 1) 
		{
			countText.text = "(" + cx.ToString () + "," + cz.ToString () + ")";
			countText.text += "\n(" + ccx.ToString () + "," + ccz.ToString () + ")";
			countText.text += "\nangle: " + angle.ToString ();
			countText.text += "\nmAngle: " + mAngle.ToString ();
			countText.text += "\nPhase: " + currentPhase.ToString ();
			countText.text += "\nHorizontalInput: " + horizontalInput.ToString ();
			countText.text += "\nLeftInput: " + leftPress.ToString ();
			countText.text += "\nRightInput: " + rightPress.ToString ();
		}
		SetCountText (currentPhase);

	}

	void setDebugText(float cx, float cz, float ccx, float ccz)
	{
		
	}

	bool isInRange(float nIn, float nMid, float nRange = 0.1f)
	{
		bool inRange = false;
		float upperBound = (nMid + nRange) % (2*Mathf.PI);
		float lowerBound = (nMid - nRange) % (2*Mathf.PI);
		if (nIn <= upperBound && nIn >= lowerBound) 
		{
			winText.text += lowerBound.ToString ();
			inRange = true;
		}
		return inRange;
	}


	void SetCountText(int status)
	{
		sOption = "Start Game";
		switch (status) 
		{
			case 0:	sOption = "New Game";
					break;
			case 1:	sOption = "Desert";
					break;
			case 2: sOption = "Temple";
					break;
			case 3: sOption = "Candyland";
					break;
			case 4: sOption = "Castella Fight";
					break;
			case 5: sOption = "Quit";
					break;
			default: sOption = "Start Game";
				break;

		}
		winText.text = sOption;

	}
}