using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour {

  private Dictionary<string, int> items;
  private int maxSlots,nextOpenSlot;

  private bool canAddNewItem;
  private bool clickedOnInventory;
  private string itemClicked;
  private ItemManager ItManager;

	// Use this for initialization
	void Awake () {
    ItManager = GameObject.Find("Manager").GetComponent<ItemManager>();
    items = new Dictionary<string,int>();
    maxSlots = transform.GetChild(0).transform.childCount;
    nextOpenSlot = 0;
    canAddNewItem = false;
  }
  
  //Used onclick funtion of items to be able to pick them up when the player gets close enough
  public void CanAdd(Pickable item)
  {
    itemClicked = item.Name;
    canAddNewItem = true;
  }

  // Add item to the inventory - if it already exists its quantity is updated, if it doesnt it is added
  // When the item is added, the index of the next slot open is updated
  public void AddItem(Pickable item)
  {
    if (canAddNewItem && item.Name == itemClicked)
    {
      string sleekName = item.Name.Split('(')[0].Trim();
      if (items.Count < maxSlots)
      {
        if (HasItem(sleekName))
        {
          items[sleekName] += item.Quantity;
          UpdateItemOnSlot(sleekName, item.Quantity);
        }
        else
        {
          items.Add(sleekName, item.Quantity);
          AddItemToSlot(sleekName, item.Quantity, item.InventoryIcon);
          CheckNextOpenSlot();
        }
      }
      canAddNewItem = false;
    }
  }

  // Remove item from the inventory
  public void RemoveItem(string name)
  {
    string sleekName = name.Split('(')[0].Trim();
    items.Remove(sleekName);
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i).GetChild(0);
      if (spot.GetComponent<Slot>().GetItemName().Trim().ToLower() == sleekName.Trim().ToLower())
      {
        spot.GetComponent<Slot>().RemoveItem();
        spot.GetComponent<ItemClickHandler>().SetSelected(false);
        break;
      }
    }
    CheckNextOpenSlot();
  }

  //check if the item exists in the inventory and it is selected
  public bool CheckForItem(string name)
  {
    string sleekName = name.Split('(')[0].Trim();
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i).GetChild(0);
      if (spot.GetComponent<Slot>().GetItemName().Trim().ToLower() == sleekName.Trim().ToLower() && spot.GetComponent<ItemClickHandler>().IsSelected())
        return true;
    }
    return false;
  }

  //check if there is any item already selected in the inventory
  public bool HasItemAlreadySelected()
  {
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i).GetChild(0);
      if (spot.GetComponent<ItemClickHandler>().IsSelected())
        return true;

    }
    return false;
  }

  public bool ClickedOnInventory()
  {
    return clickedOnInventory;
  }

  // Check if the item is in the inventory
  private bool HasItem(string name)
  {
    foreach(var item in items)
    {
      if (item.Key == name)
        return true;
    }
    return false;
  }
  
  // Add item to the next open slot
  private void AddItemToSlot(string name, int quantity, Sprite icon)
  {
    Transform spots = transform.GetChild(0);
    Transform spot = spots.GetChild(nextOpenSlot).GetChild(0);
    spot.GetComponent<Slot>().AddItem(name, quantity, icon);
  }

  // Update the quantity of the item on the slot
  private void UpdateItemOnSlot(string name, int quantity)
  {
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i).GetChild(0);
      if (spot.GetComponent<Slot>().GetItemName() == name)
      {
        spot.GetComponent<Slot>().UpdateItem(quantity);
        break;
      }
    }
  }

  // Gets the index of the next open slot
  private void CheckNextOpenSlot()
  {
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i).GetChild(0);
      if (spot.GetComponent<Slot>().IsFree())
      {
        nextOpenSlot = i;
        break;
      }
    }
  }

  void OnMouseDown()
  {
    clickedOnInventory = true;
  }

  void OnMouseUp()
  {
    Invoke("ResetClick",1f); 
  }

  void ResetClick()
  {
    clickedOnInventory = false;
  }
}
