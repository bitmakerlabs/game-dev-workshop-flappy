using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{
	// public member variables
	public float ShakeDuration = 0.5f;
	public float ShakeSpeed = 1.0f;
	public float ShakeAmplitude = 1.0f;

	// public methods
	public void Shake()
	{
		StopAllCoroutines();
		StartCoroutine(ShakeCoroutine());
	}

	// Coroutines
	private IEnumerator ShakeCoroutine()
	{
		float alpha = 0.0f;
		Vector3 oPos = this.transform.position;

		float randomStart = Random.Range (-1000.0f, 1000.0f);

		while( alpha < ShakeDuration )
		{
			alpha += Time.deltaTime;

			float percentComplete = alpha / ShakeDuration;
			float damper = 1.0f - Mathf.Clamp (2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

			float beta = randomStart * percentComplete * ShakeSpeed;

			float x = 2*Mathf.PerlinNoise(beta, 0.0f)-1.0f;
			float y = 2*Mathf.PerlinNoise (0.0f, beta)-1.0f;

			x *= ShakeAmplitude * damper;
			y *= ShakeAmplitude * damper;

			this.transform.position = new Vector3(x, y, oPos.z);

			yield return null;
		}

		this.transform.position = oPos;
	}
}
