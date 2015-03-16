using UnityEngine;
using System.Collections;

public class AudioFader : MonoBehaviour 
{
	// public member variables
	public float AudioFadeTime = 1.5f;
	
	// coroutines
	private IEnumerator FadeAudio()
	{
		float start = 1.0f;
		float end   = 0.0f;
		
		float alpha = 0.0f;
		float step = 1.0f / AudioFadeTime;
		
		while( alpha <= 1.0f )
		{
			alpha += step * Time.deltaTime;
			audio.volume = Mathf.Lerp (start, end, alpha);
			yield return null;
		}
	}

	// public methods
	public void FadeOut()
	{
		StartCoroutine(FadeAudio());
	}
}
