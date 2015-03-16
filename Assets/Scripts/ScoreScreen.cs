using UnityEngine;
using System.Collections;

public class ScoreScreen : MonoBehaviour 
{
	// public member variables
	public TextMesh BestText;
	public TextMesh ScoreText;

	public GameObject RetryButton;
	public GameObject HomeButton;	

	public GameObject NewBestText;

	// private member variables
	private bool _NewBest = false;
	private int _Score = 0;

    // Use this for initialization
	void Start () 
	{
	}

	void OnDestroy()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		// check for ray touches against the buttons
		if( Input.GetMouseButtonDown(0) )
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if( hit.collider != null )
			{
				if( hit.collider.gameObject == RetryButton )
				{
					RetryButton.animation.Play("ButtonPress");
				}
				else if( hit.collider.gameObject == HomeButton )
				{
					HomeButton.animation.Play("ButtonPress");
				}				
			}
		}

	}

	// public methods
	public void UpdateValues(int score, int best, bool newBest)
	{
		ScoreText.text = score.ToString();
		BestText.text = best.ToString();

		// if there's a new best score, show NEW sign
		_NewBest = newBest;
		_Score = score;
	}

	// animation events
	public void OnScreenShake()
	{
		Camera.main.GetComponent<CameraShake>().Shake ();

		if( _NewBest )
		{
			NewBestText.SetActive(true);			
		}
	}
		
}
