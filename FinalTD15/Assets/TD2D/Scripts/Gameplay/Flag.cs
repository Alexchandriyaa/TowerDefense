using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Flag : TowerAction 
{
	public GameObject rangeImage;


	/// <summary>
	/// Raises the enable event.
	/// </summary>

	void OnEnable()
	{
		EventManager.StartListening("UserUiClick", UserUiClick);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("UserUiClick", UserUiClick);
	}

	private void UserUiClick(GameObject obj, string param)
			{
				// If clicked on this icon
				if (obj == gameObject)
				{
					Debug.Log ("Range");
					

					Debug.Log ("RangeImage");
			StartCoroutine (Show ());
					Debug.Log ("end");
				}

			}
	IEnumerator Show()
	{
		Debug.Log ("enum");
		yield return new WaitForSeconds (0.4f);
		rangeImage.SetActive (true);
	}

			

}