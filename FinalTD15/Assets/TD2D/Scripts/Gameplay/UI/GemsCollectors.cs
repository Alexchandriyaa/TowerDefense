
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Text;

/// <summary>
/// User interface and events manager.
/// </summary>
public class GemsCollectors : MonoBehaviour
{
	
	public Text gemsAmount;
	public float load;

	public int killScore = 0; 




	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		
		Debug.Assert(gemsAmount, "Wrong initial parameters");
	}
	void Start()
	{
		gemsAmount.text = PlayerPrefs.GetFloat("Gem :").ToString();
	}
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene("LevelChoose");
		}
	}
	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("UnitKilled", UnitKilled);

	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("UnitKilled", UnitKilled);

	}
	private float GetGem()
	{
		//LoadGem ();
		float gems;
		float.TryParse (gemsAmount.text,out gems);
		return gems;
	}
	public float SetGem(float gems)
	{
		gemsAmount.text = gems.ToString ();
		return gems;
	}
	public float AddGem(float gems)
	{
		
		load = SetGem (GetGem () + 1f);
		return load;

	}


	public void UnitKilled(GameObject obj, string param)
	{
		// If this is enemy
		if (obj.CompareTag("Enemy"))
		{
			Price price = obj.GetComponent<Price>();

				killScore++;
				if (killScore >= 4)
				{
				killScore = 0;
					AddGem (price.gem);
					PlayerPrefs.SetFloat ("Gem :", load);
				}

		}
	}
}

