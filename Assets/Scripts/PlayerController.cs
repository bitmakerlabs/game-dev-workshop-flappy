using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	// constants
	public const float MaxUpAngle = 30.0f;
	public const float MaxDownAngle = -60.0f;

	// public member variables
	public float Gravity = -9.0f;
	public float FlapSpeed = 100.0f;
	public float TerminalVelocity = -5.0f;

	public GameObject ExplosionGroup;

	public AudioClip FlapSound;
	public AudioClip ExplosionSound;

	// private member variables
	private bool    _Falling = false;
	private float   _FallInterp = 0.0f;
	private float   _RiseInterp = 0.0f;
	private Vector3 _Velocity   = Vector3.zero;

	private LevelSettings _LevelSettings;

	private bool _Dead = false;

	private int _ScoreCount = 0;

	private bool _ScreenTap = false;

	// properties
	public int Score
	{
		get { return _ScoreCount; }
	}

	public bool IsDead
	{
		get { return _Dead; }
	}

	// Use this for initialization
	void Start () 
	{
		_Falling = false;
		_Dead = false;
		_ScoreCount = 0;

		ExplosionGroup.SetActive(false);

		_LevelSettings = (LevelSettings)GameObject.FindObjectOfType(typeof(LevelSettings));
	}
	
	// Update is called once per frame
	void Update()
	{
		if ( _Dead || _LevelSettings.Paused )		
			return;

		if( Input.GetMouseButtonDown(0) )
		{
			_ScreenTap = true;

			this.audio.clip = FlapSound;
			this.audio.Play ();
		}
	}

	void FixedUpdate () 
	{
		if ( _Dead || _LevelSettings.Paused )
		{
			// apply a boost
			_Velocity = Vector3.up * FlapSpeed * Time.deltaTime;		
			return;
		}

		// handle scoring difficulty changes
		if( _ScoreCount > 10 && _ScoreCount <= 25 )
		{
			_LevelSettings.SpeedMultiplier = 1.1f;
		}
		else if( _ScoreCount > 25 && _ScoreCount <= 40 )
		{
			_LevelSettings.SpeedMultiplier = 1.2f;
		}
		else if( _ScoreCount > 40 && _ScoreCount <= 55 )
		{
			_LevelSettings.SpeedMultiplier = 1.3f;
		}
		else if( _ScoreCount > 55 && _ScoreCount <= 75 )
		{
			_LevelSettings.SpeedMultiplier = 1.4f;
		}
		else if( _ScoreCount > 75 )
		{
			_LevelSettings.SpeedMultiplier = 1.5f;
		}

		if( !_Falling )
		{
			_RiseInterp -= Time.deltaTime*7;
			this.transform.rotation = Quaternion.Euler (new Vector3(0,0,Mathf.Lerp (MaxUpAngle,MaxDownAngle, _RiseInterp)));

			if( _RiseInterp <= 0.0f )
			{
				// check if velocity is negative
				_Falling = true;
				_FallInterp = 0.0f;
			}
		}
		// handle interpolation of ship to angle down
		else
		{
			this.transform.rotation = Quaternion.Euler (new Vector3(0,0,Mathf.Lerp(MaxUpAngle, MaxDownAngle, _FallInterp)));
			_FallInterp += Time.deltaTime;

			_FallInterp = Mathf.Min (1.0f, _FallInterp);
		}

		if( _ScreenTap )
		{
			_Falling = false;
			_RiseInterp = _FallInterp;

			// apply a boost
			_Velocity = Vector3.up * FlapSpeed * Time.deltaTime;
		}

		// apply gravity
		_Velocity += Vector3.up * Gravity * Time.deltaTime;

		this.rigidbody2D.velocity = _Velocity;

		_ScreenTap = false;
	}

	public void Die()
	{
		_Dead = true;
		ExplosionGroup.SetActive(true);
		_LevelSettings.Camera.GetComponent<CameraShake>().Shake ();
		_LevelSettings.Camera.GetComponent<AudioFader>().FadeOut();
		_LevelSettings.Paused = true; 

		this.rigidbody2D.isKinematic = true;

		this.audio.clip = ExplosionSound;
		this.audio.Play ();
		
		StartCoroutine(ShowScoreScreen());
	}

	// coroutines
	private IEnumerator ShowScoreScreen()
	{
		yield return new WaitForSeconds(0.5f);

		UIController uc = (UIController)GameObject.FindObjectOfType(typeof(UIController));
		uc.ShowScoreScreen(_ScoreCount);
	}

	// unity events
	void OnTriggerEnter2D( Collider2D other )
	{
		if( _Dead )
			return;

		if( other.CompareTag("Girder") )
		{
			// destroy us!
			this.Die();
		}
		else if( other.CompareTag ("Trigger") )
		{
			_ScoreCount++;
		}
	}
}
