using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour {
	
	public Rigidbody rb;
	public float force = 5f;
	public float torque = 10f;
	public int score;
	public Text txt;
	public Text highScoreTXT;
	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position
	private Vector2 startSwipe;
	private Vector2 endSwipe;
	private Score scoreClass;
	private Bounds fb;
	private Bounds lb;
	// Use this for initialization
	void Start () {
		LoadScore ();
		rb.isKinematic = false;
		txt.text = "0";
		score = -1;
		highScoreTXT.text = "High Score: " + GetHighestScore ().ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				fp = Camera.main.ScreenToViewportPoint(touch.position);
				lp = Camera.main.ScreenToViewportPoint(touch.position);
			} else if (touch.phase == TouchPhase.Moved) {
				lp = Camera.main.ScreenToViewportPoint(touch.position);;
			} else if (touch.phase == TouchPhase.Ended) {
				lp = Camera.main.ScreenToViewportPoint(touch.position);;
				Swipe();
			}
		}
		if (Input.GetMouseButtonDown (0)) {
			startSwipe = Camera.main.ScreenToViewportPoint (Input.mousePosition);
		}
		if (Input.GetMouseButtonUp (0)) {
			endSwipe = Camera.main.ScreenToViewportPoint (Input.mousePosition);
			Swipe ();
		}
			
	}

	void Swipe(){
		lb = fb;
		rb.isKinematic = false;
		Vector2 mSwipe = endSwipe - startSwipe;
		Vector2 swipe = lp - fp;
		rb.AddForce (swipe*force, ForceMode.Impulse);
		rb.AddTorque (0f, 0f, -torque, ForceMode.Impulse);
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "block") {
			rb.isKinematic = true;
			fb = col.bounds;
			if (fb == lb) {
			} else {
				score++;
				txt.text = score.ToString ();
				if (score > scoreClass.highestScore) {
					SubmitScore (score);
					LoadScore ();
					highScoreTXT.text = "High Score: " + GetHighestScore ().ToString();
				}
			}
		} else {
			SubmitScore (score);
			GameOver();

		}
			
	}

	void GameOver(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}








	private void LoadScore(){
		scoreClass = new Score ();
		if (PlayerPrefs.HasKey ("highestScore")) {
			scoreClass.highestScore = PlayerPrefs.GetInt ("highestScore");
		}
	}
	private void SaveScore(){
		PlayerPrefs.SetInt ("highestScore", scoreClass.highestScore);
	}
	private void SubmitScore(int newScore){
		
		if (newScore > scoreClass.highestScore) {
			scoreClass.highestScore = newScore;
			SaveScore();
		}
	}
	private int GetHighestScore(){
		return scoreClass.highestScore;
	}
	}