using UnityEngine;
using System.Collections;

public class GetReadyUI : MonoBehaviour 
{
	// public member variables
	public LevelSettings LevelManager;

	// animation events
	private void OnAnimComplete()
	{
		Camera.main.GetComponent<CameraShake>().Shake ();
	}

	private void OnOutAnimComplete()
	{
		GameObject.Destroy(this.gameObject);
		LevelManager.Paused = false;
	}
}
