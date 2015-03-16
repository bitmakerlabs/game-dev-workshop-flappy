using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour 
{
	public void OnAnimComplete()
	{
		this.audio.Play ();
		Application.LoadLevelAsync ("MainGame");
	}
}
