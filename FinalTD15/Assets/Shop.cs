using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour {
	private UiManager gold;
	private LevelManager defeat;
	private int coin = 0;
	public Button goldUi;
	public Button defeatUi;
	private int life;
	void Start()
	{
		goldUi = GetComponent<Button> ();
	}
	public void PurchaseGold()
	{
		Debug.Log ("Purchase");
		if(goldUi.interactable == true)
		gold.AddGold (coin + 1);
		
	}
	public void PurchaseLife()
	{
		if (defeatUi.interactable == true)
			defeat.beforeLooseCounter = life;
		Debug.Log ("Purchase");
	}
	public void Another()
	{
		Debug.Log ("Another");
	}
}
