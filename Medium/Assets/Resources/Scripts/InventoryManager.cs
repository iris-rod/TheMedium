using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour {

  private Dictionary<string, int> items;
  private int maxSlots,nextOpenSlot;

	// Use this for initialization
	void Awake () {
    items = new Dictionary<string,int>();
    maxSlots = transform.GetChild(0).transform.childCount;
    nextOpenSlot = 0;
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void AddItem(Pickable item)
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
  }

  public void RemoveItem(string name)
  {
    string sleekName = name.Split('(')[0].Trim();
    items.Remove(sleekName);
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i);
      if (spot.GetComponent<Slot>().GetItemName().Trim().ToLower() == sleekName.Trim().ToLower())
      {
        spot.GetComponent<Slot>().RemoveItem();
        break;
      }
    }
    CheckNextOpenSlot();
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
    Transform spot = spots.GetChild(nextOpenSlot);
    spot.GetComponent<Slot>().AddItem(name, quantity, icon);

  }

  private void UpdateItemOnSlot(string name, int quantity)
  {
    Transform spots = transform.GetChild(0);
    for (int i = 0; i < spots.childCount; i++)
    {
      Transform spot = spots.GetChild(i);
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
      Transform spot = spots.GetChild(i);
      if (spot.GetComponent<Slot>().IsFree())
      {
        nextOpenSlot = i;
        break;
      }
    }
  }

}
