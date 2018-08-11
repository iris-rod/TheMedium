using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour {

  public string Name;
  public int Quantity;
  public Sprite InventoryIcon;

  private bool canBePicked = false;

  // Set the name, quantity and inventory icon of the pickable item
  public void Setup(string name, int value, Sprite icon)
  {
    Name = name;
    Quantity = value;
    InventoryIcon = icon;
  }

  // Used in the onClick listener - sets the item to be pickable
  public void Clicked()
  {
    canBePicked = true;
  }

  // Returns the bool that says if it is pickable
  public bool CanBePicked()
  {
    return canBePicked;
  }

}
