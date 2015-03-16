using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{
	// public member variables
	public TextMesh ScoreText;
	public TextMesh BestScoreText;
	public GameObject ScoreScreen;
	public GameObject GetReadyPrompt;
	public LevelSettings LevelManager;

	// private member variables
	private PlayerController _Player;
	private int _BestScore;
	private bool _GetReady = true;

	// Use this for initialization
	void Start () 
	{
		ScoreText.text = "0";
		_Player = (PlayerController)GameObject.FindObjectOfType(typeof(PlayerController));

		// retrieve best score from player preferences
		if( PlayerPrefs.HasKey("BestScore") )
		{
			_BestScore = PlayerPrefs.GetInt("BestScore");
		}
		else
		{
			_BestScore = 0;
		}

		// start the game paused
		LevelManager.Paused = true;
		_GetReady = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if we're still in get ready mode
		if( _GetReady ) 
		{
			if( Input.GetMouseButtonDown(0) )
			{
				GetReadyPrompt.animation.Play ("GetReadyOut");
				_GetReady = false;
			}
		}

		if( !_Player.IsDead )
		{
			ScoreText.text = _Player.Score.ToString();
			BestScoreText.text = _BestScore.ToString();
		}
	}

	// public methods
	public void ShowScoreScreen(int scoreVal)
	{
		bool newHighScore = false;

		// store score if greater than best
		if( PlayerPrefs.HasKey("BestScore") )
		{
			int bestScore = PlayerPrefs.GetInt("BestScore");
			if( scoreVal > bestScore && scoreVal > 0 )
			{
				PlayerPrefs.SetInt("BestScore", scoreVal);
				newHighScore = true;
			}
		}
		else if( scoreVal > 0 )
		{
			PlayerPrefs.SetInt("BestScore", scoreVal);
			newHighScore = true;
		}

		int best = PlayerPrefs.GetInt ("BestScore");

		ScoreScreen.SetActive(true);
		ScoreScreen.GetComponent<ScoreScreen>().UpdateValues(scoreVal, best, newHighScore);
	}
}
