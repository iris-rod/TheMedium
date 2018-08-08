using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

  private bool hasItem;
  private string itemName;
  private int itemQuantity;
  private Sprite defaultIcon;
  private Text quantityText;


  void Awake()
  {
    defaultIcon = Resources.Load("Sprites/default_icon", typeof(Sprite)) as Sprite;
    quantityText = transform.GetChild(0).GetComponent<Text>();
    itemName = "";
    itemQuantity = 0;
  }

  public void UpdateItem(int quantity)
  {
    itemQuantity += quantity;
    quantityText.text = itemQuantity.ToString();
  }

  public void AddItem(string name, int quantity, Sprite icon)
  {
    itemName = name;
    itemQuantity = quantity;
    quantityText.text = quantity.ToString();
    GetComponent<Image>().sprite = icon;
    hasItem = true;
  }

  public void RemoveItem()
  {
    itemName = "";
    itemQuantity = 0;
    quantityText.text = "0";
    GetComponent<Image>().sprite = null;
    hasItem = false;
  }
  

  public bool IsFree()
  {
    return !hasItem;
  }

  public string GetItemName()
  {
    return itemName;
  }

  public Sprite GetIcon()
  {
    return GetComponent<Image>().sprite;
  }

}
