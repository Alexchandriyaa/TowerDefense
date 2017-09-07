using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {
	public Button buttonComponent;
	public Text nameLabel;
	public Image iconImage;
	public Text priceText;


	private Item item;
	private ShopList scrollList;

	// Use this for initialization
	void Start () 
	{
		buttonComponent.onClick.AddListener (HandleClick);
	}

	public void Setup(Item currentItem, ShopList currentScrollList)
	{
		item = currentItem;
		nameLabel.text = item.itemName;
		iconImage.sprite = item.icon;
		priceText.text = item.price.ToString ();
		scrollList = currentScrollList;

	}

	public void HandleClick()
	{
		scrollList.TryTransferItemToOtherShop (item);
	}
}
