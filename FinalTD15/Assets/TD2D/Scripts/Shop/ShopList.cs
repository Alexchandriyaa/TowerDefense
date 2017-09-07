using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
	public string itemName;
	public Sprite icon;
	public float price = 0;
}

public class ShopList : MonoBehaviour {

	public List<Item> itemList;
	public Transform contentPanel;
	public ShopList otherShop;
	public Text gemAmount;
	public SimpleobjectPool buttonObjectPool;
	private GemsCollectors gem;


	// Use this for initialization
	void Start () 
	{
		gemAmount.text = PlayerPrefs.GetFloat("Gem :").ToString();
		RefreshDisplay ();
	}

	void RefreshDisplay()
	{
		
		AddButtons ();
	}



	private void AddButtons()
	{
		for (int i = 0; i < itemList.Count; i++) 
		{
			Item item = itemList[i];
			GameObject newButton = buttonObjectPool.GetObject();
			newButton.transform.SetParent(contentPanel);

			Buttons sampleButton = newButton.GetComponent<Buttons>();
			sampleButton.Setup(item, this);
		}
	}

	public void TryTransferItemToOtherShop(Item item)
	{
		
		if (otherShop.gem.load >= item.price) 
		{

			gem.load += item.price;
			otherShop.gem.load -= item.price;

			AddItem(item, otherShop);
			RemoveItem(item, this);

			RefreshDisplay();
			otherShop.RefreshDisplay();
			Debug.Log ("enough gold");

		}
		Debug.Log ("attempted");
	}

	void AddItem(Item itemToAdd, ShopList shopList)
	{
		shopList.itemList.Add (itemToAdd);
	}

	private void RemoveItem(Item itemToRemove, ShopList shopList)
	{
		for (int i = shopList.itemList.Count - 1; i >= 0; i--) 
		{
			if (shopList.itemList[i] == itemToRemove)
			{
				shopList.itemList.RemoveAt(i);
			}
		}
	}
}