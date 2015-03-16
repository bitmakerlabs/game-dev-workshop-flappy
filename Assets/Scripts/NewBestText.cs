using UnityEngine;
using System.Collections;

public class NewBestText : MonoBehaviour 
{
	// animation events
	private void OnAnimComplete()
	{
		Camera.main.GetComponent<CameraShake>().Shake ();
	}
}
