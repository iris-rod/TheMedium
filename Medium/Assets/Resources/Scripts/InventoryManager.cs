using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour {

  private Dictionary<string, int> items;
  private int maxSlots,nextOpenSlot;

  private bool canAddNewItem;
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

  private bool HasItem(string name)
  {
    foreach(var item in items)
    {
      if (item.Key == name)
        return true;
    }
    return false;
  }
  
  private void AddItemToSlot(string name, int quantity, Sprite icon)
  {
    Transform spots = transform.GetChild(0);
    Transform spot = spots.GetChild(nextOpenSlot).GetChild(0);
    spot.GetComponent<Slot>().AddItem(name, quantity, icon);
  }

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

}
