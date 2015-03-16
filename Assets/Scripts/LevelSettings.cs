using UnityEngine;
using System.Collections;

public class LevelSettings : MonoBehaviour 
{
	// public member variables
	public float SpeedMultiplier = 1.0f;
	public float ScrollSpeed = 5.0f;
	public Vector2 SpawnRate = new Vector2(1.0f, 3.0f);
	public Vector2 SpawnHeightExtents = new Vector2(-2.5f, 2.5f);
	public Camera @Camera;

	public GameObject GirderPrefab;

	// private member variables
	private float _TimerCounter = 0.0f;
	private float _TimeTarget = 0.0f;
	private ObjectPool _Pool;
	private bool _Paused = true;	

	// properties
	public ObjectPool Pool
	{
		get { return _Pool; }
	}

	public bool Paused
	{
		get { return _Paused; }
		set { _Paused = value; }
	}
		
	// Use this for initialization
	void Start () 
	{
		_TimeTarget   = Random.Range (SpawnRate.x, SpawnRate.y);
		_TimerCounter = 0.0f;
			
		_Pool = GetComponent<ObjectPool>();
		_Pool.Init ();
		_Paused = true;		
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if( _Paused )
			return;

		_TimerCounter += Time.deltaTime * SpeedMultiplier;

		if( _TimerCounter >= _TimeTarget )
		{
			GameObject girder = (GameObject)_Pool.GetObjectForType("GirderGroup");
			girder.transform.position = new Vector3(@Camera.orthographicSize*@Camera.aspect + 3, Random.Range(SpawnHeightExtents.x, SpawnHeightExtents.y));
			
			_TimerCounter = 0.0f;
			_TimeTarget = Random.Range (SpawnRate.x, SpawnRate.y);
		}
	}

	void OnDestroy()
	{		
	}
}
