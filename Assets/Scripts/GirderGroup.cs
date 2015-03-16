using UnityEngine;
using System.Collections;

public class GirderGroup : MonoBehaviour
{
	// public member variables
	public bool FromPool = true;

	// private member variables
	private LevelSettings _LevelSettings;

	// Use this for initialization
	void Start () 	
	{
		_LevelSettings = (LevelSettings)GameObject.FindObjectOfType(typeof(LevelSettings));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( _LevelSettings.Paused )
			return;

		// scroll the girders
		this.transform.Translate ( Vector3.left * _LevelSettings.ScrollSpeed * Time.deltaTime * _LevelSettings.SpeedMultiplier);

		// if we're off the screen, recycle ourselves
		if( this.transform.position.x < -1*(_LevelSettings.Camera.aspect * _LevelSettings.Camera.orthographicSize + 3) )
		{
			if( FromPool )
				_LevelSettings.Pool.PoolObject(this.gameObject);
			else
				GameObject.Destroy(this.gameObject);
		}
	}
}
