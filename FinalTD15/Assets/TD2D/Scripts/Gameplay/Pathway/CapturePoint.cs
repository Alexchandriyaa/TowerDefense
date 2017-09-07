using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// If enemy rise this point - player will be defeated.
/// </summary>
public class CapturePoint : MonoBehaviour
{

	public GameObject flash;
	/// <summary>
	/// Raises the trigger enter2d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(other.gameObject);
		flash.SetActive (true);
		StartCoroutine (Flash());



		EventManager.TriggerEvent("Captured", other.gameObject, null);

	}
	IEnumerator Flash()
	{
		yield return new WaitForSeconds (0.4f);
		flash.SetActive (false);
	}
}
